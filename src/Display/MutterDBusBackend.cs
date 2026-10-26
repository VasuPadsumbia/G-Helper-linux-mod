using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using GHelper.Linux.Platform.Linux;

namespace GHelper.Linux.Display;

/// <summary>
/// mod: GNOME Wayland backend for GNOME 40-47, where gdctl does not exist yet.
/// Talks to the same org.gnome.Mutter.DisplayConfig D-Bus API that GNOME
/// Settings and gdctl use, through the gdbus CLI shipped with GLib.
///
/// GetCurrentState returns GVariant text such as:
///   (uint32 1, [(('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000'),
///      [('1920x1200@120.003', 1920, 1200, 120.003, 1.0, [1.0, 2.0], {'is-current': &lt;true&gt;}), ...],
///      {'is-builtin': &lt;true&gt;, ...})],
///    [(0, 0, 1.0, uint32 0, true, [('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000')], @a{sv} {})], {...})
///
/// SetRefreshRate re-applies every logical monitor with its current mode,
/// swapping only the built-in panel's mode, via ApplyMonitorsConfig
/// (method 2 = persistent).
/// </summary>
public class MutterDBusBackend : IDisplayBackend
{
    public string Name => "mutter-dbus";
    public bool SupportsGamma => false;

    private const string Dest = "org.gnome.Mutter.DisplayConfig";
    private const string ObjectPath = "/org/gnome/Mutter/DisplayConfig";
    private const int TimeoutMs = 3000;

    private static readonly Regex SerialRegex = new(@"^\(uint32 (\d+),");
    private static readonly Regex MonitorHeaderRegex =
        new(@"\(\('([^']+)', '[^']*', '[^']*', '[^']*'\), \[");
    private static readonly Regex ModeRegex = new(
        @"\('(\d+x\d+@[\d.]+)', (\d+), (\d+), ([\d.]+), [\d.]+, \[[^\]]*\], \{([^}]*)\}\)");
    private static readonly Regex LogicalMonitorRegex = new(
        @"\((-?\d+), (-?\d+), ([\d.]+), uint32 (\d+), (true|false), \[((?:\('[^']*', '[^']*', '[^']*', '[^']*'\)(?:, )?)+)\]");
    private static readonly Regex LogicalConnectorRegex = new(@"\('([^']+)', '[^']*', '[^']*', '[^']*'\)");

    private sealed record Mode(string Id, int Width, int Height, double Rate, bool Current);
    private sealed record Monitor(string Connector, bool Builtin, List<Mode> Modes);
    private sealed record LogicalMonitor(int X, int Y, string Scale, uint Transform, bool Primary, List<string> Connectors);
    private sealed record State(uint Serial, List<Monitor> Monitors, List<LogicalMonitor> Logical);

    public int GetRefreshRate()
    {
        try
        {
            var current = FindLaptop(FetchState())?.Modes.FirstOrDefault(m => m.Current);
            return current != null ? (int)Math.Round(current.Rate) : -1;
        }
        catch (Exception ex)
        {
            Helpers.Logger.WriteLine($"MutterDBus.GetRefreshRate failed: {ex.Message}");
            return -1;
        }
    }

    public List<int> GetAvailableRefreshRates()
    {
        var rates = new List<int>();
        try
        {
            var laptop = FindLaptop(FetchState());
            var current = laptop?.Modes.FirstOrDefault(m => m.Current);
            if (laptop == null || current == null)
                return rates;

            foreach (var mode in laptop.Modes)
            {
                int hz = (int)Math.Round(mode.Rate);
                if (mode.Width == current.Width && mode.Height == current.Height && !rates.Contains(hz))
                    rates.Add(hz);
            }
            rates.Sort((a, b) => b.CompareTo(a));
        }
        catch (Exception ex)
        {
            Helpers.Logger.WriteLine($"MutterDBus.GetAvailableRefreshRates failed: {ex.Message}");
        }
        return rates;
    }

