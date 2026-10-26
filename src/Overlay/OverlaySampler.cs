using GHelper.Linux.Helpers;

namespace GHelper.Linux.Overlay;

/// <summary>One reading of everything the overlay can show. Null / negative = unavailable.</summary>
public sealed record OverlaySample(
    int CpuTemp, int GpuTemp,
    int CpuFanRpm, int GpuFanRpm,
    double? CpuWatts, double? GpuWatts,
    int CpuLoad, int GpuLoad,
    double RamUsedGb, int RamPercent,
    bool OnBattery, double BatteryWatts, int BatteryPercent,
    string CpuName, string? GpuName);

/// <summary>
/// mod: collects overlay data. Runs on a background thread. Never wakes a
/// sleeping dGPU: GPU load and power are only read when the GPU control
/// reports it available and the laptop is not in Eco mode (the NVIDIA control
/// itself skips nvidia-smi while the GPU is runtime-suspended).
/// </summary>
public static class OverlaySampler
{
    private static string? _cpuName;

    public static OverlaySample Read()
    {
        var wmi = App.Wmi;
        int cpuTemp = wmi?.DeviceGet(0x00120094) ?? -1; // Temp_CPU
        int gpuTemp = wmi?.DeviceGet(0x00120097) ?? -1; // Temp_GPU
        int cpuFan = wmi?.GetFanRpm(0) ?? -1;
        int gpuFan = wmi?.GetFanRpm(1) ?? -1;

        double? gpuWatts = null;
        int gpuLoad = -1;
        string? gpuName = null;
        var gpu = App.GpuControl;
        if (gpu?.IsAvailable() == true && wmi?.GetGpuEco() != true)
        {
            try
            {
                gpuName = gpu.GetGpuName();
                gpuLoad = gpu.GetGpuUse() ?? -1;
                int? w = gpu.GetCurrentPower();
                if (w is >= 0)
                    gpuWatts = w;
            }
            catch { }
        }

        var (ramUsedGb, ramPercent) = ReadRam();

        var power = App.Power;
        bool onBattery = power != null && !power.IsOnAcPower();
        double batteryWatts = power != null ? power.GetBatteryDrainRate() / 1000.0 : 0;
        int batteryPercent = power?.GetBatteryPercentage() ?? -1;

        return new OverlaySample(
            cpuTemp, gpuTemp, cpuFan, gpuFan,
            CpuPower.GetWatts(), gpuWatts,
            SystemTelemetry.GetCpuUsagePercent(), gpuLoad,
            ramUsedGb, ramPercent,
            onBattery, batteryWatts, batteryPercent,
            _cpuName ??= ShortCpuName(), gpuName != null ? ShortGpuName(gpuName) : null);
    }

    /// <summary>True when a game appears to be running: a Steam game (SteamGameId in
    /// its environment, which covers Proton), or gamescope.</summary>
    public static bool IsGameRunning()
    {
        try
        {
            foreach (var dir in Directory.EnumerateDirectories("/proc"))
            {
                if (!char.IsDigit(Path.GetFileName(dir)[0]))
                    continue;
                try
                {
                    var comm = File.ReadAllText(Path.Combine(dir, "comm")).Trim();
                    if (comm.StartsWith("gamescope", StringComparison.Ordinal))
                        return true;
                    var env = File.ReadAllBytes(Path.Combine(dir, "environ"));
                    if (System.Text.Encoding.ASCII.GetString(env).Contains("SteamGameId="))
                        return true;
                }
                catch { /* other users' processes, exited processes */ }
            }
        }
        catch { }
        return false;
    }

    private static (double usedGb, int percent) ReadRam()
    {
        try
        {
            long total = 0, available = 0;
            foreach (var line in File.ReadLines("/proc/meminfo"))
            {
                if (line.StartsWith("MemTotal:", StringComparison.Ordinal))
                    total = ParseKb(line);
                else if (line.StartsWith("MemAvailable:", StringComparison.Ordinal))
                    available = ParseKb(line);
            }
            if (total <= 0)
                return (0, -1);
            long used = total - available;
            return (used / 1024.0 / 1024.0, (int)Math.Round(100.0 * used / total));
        }
        catch
        {
            return (0, -1);
        }
    }

    private static long ParseKb(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 && long.TryParse(parts[1], out long kb) ? kb : 0;
    }

    /// <summary>"AMD Ryzen 9 6900HS with Radeon Graphics" → "Ryzen 9 6900HS".</summary>
    private static string ShortCpuName()
    {
        try
        {
            var line = File.ReadLines("/proc/cpuinfo").FirstOrDefault(l => l.StartsWith("model name", StringComparison.Ordinal));
            var name = line?.Split(':', 2)[1].Trim() ?? "CPU";
            foreach (var junk in new[] { "AMD ", "Intel(R) ", "Core(TM) ", "(R)", "(TM)", " with Radeon Graphics", " Processor" })
                name = name.Replace(junk, "");
            int at = name.IndexOf(" w/ ", StringComparison.Ordinal);
            return (at > 0 ? name[..at] : name).Trim();
        }
        catch
        {
            return "CPU";
        }
    }

    /// <summary>"NVIDIA GeForce RTX 3050 Ti Laptop GPU" → "RTX 3050 Ti".</summary>
    private static string ShortGpuName(string name)
    {
        foreach (var junk in new[] { "NVIDIA ", "GeForce ", "AMD ", "Radeon ", " Laptop GPU", " Laptop" })
            name = name.Replace(junk, "");
        return name.Trim();
    }
}
