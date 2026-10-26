# The AMD parts explained

"AMD setup" in g-helper-linux covers three separate things. They're easy to mix up, so
here is each one in plain terms, followed by a recommendation.

## 1. AMD CPU power limits (ryzenadj)

A Ryzen laptop CPU has power limits: sustained, slow boost and fast boost. ASUS lets you
change them through the firmware, using `ppt_pl1_spl`, `ppt_pl2_sppt` and `ppt_fppt` in
asus-wmi. This is the main path, and it needs nothing extra.

Some models don't expose those firmware files, or they expose only some of them. For those,
upstream bundles **ryzenadj**, a small static program. It talks to the CPU's internal
management unit (the SMU) directly and sets the same limits, plus temperature and current
limits. It ships inside the app, so nothing needs installing. It does need root, and it
can be blocked when Secure Boot puts the kernel in lockdown mode.

**Recommendation: keep it.** It is a fallback that costs nothing when it isn't used. On the
Flow X13 the firmware files should be enough, and ryzenadj only adds the extra temperature
and current controls.

## 2. AMD Curve Optimizer undervolting (ryzen_smu)

This lowers the CPU's voltage at every clock speed, so it runs cooler and boosts longer. It's
the same thing as "Curve Optimizer" in G-Helper for Windows. Your Ryzen 6000 (Rembrandt) is on
upstream's supported list.

It needs the **ryzen_smu kernel module**, which isn't part of the normal kernel. You build it
with DKMS. If Secure Boot is on, you also have to sign the module. This is the one place where
the app depends on something outside the stock kernel, which is why upstream hides the feature
completely unless the module is loaded.

The safety behaviour is good. A read-only probe runs first, values are limited to −40, every
write is read back to check it, and everything resets on reboot.

**Recommendation: keep it exactly as upstream has it.** It is optional and hidden by
default. Our only addition is a short setup guide for your distro, plus the same
30-second test-and-revert used for GPU offsets.

## 3. AMD dGPU support (amdgpu)

Some ROG laptops have an AMD Radeon dGPU instead of an NVIDIA one. Upstream detects this
automatically and reads temperature, clocks and power through the standard `amdgpu` driver.
That driver is in every kernel. Nothing needs setting up.

**Recommendation: keep it.** That covers your requirement that "AMD-GPU laptops should be
automatic". We add AMD overclocking through `pp_od_clk_voltage` to the same GPU tuning panel,
as described in [gpu-tuning.md](gpu-tuning.md).

## Summary for your machine

The Flow X13 2022 has an AMD **CPU** and an NVIDIA **GPU**. That means parts 1 and 2 apply to
you, and part 3 doesn't. Part 1 works without any setup. Part 2 is optional and comes last
in the roadmap, because it's the riskiest.