    public void SetRefreshRate(int hz)
    {
        try
        {
            Helpers.Logger.WriteLine($"MutterDBus.SetRefreshRate: requesting {hz}Hz");

            var text = FetchText();
            var plan = text != null ? PlanRefresh(text, hz) : null;
            if (plan == null)
            {
                Helpers.Logger.WriteLine("MutterDBus.SetRefreshRate: nothing to change (no panel, mode or layout)");
                return;
            }

            var result = SysfsHelper.RunCommandWithTimeout("gdbus", new[]
            {
                "call", "--session", "--dest", Dest, "--object-path", ObjectPath,
                "--method", Dest + ".ApplyMonitorsConfig",
                plan.Value.Serial.ToString(CultureInfo.InvariantCulture), "2", plan.Value.Config, "@a{sv} {}",
            }, TimeoutMs);
            Helpers.Logger.WriteLine($"MutterDBus.SetRefreshRate: {plan.Value.Config} {(result != null ? "OK" : "FAILED")}");
        }
        catch (Exception ex)
        {
            Helpers.Logger.WriteLine($"MutterDBus.SetRefreshRate failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Serial and ApplyMonitorsConfig layout that moves the built-in panel to the
    /// mode closest to <paramref name="hz"/> at its current resolution, or null
    /// when there is no panel, it is already there, or the layout can't be rebuilt.
    /// </summary>
    internal static (uint Serial, string Config)? PlanRefresh(string stateText, int hz)
    {
        var state = Parse(stateText.Trim());
        var laptop = FindLaptop(state);
        var current = laptop?.Modes.FirstOrDefault(m => m.Current);
        if (state == null || laptop == null || current == null)
            return null;

        var target = laptop.Modes
            .Where(m => m.Width == current.Width && m.Height == current.Height)
            .OrderBy(m => Math.Abs(m.Rate - hz))
            .First();
        if (target.Id == current.Id)
            return null;

        var config = BuildLogicalConfig(state, laptop.Connector, target.Id);
        return config != null ? (state.Serial, config) : null;
    }

    public void SetGamma(float r, float g, float b) { }

    public string? GetDisplayName() => FindLaptop(FetchState())?.Connector;

    // Probing

    /// <summary>True when gdbus is present and Mutter answers GetCurrentState.</summary>
    public static bool Probe() => FetchState()?.Monitors.Count > 0;

    // Internal helpers

    private static string? FetchText() => SysfsHelper.RunCommandWithTimeout("gdbus", new[]
    {
        "call", "--session", "--dest", Dest, "--object-path", ObjectPath,
        "--method", Dest + ".GetCurrentState",
    }, TimeoutMs);

    private static State? FetchState() => FetchText() is { } text ? Parse(text.Trim()) : null;

    /// <summary>Panel refresh rate in a GetCurrentState reply, or -1.</summary>
    internal static int CurrentHz(string stateText)
    {
        var current = FindLaptop(Parse(stateText.Trim()))?.Modes.FirstOrDefault(m => m.Current);
        return current != null ? (int)Math.Round(current.Rate) : -1;
    }

    private static State? Parse(string text)
    {
        var serial = SerialRegex.Match(text);
        if (!serial.Success)
            return null;

        // Logical monitors come after every physical monitor block.
        var logicalMatches = LogicalMonitorRegex.Matches(text);
        int logicalStart = logicalMatches.Count > 0 ? logicalMatches[0].Index : text.Length;

        var monitors = new List<Monitor>();
        var headers = MonitorHeaderRegex.Matches(text);
        for (int i = 0; i < headers.Count; i++)
        {
            int start = headers[i].Index;
            if (start >= logicalStart)
                break;
            int end = i + 1 < headers.Count ? Math.Min(headers[i + 1].Index, logicalStart) : logicalStart;
            var block = text[start..end];

            var modes = new List<Mode>();
            foreach (Match m in ModeRegex.Matches(block))
            {
                modes.Add(new Mode(
                    m.Groups[1].Value,
                    int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture),
                    int.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture),
                    double.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture),
                    m.Groups[5].Value.Contains("'is-current': <true>")));
            }
            bool builtin = block.Contains("'is-builtin': <true>");
            monitors.Add(new Monitor(headers[i].Groups[1].Value, builtin, modes));
        }

        var logical = new List<LogicalMonitor>();
        foreach (Match m in logicalMatches)
        {
            var connectors = LogicalConnectorRegex.Matches(m.Groups[6].Value)
                .Select(c => c.Groups[1].Value).ToList();
            logical.Add(new LogicalMonitor(
                int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture),
                int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture),
                m.Groups[3].Value,
                uint.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture),
                m.Groups[5].Value == "true",
                connectors));
        }

        return new State(uint.Parse(serial.Groups[1].Value, CultureInfo.InvariantCulture), monitors, logical);
    }

    private static Monitor? FindLaptop(State? state)
    {
        if (state == null)
            return null;
        return state.Monitors.FirstOrDefault(m => m.Builtin)
            ?? state.Monitors.FirstOrDefault(m => m.Connector.StartsWith("eDP", StringComparison.Ordinal));
    }

    /// <summary>
    /// GVariant text for ApplyMonitorsConfig's a(iiduba(ssa{sv})): every logical
    /// monitor with its current mode, except <paramref name="connector"/>,
    /// which gets <paramref name="modeId"/>. Null if a monitor's current mode
    /// is unknown (re-applying it without a mode would disable that screen).
    /// </summary>
    private static string? BuildLogicalConfig(State state, string connector, string modeId)
    {
        var sb = new StringBuilder("[");
        for (int i = 0; i < state.Logical.Count; i++)
        {
            var lm = state.Logical[i];
            if (i > 0)
                sb.Append(", ");
            sb.Append(CultureInfo.InvariantCulture,
                $"({lm.X}, {lm.Y}, {lm.Scale}, {lm.Transform}, {(lm.Primary ? "true" : "false")}, [");
            for (int j = 0; j < lm.Connectors.Count; j++)
            {
                string name = lm.Connectors[j];
                string? mode = name == connector
                    ? modeId
                    : state.Monitors.FirstOrDefault(m => m.Connector == name)?.Modes.FirstOrDefault(m => m.Current)?.Id;
                if (mode == null)
                    return null;
                if (j > 0)
                    sb.Append(", ");
                sb.Append($"('{name}', '{mode}', @a{{sv}} {{}})");
            }
            sb.Append("])");
        }
        sb.Append(']');
        return sb.ToString();
    }
}
