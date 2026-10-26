using Avalonia.Media;
using GHelper.Linux.Helpers;

namespace GHelper.Linux.Overlay;

/// <summary>Overlay presets, numbered as in G-Helper for Windows (HardwareOverlay.OverlayMode).</summary>
public enum OverlayMode { Default = 0, Light = 1, Full = 2, Complete = 3 }

/// <summary>
/// mod: overlay settings. Keys, defaults and per-mode block rules are copied from
/// G-Helper for Windows (app/Overlay/HardwareOverlay.cs, app/OverlayConfig.cs) so a
/// config.json behaves the same on both.
/// </summary>
public static class OverlaySettings
{
    public static readonly Color DefaultCpuColor = Color.FromRgb(60, 220, 255);
    public static readonly Color DefaultGpuColor = Color.FromRgb(0, 255, 80);

    /// <summary>Per-mode block toggles, in the order the settings window lists them.</summary>
    public static readonly string[] BlockKeys =
    [
        "overlay_show_fps", "overlay_show_temp", "overlay_show_fans", "overlay_show_chart",
        "overlay_show_power", "overlay_show_usage", "overlay_show_ram", "overlay_names",
    ];

    public static bool Enabled
    {
        get => AppConfig.Is("overlay");
        set => AppConfig.Set("overlay", value ? 1 : 0);
    }

    public static bool GameOnly
    {
        get => AppConfig.Is("overlay_game_only");
        set => AppConfig.Set("overlay_game_only", value ? 1 : 0);
    }

    public static OverlayMode Mode
    {
        get => (OverlayMode)Math.Clamp(AppConfig.Get("overlay_mode", 0), 0, 3);
        set => AppConfig.Set("overlay_mode", (int)value);
    }

    public static int ScalePercent => Math.Clamp(AppConfig.Get("overlay_scale_percent", 100), 50, 300);
    public static int Alpha => Math.Clamp(AppConfig.Get("overlay_alpha", 128), 0, 255);

    /// <summary>Corner: bit 0 = right, bit 1 = bottom (Windows overlay_anchor encoding).</summary>
    public static int Anchor => Math.Clamp(AppConfig.Get("overlay_anchor", 0), 0, 3);
    public static int OffsetX => Math.Max(0, AppConfig.Get("overlay_offset_x", 10));
    public static int OffsetY => Math.Max(0, AppConfig.Get("overlay_offset_y", 10));

    public static Color CpuColor => ParseColor("overlay_color_cpu", DefaultCpuColor);
    public static Color GpuColor => ParseColor("overlay_color_gpu", DefaultGpuColor);

    public static string ModeKey(string key, int mode) => key + "_" + mode;

    public static bool BlockShown(string key, OverlayMode mode)
    {
        bool def = key switch
        {
            "overlay_show_fans" or "overlay_show_chart" => mode != OverlayMode.Light,
            "overlay_show_usage" => mode == OverlayMode.Full || mode == OverlayMode.Complete,
            "overlay_show_ram" => mode == OverlayMode.Complete,
            "overlay_names" => false,
            _ => true,
        };
        return AppConfig.Get(ModeKey(key, (int)mode), def ? 1 : 0) != 0;
    }

    /// <summary>-1 = only on battery, 0 = off, 1 = always.</summary>
    public static int BatteryState(OverlayMode mode) =>
        AppConfig.Get(ModeKey("overlay_show_battery", (int)mode), mode == OverlayMode.Light ? 0 : -1);

    public static void Reset()
    {
        for (int m = 0; m < 4; m++)
        {
            foreach (var key in BlockKeys)
                AppConfig.Remove(ModeKey(key, m));
            AppConfig.Remove(ModeKey("overlay_show_battery", m));
        }
        foreach (var key in new[] { "overlay_scale_percent", "overlay_alpha", "overlay_color_cpu",
                     "overlay_color_gpu", "overlay_anchor", "overlay_offset_x", "overlay_offset_y" })
            AppConfig.Remove(key);
    }

    public static string ToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";

    private static Color ParseColor(string key, Color fallback)
    {
        string? hex = AppConfig.GetString(key);
        if (string.IsNullOrEmpty(hex))
            return fallback;
        return Color.TryParse(hex.StartsWith('#') ? hex : "#" + hex, out var c) ? c : fallback;
    }
}
