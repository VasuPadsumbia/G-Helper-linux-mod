# Implementation design

## 1. Base: a fork of g-helper-linux

This repository (https://github.com/VasuPadsumbia/G-Helper-linux-mod) is a git fork of
https://github.com/utajum/g-helper-linux with its full history. The `upstream` remote tracks
upstream's `master` branch. Our branch is also `master`, because upstream hard-codes that name
in its installer, updater and release workflow.

- We use upstream's README, installer, build, release and project description. The README gains
  one "MOD EDITION" section, and the install and update URLs point at this fork.
- We use upstream's code: C# / .NET 10, Avalonia, a Native AOT single binary plus AppImage, the
  `vendor/gpu-helper` root helper, and the udev rules and sudoers drop-in. Upstream is merged
  regularly with `git fetch upstream && git merge upstream/master`.
- New code goes in new files. Edits to upstream files stay small and are listed in CHANGELOG.

The UI follows G-Helper for Windows (https://github.com/seerge/g-helper). The goal is full
feature parity with the Windows app.

## 2. Architecture (inherited)

```
 ghelper (user process, Avalonia tray app)
   UI/Views ─ Mode ─ Fan ─ Battery ─ Display ─ USB (Aura, XGM, AniMe) ─ Input ─ Overlay (mod)
   Gpu/GPUModeControl ─ Gpu/NVidia ─ Gpu/AMD ─ Gpu/GpuDriverWatcher (mod) ─ Gpu/Tuning (mod)
   Platform/Linux/Asus (sysfs + firmware-attributes + raw WMI)  Platform/Linux/RyzenPower
        │ sysfs (udev grants write)     │ sudoers / pkexec           │ subprocess / pipe
        ▼                               ▼                            ▼
   /sys/... asus-nb-wmi,           gpu-helper (C, root)        ryzenadj, audio-helper,
   asus-armoury, amdgpu, hwmon     NVML, PCI, WMI, EC          overlay-helper (mod), MangoHud
```

There is no always-on daemon. The tray app is the only long-lived process, and root work
happens in short helper calls. A systemd oneshot applies pending GPU mode changes at boot.

## 3. Hardware detection

This is automatic and inherited from upstream. The app reads the DMI model, finds the dGPU
vendor on PCI (NVIDIA, AMD or none), detects the CPU vendor and family, and probes each ASUS
attribute in this order: asus-armoury, then legacy asus-nb-wmi, then raw WMI. Whatever is
missing is hidden. **Change:** GPU capabilities are re-probed whenever the driver situation
changes. See [driver-detection.md](driver-detection.md).

## 4. Kernel and distro independence

- Core features use only the stock in-kernel drivers. `ryzen_smu` (for Curve Optimizer) and
  MangoHud (for in-game FPS) are optional.
- Every feature probes its interface and degrades gracefully.
- There are no distro packages: one binary or AppImage, plus the upstream installer, which works
  on any glibc 2.38+ distro. NixOS uses the flake.
- No Docker.

## 5. asusctl

There is no dependency on asusctl and no fork of it. Upstream already talks to sysfs directly.
The installer will detect `asusd` and `supergfxd` and offer to disable them, because they fight
over the same controls. asusctl serves only as reference material. Any code we port from it gets
a link and a licence note in [third-party.md](third-party.md).

## 6. What we add (mod features)

1. **Windows look**: theme and layout. See [ui.md](ui.md).
2. **Hardware overlay**: the Windows presets and items, a native desktop overlay, and MangoHud
   for in-game FPS. See [overlay.md](overlay.md).
3. **Richer tray**: power, load, fan and battery, alongside the temperatures. See
   [overlay.md](overlay.md).
4. **Driver re-scan**, with an explanation whenever GPU tuning is unavailable. See
   [driver-detection.md](driver-detection.md).
5. **GPU tuning safety**: a 30-second test with automatic revert, and a crash flag. See
   [gpu-tuning.md](gpu-tuning.md).
6. **Advanced GPU power unlock**, using Windows G-Helper ranges, opt-in and behind a disclaimer.
   See [gpu-tuning.md](gpu-tuning.md).
7. **Undervolting parity**: CPU temperature limit, test-and-revert, and a GPU undervolt preset.
8. **XG Mobile panel** and a **per-app dGPU offload list**.
9. **Remaining Windows parity**, to be checked feature by feature: visual modes and colour gamut,
   ELMB, clamshell mode, touchscreen toggle, ASUS keyboard and headset peripherals, FnLock and
   extra hotkeys.

## 7. Safety rules

- Risky values are GPU offsets, GPU power unlock, CPU and iGPU undervolt, and temperature
  limits. They are applied as "test 30 s → keep, or revert automatically".
- A crash flag is written before applying a risky value. If it is still present at the next
  start, everything resets to stock.
- Risky values are never applied at boot unless the user opts in after a passed test.
- Values are clamped to the ranges that the driver or firmware reports. The exception is the
  advanced unlock, which uses Windows G-Helper's ranges after the user accepts the disclaimer.
