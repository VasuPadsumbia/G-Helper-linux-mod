using GHelper.Linux.Platform.Linux;

namespace GHelper.Linux.Helpers;

/// <summary>
/// mod: CPU package power in watts, without root.
///
/// Sources, in order:
///   1. AMD APU: the integrated GPU's amdgpu hwmon publishes the whole package
///      power (CPU cores + iGPU) as power1_input labelled "PPT" (microwatts).
///      This is the same figure ryzenadj reports as the socket power.
///   2. RAPL (/sys/class/powercap/intel-rapl:0/energy_uj, Intel and AMD) when
///      it is readable; most distros restrict it to root.
/// </summary>
public static class CpuPower
{
    private static string? _ppPath;
    private static bool _ppProbed;

    private const string RaplEnergy = "/sys/class/powercap/intel-rapl:0/energy_uj";
    private static long _lastEnergyUj = -1;
    private static long _lastTicks;

    /// <summary>Current CPU package power in watts, or null when no source is readable.</summary>
    public static double? GetWatts()
    {
        var ppt = FindApuPptPath();
        if (ppt != null)
        {
            long uw = ReadLong(ppt);
            if (uw >= 0)
                return uw / 1_000_000.0;
        }
        return ReadRapl();
    }

    private static string? FindApuPptPath()
    {
        if (_ppProbed)
            return _ppPath;
        _ppProbed = true;
        try
        {
            foreach (var hwmon in Directory.GetDirectories("/sys/class/hwmon"))
            {
                if (SysfsHelper.ReadAttribute(Path.Combine(hwmon, "name")) != "amdgpu")
                    continue;
                // The integrated GPU drives the boot display; a Radeon dGPU reports its own PPT.
                if (SysfsHelper.ReadAttribute(Path.Combine(hwmon, "device", "boot_vga")) != "1")
                    continue;
                if (SysfsHelper.ReadAttribute(Path.Combine(hwmon, "power1_label")) != "PPT")
                    continue;
                var input = Path.Combine(hwmon, "power1_input");
                if (!File.Exists(input))
                    input = Path.Combine(hwmon, "power1_average");
                if (File.Exists(input))
                {
                    _ppPath = input;
                    Logger.WriteLine($"CpuPower: using APU package power {input}");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.WriteLine("CpuPower: hwmon scan failed", ex);
        }
        return _ppPath;
    }

    private static double? ReadRapl()
    {
        long energy = ReadLong(RaplEnergy);
        if (energy < 0)
            return null;
        long now = Environment.TickCount64;
        double? watts = null;
        if (_lastEnergyUj >= 0 && energy >= _lastEnergyUj && now > _lastTicks)
            watts = (energy - _lastEnergyUj) / 1000.0 / (now - _lastTicks);
        _lastEnergyUj = energy;
        _lastTicks = now;
        return watts;
    }

    private static long ReadLong(string path)
        => long.TryParse(SysfsHelper.ReadAttributeSilent(path), out long v) ? v : -1;
}
