# G-Helper Linux Mod - Lightweight control tool for Asus laptops on Linux

A modified version of [g-helper-linux](https://github.com/utajum/g-helper-linux) that keeps everything it does and adds the parts of [G-Helper for Windows](https://github.com/seerge/g-helper) that are still missing on Linux. That means reliable GPU tuning, a metrics overlay, a richer tray, advanced GPU power settings and the Windows look. It's developed and tested on the ROG Flow X13 (2022), and works on the same Asus laptops as g-helper-linux.

[![GitHub release](https://img.shields.io/github/v/release/VasuPadsumbia/G-Helper-linux-mod?cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/VasuPadsumbia/G-Helper-linux-mod/total?cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases)
[![License](https://img.shields.io/github/license/VasuPadsumbia/G-Helper-linux-mod?cacheSeconds=3600)](LICENSE)
[![Based on g-helper-linux](https://img.shields.io/badge/based%20on-g--helper--linux-blue)](https://github.com/utajum/g-helper-linux)

**[Download latest release](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)**

- [Installation](#arrow_down-installation)
- [What the mod adds](#gift-what-the-mod-adds)
- [Design documents](docs/mod/)

> **Early development.** Features are marked **done** or **planned**. Planned features are not in the current release yet.

## :gift: What the mod adds

1. **First-run fix** *(done)*: after installing, the app no longer reports "System files need attention".
2. **GPU driver rescan** *(planned)*: GPU tuning detects the NVIDIA driver again after an install or update, after resume, and after a GPU mode change. When tuning is unavailable, the app says why instead of hiding the controls.
3. **GPU tuning safety** *(planned)*: new clock offsets run for a 30-second test and revert automatically unless confirmed.
4. **GPU undervolt** *(planned)*: one-click clock cap plus core offset, like G-Helper for Windows.
5. **Advanced GPU power** *(planned)*: G-Helper for Windows ranges for Dynamic Boost and TGP. Off by default, not advised, and used at your own risk.
6. **Metrics overlay** *(planned)*: Light, Default, Full and Complete presets on X11 and Wayland, with in-game FPS through MangoHud.
7. **Richer tray** *(planned)*: CPU, GPU and battery watts, load and fan speed next to the temperatures.
8. **CPU temperature limit** *(planned)*, plus the same safe test for undervolting.
9. **Windows look** *(planned)*: the flat dark G-Helper interface with mode colours.
10. **XG Mobile panel** *(planned)*: dock status and eGPU switching.
11. **Per-app dGPU list** *(planned)*: run selected apps on the NVIDIA GPU.

### :zap: Features from g-helper-linux

Everything below comes from g-helper-linux and works the same way. See the [g-helper-linux README](https://github.com/utajum/g-helper-linux#readme) for details.

1. Performance modes: Silent - Balanced - Turbo, with custom fan curves and power limits
2. GPU modes: Eco - Standard - Optimized - Ultimate
3. Battery charge limit
4. Screen refresh rate and panel overdrive
5. NVIDIA Dynamic Boost, temperature target and clock offsets
6. AMD Curve Optimizer and Intel undervolting
7. Keyboard RGB (Aura), Anime Matrix and Slash lighting
8. XG Mobile dock lights and dock fan curve
9. ROG Ally controller settings, NumberPad, FnLock and hotkeys
10. Asus and Logitech mice and other peripherals
11. CPU and GPU temperature, fan speed and battery monitoring in the tray
12. Self-updater and autostart

### :video_game: GPU Tweaking

On NVIDIA GPUs, the core and memory clock offsets need driver 555 or newer, and the GPU must not be in Eco mode. On the Flow X13 (2022), the firmware limits the GPU to 35 W plus 5 W of Dynamic Boost.

The planned **Advanced GPU power** option sends the same higher values that G-Helper for Windows can send. The firmware may ignore or clamp them, so the app will show what was actually applied. Flashing a modified GPU VBIOS is not supported.

## :arrow_down: Installation

Requirements:
- Any modern distro with glibc 2.38+, such as Ubuntu 24.04+, Debian 13+, Fedora 39+ or Arch.
- X11 or Wayland.
- Kernel 6.2+, or 6.9+ for all features.

No Docker, distro packages or custom kernel needed.

**Install**

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash
```

Reboot once after installing.

**Uninstall**

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash -s -- --uninstall
```

Your settings in `~/.config/ghelper` are kept.

**AppImage**

```bash
curl -sL https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest/download/GHelper-x86_64.AppImage -o GHelper-x86_64.AppImage
chmod +x GHelper-x86_64.AppImage
./GHelper-x86_64.AppImage
```

**Coming from g-helper-linux or asusctl?** Keep only one control app installed. Uninstall g-helper-linux first, because this mod installs to the same places. Also stop or remove asusctl/asusd, because it controls the same hardware.

**Build from source**

```bash
sudo apt install dotnet-sdk-10.0 clang zlib1g-dev libpipewire-0.3-dev libwayland-dev pkg-config
./build.sh
sudo ./install/install-local.sh
```

### Libraries and projects used

- [g-helper-linux](https://github.com/utajum/g-helper-linux) by utajum: the project this mod is built on ([support the author](https://buymeacoffee.com/utajum))
- [G-Helper](https://github.com/seerge/g-helper) by seerge: the original Windows app
- [Avalonia UI](https://avaloniaui.net/), [RyzenAdj](https://github.com/FlyGoat/RyzenAdj), [ryzen_smu](https://github.com/amkillam/ryzen_smu), [MangoHud](https://github.com/flightlessmango/MangoHud) and others. The full list is in [docs/mod/third-party.md](docs/mod/third-party.md).

### Disclaimers

This app changes hardware settings through the same firmware interfaces Asus Armoury Crate uses. That includes power limits, fan speeds, GPU modes, clock offsets and voltages. Overclocking, undervolting and raising power limits can make your system unstable. Use them at your own risk. The software is provided as is, without warranty, and the authors are not responsible for hardware damage.

"ROG", "TUF" and "Armoury Crate" are trademarks of ASUSTeK Computer Inc. This project is not affiliated with Asus.

Licensed under GPL-3.0, the same as g-helper-linux and G-Helper.
