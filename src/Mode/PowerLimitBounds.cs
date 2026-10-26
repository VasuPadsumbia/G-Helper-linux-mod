using GHelper.Linux.Platform;
using GHelper.Linux.Platform.Linux;

namespace GHelper.Linux.Mode;

/// <summary>
/// Watt bounds for one CPU power-limit attribute (PL1 / PL2 / fPPT).
///
/// The asus-armoury backend publishes min_value/max_value per attribute and
/// rejects writes outside them with EINVAL - a G614JU reports 28-140 W for
/// ppt_pl1_spl. The model table in ModeControl (MinTotal / GetMaxTotal) only
/// knows the Windows floor of 5 W, so a saved 20 W passed validation and was
/// silently refused by the driver. Firmware wins when readable; the table is
/// the fallback for legacy-only kernels and non-ASUS backends.
/// </summary>
public sealed record PowerLimitBounds(int Min, int Max, bool FromFirmware)
{
    public static PowerLimitBounds Resolve(AttrRange? range, int fallbackMin, int fallbackMax)
    {
        bool hasMin = range != null && range.Min > 0;
        bool hasMax = range != null && range.Max > 0;
        int min = hasMin ? range!.Min : fallbackMin;
        int max = hasMax ? range!.Max : fallbackMax;
        if (max < min)
            return new PowerLimitBounds(fallbackMin, fallbackMax, false);
        return new PowerLimitBounds(min, max, hasMin || hasMax);
    }

    public static PowerLimitBounds For(IHardwareControl? wmi, AttrDef attr, int fallbackMin, int fallbackMax)
    {
        var bounds = Resolve(wmi?.GetAttributeRange(attr), fallbackMin, fallbackMax);

        // mod: on AMD, SetPptLimit routes PPT writes to the SMU through ryzenadj,
        // which isn't bound by the asus-armoury min_value (e.g. 15/25/35 W on a
        // GV301RE). Offer G-Helper for Windows' floor there, plus 1 W: Sanitize
        // treats exactly fallbackMin as a poisoned legacy readback (issue #151).
        int smuFloor = fallbackMin + 1;
        if (bounds.Min > smuFloor
            && RyzenPower.PptToParam(attr.LegacyName) is { } param
            && RyzenPower.IsSupported(param))
            bounds = bounds with { Min = smuFloor };

        return bounds;
    }

    public bool Contains(int watts) => watts >= Min && watts <= Max;

    /// <summary>
    /// Watts safe to send, or -1 when the saved value is a poisoned legacy
    /// floor readback (issue #151) rather than user intent. Anything else is
    /// clamped: a limit the user chose is better honoured at the nearest
    /// legal value than dropped.
    /// </summary>
    public int Sanitize(int watts, int poisonFloor)
    {
        if (watts <= poisonFloor)
            return -1;
        if (!FromFirmware && watts > Max)
            return -1;
        return Math.Clamp(watts, Min, Max);
    }
}
