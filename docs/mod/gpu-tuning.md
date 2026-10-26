# GPU tuning, and the 35 W + 5 W question

## What upstream already has

g-helper-linux already contains NVIDIA clock offsets. The work is done by `vendor/gpu-helper`
(`nvml-clocks`, which calls `nvmlDeviceSetClockOffsets` and falls back to the older
`nvmlDeviceSetGpcClkVfOffset` / `nvmlDeviceSetMemClkVfOffset`) and shown in the Fans window.

The controls are **hidden** unless the `nvml-info` probe returns a core-offset range. The
probe fails, and the rows disappear without any message, when any of these are true:

- the NVIDIA driver is older than 555, so NVML has no offset API;
- the GPU is in Eco mode or asleep, so it can't be queried;
- `gpu-helper` isn't installed, or the install script wasn't re-run after an update.

That is the most likely reason you haven't seen GPU overclocking. The first job for this
fork is to **show the rows greyed out, with the reason**, instead of hiding them.

## What G-Helper for Windows offers (target behaviour)

- **Core clock offset.** The default range is −250 to +250 MHz, and up to +400 on some cards.
- **Memory clock offset.**
- **Max GPU clock cap.** A lock or limit on the core clock. Combined with a positive offset,
  this is G-Helper's "undervolt": the same clocks at a lower voltage point.
- **Dynamic Boost and temperature target**, through the ASUS firmware.

## What we build

1. A `Gpu/Tuning/` session. It writes a crash flag, applies the core offset, memory offset and
   clock cap, then starts a 30-second countdown. If the user confirms, the values are saved. If
   the user doesn't confirm, or the app closes or crashes, the values are reverted. A crash
   flag still present at the next start means the app resets to stock.
2. Ranges come from NVML for each card. They are never hard-coded.
3. Values are saved per performance mode (Silent / Balanced / Turbo), the same as upstream's
   `gpu_core` / `gpu_memory` / `gpu_clock_limit` keys.
4. This works on X11 and Wayland, because NVML doesn't need `nvidia-settings`.
5. For an AMD dGPU, the same panel is driven by `amdgpu` `pp_od_clk_voltage`, when the kernel
   has the overdrive bit set in `amdgpu.ppfeaturemask`. If that bit is not set, the panel
   explains how to enable it.

## Can we give the RTX GPU more than 35 W + 5 W?

Short answer: **only as far as the ASUS firmware will accept, and it's not advised.** By
default the app stays within the factory limits. An opt-in "Unlock GPU power" setting, described
below, sends the same higher values Windows G-Helper can send, behind a disclaimer. Here is why
there is a hard ceiling, and what helps more.

The 35 W is the GPU's base TGP. The 5 W is NVIDIA Dynamic Boost, which moves power from the
CPU to the GPU when the CPU isn't using it. Together they make the 40 W ceiling ASUS designed
the Flow X13's cooling and power delivery around. Three layers enforce it:

- **The GPU's own VBIOS** sets a maximum power limit. NVML and `nvidia-smi -pl` can only move
  the limit *below* that maximum, and on most laptops the setting is locked entirely.
- **The ASUS firmware** exposes `nv_dynamic_boost`, `nv_base_tgp` and `nv_tgp` with a min and a
  max. Windows G-Helper can't go past those maximums either. It only lets you pick a value
  inside them.
- **VBIOS modding** is the only way past both. NVIDIA GPUs since Pascal refuse unsigned VBIOS
  images, so this means cross-flashing another laptop's VBIOS. That risks bricking the GPU and
  overloading the power circuitry. **We will not support this.**

What we *can* do:

1. **Read the real limits on your machine.** If the firmware reports a maximum for
   `nv_dynamic_boost` or `nv_tgp` above what Turbo mode sets, the app can offer it. Some ASUS
   models do have headroom here. The prototype probe script will tell us.
2. **Make sure the 5 W boost actually gets used.** Dynamic Boost only gives the GPU power the
   CPU isn't using. A "GPU priority" preset lowers the CPU's sustained and fast power limits
   (`ppt_pl1_spl` / `ppt_fppt`) while gaming, so the shared budget and the heat go to the GPU.
