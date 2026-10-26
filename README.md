<p align="center">
  <a href="https://github.com/VasuPadsumbia/G-Helper-linux-mod">
    <img src="https://capsule-render.vercel.app/api?type=venom&height=200&text=G-HELPER%20LINUX%20MOD&fontSize=56&color=0:0d1117,100:00ff41&stroke=00ff41&fontColor=00ff41&animation=twinkling&desc=%3E_%20BASED%20ON%20G-HELPER%20LINUX%20%7C%20STATUS%3A%20IN%20DEVELOPMENT&descAlignY=75&descSize=16&fontFamily=monospace" />
  </a>
</p>

<div align="center">

[![GitHub Release](https://img.shields.io/github/v/release/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&color=4a9eff&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)
[![Total Binary Downloads](https://img.shields.io/github/downloads/VasuPadsumbia/G-Helper-linux-mod/ghelper?style=for-the-badge&color=28c840&cacheSeconds=3600&label=total%20binary%20downloads)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases)
[![Total AppImage Downloads](https://img.shields.io/github/downloads/VasuPadsumbia/G-Helper-linux-mod/GHelper-x86_64.AppImage?style=for-the-badge&color=28c840&cacheSeconds=3600&label=total%20appimage%20downloads)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases)
[![License](https://img.shields.io/github/license/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&color=a0c8ff&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/blob/master/LICENSE)
[![Upstream](https://img.shields.io/badge/based_on-g--helper--linux-ff8c42?style=for-the-badge)](https://github.com/utajum/g-helper-linux)
[![Stars](https://img.shields.io/github/stars/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&color=f0c040&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/stargazers)

</div>

```
 ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
         ╔══[ SYSTEM ]════════════════════════════════╗
         ║  >_ BASE:   G-HELPER LINUX                 ║
         ║  >_ TARGET: G-HELPER FOR WINDOWS PARITY    ║
         ║  >_ STATUS: IN DEVELOPMENT...              ║
         ╚════════════════════════════════════════════╝
```

## `░▒▓█ ╔══[ WHAT IS THIS ]══╗ █▓▒░`

**G-Helper Linux Mod** is a modified version of
[g-helper-linux](https://github.com/utajum/g-helper-linux) by utajum, the ASUS laptop control app for Linux.

Everything g-helper-linux does is still here, unchanged. This mod adds the parts of
[G-Helper for Windows](https://github.com/seerge/g-helper) that the Linux port doesn't have yet,
or doesn't do well. That means GPU tuning that is detected reliably, a hardware overlay, a richer
tray, an advanced GPU power option and the Windows look. It's developed and tested on an
**ASUS ROG Flow X13 (2022)** with a Ryzen 6000 CPU and an NVIDIA RTX GPU.

> This mod is in early development. The features below are marked **DONE** or **PLANNED**.
> Planned features aren't in the current release yet.

---

## `░▒▓█ ╔══[ WHAT THE MOD ADDS ]══╗ █▓▒░`

```
┌──────────────────────────────────────────────────────────────────────┐
│  [DONE]     First-run fix      Installer writes udev rules and       │
│                                modules-load.d the way the app        │
│                                checks them (no "system files need    │
│                                attention" popup after install)       │
│  [PLANNED]  GPU driver rescan  GPU tuning re-detects the driver      │
│                                after install / update / resume /     │
│                                mode change; shows WHY it's off       │
│  [PLANNED]  GPU tuning safety  30 s test, auto-revert, crash reset   │
│  [PLANNED]  GPU undervolt      One-click clock cap + core offset     │
│  [PLANNED]  Advanced GPU power Windows G-Helper ranges for Dynamic   │
│                                Boost / TGP — opt-in, own risk        │
│  [PLANNED]  Hardware overlay   Light / Default / Full / Complete,    │
│                                FPS via MangoHud, X11 + Wayland       │
│  [PLANNED]  Richer tray        CPU / GPU / battery watts, load, fan  │
│  [PLANNED]  CPU temp limit     Plus test-and-revert for undervolt    │
│  [PLANNED]  Windows look       G-Helper flat dark UI + mode colours  │
│  [PLANNED]  XG Mobile panel    Dock status, eGPU switch              │
│  [PLANNED]  Per-app dGPU list  Run chosen apps on the NVIDIA GPU     │
└──────────────────────────────────────────────────────────────────────┘
```

The design for each feature is in **[docs/mod/](docs/mod/)**.

### `╠══[ ADVANCED GPU POWER — READ THIS ]══╣`

Laptop GPUs are limited by their own firmware and by ASUS's firmware. On the Flow X13, the limit is
35 W plus 5 W of Dynamic Boost. The planned advanced option sends the same higher values that
G-Helper for Windows can send. **It is off by default. It is not advised, and you use it at your
own risk.** The firmware may ignore or clamp those values, so the app will show what the firmware
actually applied. Flashing a modified GPU VBIOS is not supported and never will be.

---

## `░▒▓█ ╔══[ INHERITED FROM G-HELPER LINUX ]══╗ █▓▒░`

These features all come from g-helper-linux:
- Performance modes and custom fan curves.
- Battery charge limit.
- GPU modes (Eco / Standard / Optimized / Ultimate).
- CPU power limits, NVIDIA Dynamic Boost and NVIDIA clock offsets.
- AMD Curve Optimizer and Intel undervolting.
- Refresh rate and panel overdrive.
- Aura RGB, AniMe Matrix and Slash.
- Audio DSP.
- ROG Ally support.
- XG Mobile dock lights.
- Steam integration.
- NumberPad.
- Peripherals.
- Self-updater.

For how each of these works, the GPU mode reboot rules, the kernel feature matrix and the
project structure, see the
**[g-helper-linux README](https://github.com/utajum/g-helper-linux#readme)**. Those parts work the same here.

---

## `░▒▓█ ╔══[ DISCLAIMER ]══╗ █▓▒░`

```
╔══[ TERMS OF USE ]══════════════════════════════════════════════╗
║                                                                 ║
║  This software writes to ASUS firmware through kernel sysfs     ║
║  attributes and ACPI/WMI methods — the same interfaces ASUS     ║
║  Armoury Crate uses on Windows. It changes GPU power state,     ║
║  fan speeds, power limits, clocks and voltages.                 ║
║                                                                 ║
║  Incorrect or interrupted writes can leave hardware in an       ║
║  unexpected state; in rare cases a CMOS reset may be needed.    ║
║  Overclocking, undervolting and raising power limits can make   ║
║  the system unstable or shorten hardware life.                  ║
║                                                                 ║
║  Provided AS-IS with no warranty. The authors are not           ║
║  responsible for hardware damage.                               ║
║                                                                 ║
╚═════════════════════════════════════════════════════════════════╝
```

---

## `░▒▓█ ╔══[ REQUIREMENTS ]══╗ █▓▒░`

Same as g-helper-linux:
- Any modern distro with **glibc 2.38+**, such as Ubuntu 24.04+, Debian 13+, Fedora 39+ or Arch.
- X11 or Wayland.
- Kernel 6.2+ recommended, 6.9+ for all features.
- The `asus-nb-wmi` module, which ASUS laptops load by default.

The mod doesn't need Docker, distro packages or a custom kernel.

---

## `░▒▓█ ╔══[ INSTALLATION ]══╗ █▓▒░`

### `╠══[ ONE-LINER INSTALL ]══╣`

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash
```

This installs the app to `/opt/ghelper`, along with the hardware access rules, the GPU helper and
a menu entry. Reboot once afterwards so the hardware permissions take full effect.

### `╠══[ SWITCHING FROM G-HELPER LINUX OR ASUSCTL ]══╣`

Keep only one of these apps installed. This mod installs to the same places as g-helper-linux,
so uninstall g-helper-linux first. **asusctl / asusd** controls the same hardware, and the two will
overwrite each other's settings, so stop or uninstall it as well.

### `╠══[ UNINSTALL ]══╣`

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash -s -- --uninstall
```

Your settings in `~/.config/ghelper` are kept.

### `╠══[ APPIMAGE ]══╣`

```bash
curl -sL https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest/download/GHelper-x86_64.AppImage -o GHelper-x86_64.AppImage
chmod +x GHelper-x86_64.AppImage
./GHelper-x86_64.AppImage
```

### `╠══[ BUILD FROM SOURCE ]══╣`

```bash
# Ubuntu/Debian
sudo apt install dotnet-sdk-10.0 clang zlib1g-dev libpipewire-0.3-dev libwayland-dev pkg-config

./build.sh                      # Native AOT build → dist/ghelper
sudo ./install/install-local.sh # install the local build
```

For Fedora and Arch packages and the developer build options, see the
[g-helper-linux build guide](https://github.com/utajum/g-helper-linux#readme).

---

## `░▒▓█ ╔══[ STAYING IN SYNC ]══╗ █▓▒░`

This repository is a git fork that keeps g-helper-linux's full history. Upstream fixes are merged
in regularly, and the mod's own changes are kept in separate files wherever possible. Fixes that
help everyone are offered back to g-helper-linux.

---

## `░▒▓█ ╔══[ CREDITS ]══╗ █▓▒░`

- [g-helper-linux](https://github.com/utajum/g-helper-linux) by utajum: the project this mod is
  built on ([support the author](https://buymeacoffee.com/utajum)).
- [G-Helper](https://github.com/seerge/g-helper) by seerge: the original Windows app and the
  feature reference.
- Every other project the mod uses is linked in
  **[docs/mod/third-party.md](docs/mod/third-party.md)**, including RyzenAdj, ryzen_smu, MangoHud
  and Avalonia.

---

## `░▒▓█ ╔══[ LICENSE ]══╗ █▓▒░`

GPL-3.0, the same as g-helper-linux and G-Helper.

<div align="center">

```
  ░▒▓█ END OF TRANSMISSION █▓▒░
```

</div>
