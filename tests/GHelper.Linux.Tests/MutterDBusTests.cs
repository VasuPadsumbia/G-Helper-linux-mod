using GHelper.Linux.Display;

namespace GHelper.Linux.Tests;

public static class MutterDBusTests
{
    // Real GetCurrentState reply from a ROG Flow X13 GV301RE on GNOME 46 (trimmed mode list).
    internal const string FlowX13 =
        "(uint32 1, [(('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000'), ["
        + "('1920x1200@120.003', 1920, 1200, 120.003173828125, 1.0, [1.0, 2.0], {'is-current': <true>, 'is-preferred': <true>}), "
        + "('1920x1200@60.002', 1920, 1200, 60.0015869140625, 1.0, [1.0, 2.0], {}), "
        + "('1920x1080@120.003', 1920, 1080, 120.003173828125, 1.0, [1.0, 2.0], {}), "
        + "('800x600@120.003', 800, 600, 120.003173828125, 1.0, [1.0], {})], "
        + "{'is-underscanning': <false>, 'is-builtin': <true>, 'display-name': <'Built-in display'>})], "
        + "[(0, 0, 1.0, uint32 0, true, [('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000')], @a{sv} {})], "
        + "{'renderer': <'native'>, 'layout-mode': <uint32 2>, 'legacy-ui-scaling-factor': <1>})";

    // Same panel plus an external monitor to its right, which must keep its own mode.
    internal const string WithExternal =
        "(uint32 7, [(('HDMI-1', 'DEL', 'U2720Q', 'ABC'), ["
        + "('3840x2160@60.000', 3840, 2160, 60.0, 1.0, [1.0, 2.0], {'is-current': <true>})], "
        + "{'is-builtin': <false>, 'display-name': <'Dell 27\"'>}), "
        + "(('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000'), ["
        + "('1920x1200@120.003', 1920, 1200, 120.003173828125, 1.0, [1.0, 2.0], {'is-current': <true>}), "
        + "('1920x1200@60.002', 1920, 1200, 60.0015869140625, 1.0, [1.0, 2.0], {})], "
        + "{'is-builtin': <true>})], "
        + "[(0, 0, 1.0, uint32 0, true, [('eDP-2', 'SHP', 'LQ134N1JW55', '0x00000000')], @a{sv} {}), "
        + "(1920, 0, 2.0, uint32 0, false, [('HDMI-1', 'DEL', 'U2720Q', 'ABC')], @a{sv} {})], {})";

    public static void RunAll()
    {
        Harness.Scenario("Mutter: current panel rate", _ =>
            Harness.AssertEqual(120, MutterDBusBackend.CurrentHz(FlowX13), "Hz"));
        Harness.Scenario("Mutter: 60Hz plan keeps layout and serial", _ =>
        {
            var plan = MutterDBusBackend.PlanRefresh(FlowX13, 60);
            Harness.Assert(plan != null, "plan exists");
            Harness.AssertEqual(1u, plan!.Value.Serial, "serial");
            Harness.AssertEqual("[(0, 0, 1.0, 0, true, [('eDP-2', '1920x1200@60.002', @a{sv} {})])]",
                plan.Value.Config, "config");
        });
        Harness.Scenario("Mutter: no change when already at target", _ =>
            Harness.AssertEqual(false, MutterDBusBackend.PlanRefresh(FlowX13, 120).HasValue, "plan"));
        Harness.Scenario("Mutter: external monitor keeps its mode", _ =>
        {
            var plan = MutterDBusBackend.PlanRefresh(WithExternal, 60);
            Harness.Assert(plan != null, "plan exists");
            Harness.AssertEqual(
                "[(0, 0, 1.0, 0, true, [('eDP-2', '1920x1200@60.002', @a{sv} {})]), "
                + "(1920, 0, 2.0, 0, false, [('HDMI-1', '3840x2160@60.000', @a{sv} {})])]",
                plan!.Value.Config, "config");
        });
        Harness.Scenario("Mutter: garbage input is ignored", _ =>
            Harness.AssertEqual(-1, MutterDBusBackend.CurrentHz("Error: no such object"), "Hz"));
    }
}
