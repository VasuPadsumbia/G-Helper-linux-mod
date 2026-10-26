# Third-party projects

Every project we fork, vendor, port code from, or depend on at runtime is listed here with a
link. Anything copied into the tree also lives under `vendor/<name>/` with its own `LICENSE`
and a `VERSION` file that records the source URL and commit.

## Forked (our base)

- **g-helper-linux** by utajum: https://github.com/utajum/g-helper-linux (GPL-3.0).
  This repository (https://github.com/VasuPadsumbia/G-Helper-linux-mod) is a fork of it,
  imported with its full history. We track it as the `upstream` git remote, branch `master`.

## Reference (ported behaviour, not vendored)

- **G-Helper** by seerge: https://github.com/seerge/g-helper (GPL-3.0).
  This is the feature reference: GPU offset ranges, mode logic and UI behaviour.

## Vendored by upstream (inherited)

- **RyzenAdj** by FlyGoat: https://github.com/FlyGoat/RyzenAdj. A static CLI binary in
  `vendor/ryzenadj/` (LGPL-3.0).
- **wlr-randr**: https://gitlab.freedesktop.org/emersion/wlr-randr. Source in
  `vendor/wlr-randr/` (MIT).
- **RNNoise** by Xiph.Org: https://github.com/xiph/rnnoise. Source in `audio-helper/rnnoise/`
  (BSD-3-Clause).

## Optional runtime dependencies (not bundled)

- **ryzen_smu**: https://github.com/amkillam/ryzen_smu. A kernel module, needed only for Curve
  Optimizer (GPL-2.0).
- **MangoHud**: https://github.com/flightlessmango/MangoHud. Provides the in-game overlay and FPS.
  We only generate its config file (MIT).
- **wlr-layer-shell protocol**: https://gitlab.freedesktop.org/wlroots/wlr-protocols. The
  protocol XML will be vendored into `overlay-helper/` when it's built (MIT).
- **GameMode**: https://github.com/FeralInteractive/gamemode. Used only through its D-Bus
  interface, to detect when a game is running (BSD-3-Clause).
- **NVIDIA driver / NVML** (`libnvidia-ml.so.1`). Loaded at runtime with `dlopen`. Driver 555+
  is needed for clock offsets.
- **asus-wmi / asus-nb-wmi / asus-armoury**. Linux kernel drivers:
  https://github.com/torvalds/linux/tree/master/drivers/platform/x86

## Consulted, not depended on

- **asusctl / asusd**: https://gitlab.com/asus-linux/asusctl. Used as a reference for
  protocols and model quirks. We don't fork it or require it. See design.md §5.
- **supergfxctl**: https://gitlab.com/asus-linux/supergfxctl. A reference for GPU mode
  switching and XG Mobile.
- **LACT**: https://github.com/ilya-zlobintsev/LACT. A reference for NVML and amdgpu
  overclocking.

Licences marked here are as understood at design time. Re-check each one when its code is
actually brought in.