3. **Get more performance per watt instead of more watts.** A clock cap plus a positive core
   offset (the G-Helper undervolt) usually gives higher sustained clocks within the same 40 W,
   because the GPU stops hitting its power limit so often. On a power-limited laptop GPU this
   is the real gain.
4. **Raise the temperature target**, within the firmware range, so the GPU slows down later.
5. **XG Mobile.** With the dock attached, the dock's GPU runs at its full desktop-class power.

## Advanced: "Unlock GPU power" (opt-in, at your own risk)

You asked for an option to push the GPU wattage anyway. This is the design.

**What Windows G-Helper does.** It sends ACPI values straight to the ASUS firmware.
- Dynamic Boost `0x0012009C`: 5–25 W.
- Extra GPU TGP `0x00120098`: 0–75 W, alongside the base TGP `0x00120099`.
- Temperature target `0x001200C2`: 75–87 °C.

It does this whatever the laptop model. The firmware then accepts the value, clamps it, or
ignores it. We copy that behaviour exactly, so the advanced setting can never ask for more than
Windows G-Helper can.

**How it is unlocked.**
- An "Advanced" expander sits in the GPU section of Fans + Power, and is off by default.
- Turning it on opens a disclaimer dialog with this text:
  *"Raising GPU power beyond the factory setting is not advised. It can cause higher
  temperatures, louder fans, instability, shorter battery life, and may damage your hardware.
  The firmware may ignore or clamp these values. Use entirely at your own risk."*
- Continuing requires ticking "I understand", then confirming. The choice is saved as
  `gpu_power_unlock=1`.
- The disclaimer also says that the unlocked range may hand power to the GPU that the CPU or
  battery then can't get.

**What it unlocks.**
- Dynamic Boost, extra TGP and temperature target sliders, extended to the Windows ranges above.
- Writes go through asus-armoury firmware-attributes when the value fits the range they report.
  Above that range, they go through upstream's raw WMI debugfs path, which issues the same DEVS
  call Windows makes. This path can be blocked when Secure Boot puts the kernel in lockdown; if
  so, the UI says it is blocked.
- The NVIDIA driver power limit (`nvidia-smi -pl`), up to `power.max_limit`, where the driver
  allows changing it at all.

**Safety that still applies.**
- Every value is read back after writing. The UI then shows "requested 15 W → firmware reports
  5 W", together with the actual GPU power draw from NVML, so the user can see whether it worked.
- The same 30-second test-and-revert as the clock offsets applies, plus the crash flag.
- Values are never applied at boot unless the user opts in after a passed test.
- If the GPU temperature exceeds the temperature target plus 5 °C for 10 seconds, the app drops
  back to the mode's default power.
- VBIOS flashing remains unsupported.

## Undervolting (Windows parity)

Windows G-Helper has CPU undervolt, iGPU undervolt (AMD), a CPU temperature limit and
"Auto Apply". Upstream already has `cpu_uv`, `igpu_uv` and `auto_uv` for AMD through ryzen_smu,
and Intel MSR undervolting. We add:
- The CPU temperature limit (`cpu_temp`), through ryzenadj `--tctl-temp` or the ASUS
  `PPT_TEMP9E` attribute where present.
- The same 30-second test-and-revert for undervolt values.
- GPU undervolt, which is the clock cap plus core offset above, as a one-click "Undervolt"
  preset that the user can then fine-tune.

## Open questions to check on the machine

- The output of `nvidia-smi -q -d POWER`: the default, minimum and maximum limits, and whether
  the limit can be changed at all.
- The min and max of `nv_dynamic_boost`, `nv_temp_target`, `nv_base_tgp` and `nv_tgp`, read
  from `/sys/class/firmware-attributes/asus-armoury/attributes/*/` or the
  `asus-nb-wmi` files.
- The driver version, which needs to be 555 or newer for the offset API.
