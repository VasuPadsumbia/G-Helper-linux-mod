# G-Helper Linux Mod - Lightweight control tool for Asus laptops on Linux

A small, fast Armoury Crate alternative for Asus ROG and TUF laptops on Linux. It is a modified version of [g-helper-linux](https://github.com/utajum/g-helper-linux) that keeps everything g-helper-linux does, and works towards **full feature parity with [G-Helper for Windows](https://github.com/seerge/g-helper)**. It adds:
- GPU overclocking that detects your GPU reliably;
- safe undervolting with automatic revert;
- an in-game metrics overlay;
- a richer tray;
- advanced GPU power controls;
- the same clean look as the Windows app.

It's developed and tested on the **ROG Flow X13 (2022)**, with a Ryzen 9 6900HS and an RTX 3050 Ti. It supports the same models as g-helper-linux: Zephyrus G14 / G15 / G16 / M16, Flow X13 / X16 / Z13, Strix and Scar, TUF, ProArt, ROG Ally and more.

[![GitHub release](https://img.shields.io/github/v/release/VasuPadsumbia/G-Helper-linux-mod?cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/VasuPadsumbia/G-Helper-linux-mod/total?cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases)
[![License](https://img.shields.io/github/license/VasuPadsumbia/G-Helper-linux-mod?cacheSeconds=3600)](LICENSE)
[![Based on g-helper-linux](https://img.shields.io/badge/based%20on-g--helper--linux-blue)](https://github.com/utajum/g-helper-linux)
[![Feature reference: G-Helper](https://img.shields.io/badge/feature%20reference-G--Helper%20for%20Windows-red)](https://github.com/seerge/g-helper)

**[⬇️ Download latest release](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)** &nbsp;·&nbsp; **[📦 One-line install](#arrow_down-installation)** &nbsp;·&nbsp; **[🗺️ Roadmap](docs/mod/roadmap.md)** &nbsp;·&nbsp; **[🧩 Design docs](docs/mod/)**

**⭐ Like the project? Star it and help spread the word.**

- [What's new in the mod](#sparkles-whats-new-in-the-mod)
- [Features](#zap-features)
- [GPU Tweaking](#video_game-gpu-tweaking)
- [Installation](#arrow_down-installation)
- [Troubleshooting / FAQ](#wrench-troubleshooting--faq)
- [Keybindings](#keyboard-keybindings)

> [!IMPORTANT]
> **Early development.** Each mod feature below is marked ✅ **done**, 🚧 **in progress** or 🗓️ **planned**. Only ✅ features are in the current release. Everything inherited from g-helper-linux works today.

## :gift: Advantages

1. **One app, no daemon.** A single self-contained binary or AppImage. No Docker, no distro packages, no custom kernel.
2. **Works on any distro.** Ubuntu, Debian, Fedora, Arch, openSUSE, NixOS and more: anything with glibc 2.38+.
3. **Uses the stock kernel drivers.** It talks to the same `asus-wmi` / `asus-armoury` firmware interfaces as Armoury Crate, and needs no asusctl or supergfxctl.
4. **Detects your hardware automatically.** NVIDIA or AMD dGPU, AMD or Intel CPU, and which firmware controls your model supports. Unsupported controls are hidden.
5. **Safe by default.** Overclocks and undervolts are tested before they stick, and are never applied at boot unless you ask.
6. **Familiar.** The same modes, fan curves and settings file format as G-Helper for Windows.

## :sparkles: What's new in the mod

### ✅ First-run fix
Upstream's one-line installer left two system files in a form the app didn't recognise. Right after a fresh install, it showed a *"System files need attention"* popup about the hardware access rules and kernel modules. The mod's installer writes both exactly as the app expects. It also makes the `uinput` and `i2c-dev` modules load at every boot, not just during install.

### 🚧 GPU detection that doesn't give up
Linux puts the NVIDIA GPU to sleep when nothing uses it. Upstream only checks for the GPU once, at startup, without waking it. If the GPU was asleep at that moment, the app shows *"No NVIDIA GPU detected"* for the rest of the session, even while games are running on it. The mod:
- reads the GPU model from the driver without waking the GPU, so detection works while it's asleep;
- **re-checks automatically** after a driver install or update, after resume from sleep, and after a GPU mode change. A **Rescan** button is also planned;
- never hides GPU controls without a reason. It shows why they're unavailable (driver too old, Eco mode, helper missing) and how to fix it.

### 🗓️ GPU tuning safety
New clock offsets, power settings and undervolts run for a **30-second test**. If you don't confirm, or the app or system crashes, they revert to stock automatically.

### 🗓️ One-click GPU undervolt
This is G-Helper's undervolt recipe: a max clock cap plus a positive core offset. You get the same or better clocks at lower voltage, which on a power-limited laptop GPU means higher sustained performance.

### 🗓️ Advanced GPU power
An opt-in panel that uses G-Helper for Windows' ranges: Dynamic Boost up to 25 W, extra TGP up to 75 W and a temperature target from 75 to 87 °C. See [GPU Tweaking](#video_game-gpu-tweaking) for the details and risks.

### 🗓️ Metrics overlay
The Windows G-Helper overlay brought to Linux:
- Light, Default, Full and Complete presets;
- FPS, temperatures, fan, chart, power, load, RAM and battery;
- size, transparency, colours and position.

It works on X11 and Wayland (KDE, GNOME, Hyprland, Sway, COSMIC). In-game FPS comes from [MangoHud](https://github.com/flightlessmango/MangoHud), configured automatically to match your overlay settings.

### 🗓️ Richer tray
Optional tray icons for **CPU W, GPU W and battery W**, next to the existing temperature icons. A full summary tooltip shows temperatures, load, power, fan speed and battery rate.

### 🗓️ CPU temperature limit and safe undervolting
Adds a CPU temperature limit, like the Windows app. The same 30-second test is added to AMD Curve Optimizer and iGPU undervolting.

### 🗓️ The Windows look
The flat dark G-Helper interface:
- square mode tiles with a coloured border on the active mode (green Silent, blue Balanced, red Turbo, orange Custom);
- the same Fans + Power layout as Windows.

### 🗓️ XG Mobile panel and per-app dGPU list
- XG Mobile dock status (attached / locked) with eGPU switching and a logout reminder.
- A list of apps that should always run on the NVIDIA GPU.

## :zap: Features

1. Performance modes: Silent - Balanced - Turbo, plus custom modes, each with its own fan curves and power limits
2. GPU modes: Eco - Standard - Optimized - Ultimate (MUX, on supported models)
3. Custom fan curve editor: 8 points per fan, per mode
4. CPU power limits (sustained / slow / fast PPT), CPU boost and energy preference
5. NVIDIA GPU tuning: core and memory clock offsets, clock lock, Dynamic Boost and temperature target
6. AMD CPU undervolting (Curve Optimizer) and Intel undervolting
7. Battery charge limit to preserve battery health
8. Screen refresh rate and panel overdrive, Mini-LED modes, brightness and gamma
9. Keyboard RGB (Aura) modes and colours, Anime Matrix and Slash lighting
10. XG Mobile dock lights and dock fan curve
11. CPU, GPU, fan and battery monitoring in the tray and a system monitor
12. BIOS and driver update checker for your model, using Asus's official site
13. Asus and Logitech mice, keyboards and other peripherals
14. ROG Ally controller modes, NumberPad, FnLock and custom hotkeys
15. Audio tools: equaliser, noise suppression, reverb
16. Steam integration and game mode
17. Self-updater and autostart

### :gear: Automation
- Switches the performance mode when you go on battery or plug in.
- **Optimized** GPU mode turns the dGPU off on battery and back on when plugged in.
- **Auto screen refresh** runs at 60 Hz on battery and at maximum Hz with overdrive when plugged in.

### :rocket: Performance Modes
> [!NOTE]
> All modes are **built into the BIOS**, together with their default fan curves and power limits. They are the **same modes** as in Armoury Crate.

1. `Silent`: quiet, low power.
2. `Balanced`: the everyday default.
3. `Turbo`: maximum performance and fan speed.

Each mode can have its own fan curves, CPU power limits and GPU settings under **Fans + Power**.

### 💻 GPU Modes

1. **Eco**: only the integrated GPU is on, for the best battery life.
2. **Standard** (Hybrid): both GPUs are on. The integrated GPU drives the display, and apps can run on the dGPU.
3. **Optimized**: Eco on battery, Standard when plugged in.
4. **Ultimate**: the dGPU drives the built-in display directly. This needs a MUX switch, which 2022+ models have, and a reboot.

Switching to or from Ultimate always needs a reboot. Switching to Eco may need one if an app is still using the dGPU, and the app offers to close those apps for you.

## :video_game: GPU Tweaking

Each performance mode can have its own GPU settings:
- **Core clock offset** and **memory clock offset**. NVIDIA driver 555 or newer is needed.
- **Max clock cap**. Combined with a positive core offset, this is the G-Helper undervolt.
- **Dynamic Boost**: extra watts moved from the CPU to the GPU when the CPU doesn't need them.
- **Temperature target**: the temperature at which the GPU starts slowing down.

The GPU must not be in **Eco** mode for these controls to work.

### ⚠️ Advanced GPU power (opt-in, at your own risk)

Laptop GPUs are limited by their own firmware and by Asus's firmware. On the Flow X13 (2022), for example, the RTX 3050 Ti runs at **35 W plus 5 W of Dynamic Boost**. The planned **Advanced GPU power** panel sends the same higher values G-Helper for Windows can send.

> [!WARNING]
> This is **off by default and not advised**. More GPU power means more heat, louder fans and shorter battery life. It can make the system unstable or shorten hardware life. The firmware may ignore or clamp the values, so the app shows what was actually applied next to the GPU's real power draw. Each change is tested for 30 seconds and reverted unless you confirm. **Flashing a modified GPU VBIOS is not supported and never will be.**

Often, a better way to more FPS is efficiency rather than more watts. The GPU undervolt preset, and lowering CPU power limits so Dynamic Boost actually reaches the GPU, usually give more sustained performance on a power-limited laptop.

## :arrow_down: Installation

### Requirements
- An Asus ROG / TUF laptop. Lenovo IdeaPad, Legion and LOQ have experimental support, inherited from g-helper-linux.
- Any distro with **glibc 2.38+**, such as Ubuntu 24.04+, Debian 13+, Fedora 39+ or Arch.
- X11 or Wayland.
- Kernel **6.2+**, or **6.9+** for every feature.
- For NVIDIA GPU tuning: the proprietary NVIDIA driver, **555 or newer**.

### One-line install

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash
```

This installs the app to `/opt/ghelper`, together with the hardware access rules, the GPU helper, the boot-time GPU service and a menu entry. **Reboot once** afterwards. Start it from your app menu or with `ghelper`. On NixOS, the same command sets it up as a NixOS module.

### AppImage

```bash
curl -sL https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest/download/GHelper-x86_64.AppImage -o GHelper-x86_64.AppImage
chmod +x GHelper-x86_64.AppImage
./GHelper-x86_64.AppImage
```

On first start, the AppImage offers to install the system files it needs, with one password prompt.

### Uninstall

```bash
curl -sL https://raw.githubusercontent.com/VasuPadsumbia/G-Helper-linux-mod/master/install/install.sh | sudo bash -s -- --uninstall
```

Your settings in `~/.config/ghelper` are kept.

### Coming from g-helper-linux or asusctl?

Keep **only one** control app:
- **g-helper-linux** installs to the same places as this mod, so uninstall it first. Your settings carry over.
- **asusctl / asusd** and **supergfxctl** control the same hardware. Running them alongside this app makes the two overwrite each other's settings, so stop and disable or uninstall them.

### Configuration

Settings are stored in `~/.config/ghelper/config.json`. They use the same keys as G-Helper for Windows, so fan curves and mode settings are compatible.

## :wrench: Troubleshooting / FAQ

**"No NVIDIA GPU detected", but my games run on the NVIDIA GPU.**
The GPU was asleep when G-Helper started, which is a known upstream limitation. The mod's detection fix (🚧) solves this. Until it ships, opening **Fans + Power** while a game or other app is using the GPU makes it try again, and it usually finds the GPU.

**I don't see the GPU clock offset sliders.**
Check three things:
- your NVIDIA driver is version 555 or newer (`nvidia-smi`);
- the GPU mode is not Eco;
- the installer has been run, because it installs the GPU helper the sliders need. Re-running the one-line install repairs it.

**"System files need attention" popup.**
Click **Install / Update** and it fixes itself. Installs made with the mod's installer don't show this popup.

**My settings keep changing back.**
Another control app is probably running. Check with `systemctl status asusd supergfxd`, and see [Coming from g-helper-linux or asusctl](#coming-from-g-helper-linux-or-asusctl).

**Curve Optimizer (CPU undervolt) doesn't appear.**
It needs the [ryzen_smu](https://github.com/amkillam/ryzen_smu) kernel module, which is installed separately. With Secure Boot on, that module must also be signed. The feature stays hidden until the module is loaded. See [docs/amd-undervolting.md](docs/amd-undervolting.md).

**Refresh rate controls don't work on Wayland.**
They depend on your desktop. KDE, GNOME, wlroots-based compositors and X11 are supported through their own tools. X11 gives the most complete control.

**What do the AMD settings mean?**
See [docs/mod/amd-explained.md](docs/mod/amd-explained.md). It's a plain-language explanation of ryzenadj, Curve Optimizer and AMD GPU support.

## :keyboard: Keybindings

- `ROG / M5 key`: open or hide G-Helper
- `Fn + F5` / `M4`: cycle performance modes
- `Fn + F4`: cycle keyboard lighting (Aura) modes
- `Fn + F2 / F3`: keyboard backlight down / up
- `ghelper --osk`: toggle the on-screen keyboard. You can bind this to a shortcut in your desktop settings.

You can change these bindings in **Extra** settings. Overlay hotkeys will match G-Helper for Windows (`Ctrl + Shift + Alt + O`) once the overlay ships.

## :hammer_and_wrench: Building from source

```bash
# Ubuntu / Debian
sudo apt install dotnet-sdk-10.0 clang zlib1g-dev libpipewire-0.3-dev libwayland-dev pkg-config
# Fedora
sudo dnf install dotnet-sdk-10.0 clang zlib-devel pipewire-devel wayland-devel pkg-config
# Arch
sudo pacman -S dotnet-sdk clang libpipewire wayland pkg-config

./build.sh                         # Native AOT build -> dist/ghelper
./build.sh --fast                  # quick development build
sudo ./install/install-local.sh    # install your local build
```

The roadmap and feature designs are in [docs/mod/](docs/mod/). Pull requests and feature requests are welcome.

------------------

> [!NOTE]
> ### 🔖 Important Notice
> G-Helper Linux Mod is **not** a driver, firmware or operating system, and it doesn't "run" your hardware. It's an app that selects the operating modes Asus built into your BIOS, and optionally changes settings your laptop already has, through the same firmware interface Armoury Crate uses. The Linux kernel's `asus-wmi` and `asus-armoury` drivers expose that interface.
>
> If you use the same mode and settings as Armoury Crate, your laptop behaves the same. G-Helper is a remote control for your laptop, not a new engine.

### Libraries and projects used
- [g-helper-linux](https://github.com/utajum/g-helper-linux) by utajum: the project this mod is built on ([support the author](https://buymeacoffee.com/utajum)).
- [G-Helper](https://github.com/seerge/g-helper) by seerge: the original Windows app and the feature reference.
- [Linux kernel asus-wmi / asus-armoury](https://github.com/torvalds/linux/tree/master/drivers/platform/x86): the firmware interface.
- [Avalonia UI](https://avaloniaui.net/): the user interface.
- [NVML](https://developer.nvidia.com/management-library-nvml): NVIDIA GPU clocks and power.
- [RyzenAdj](https://github.com/FlyGoat/RyzenAdj) and [ryzen_smu](https://github.com/amkillam/ryzen_smu): AMD CPU power limits and undervolting.
- [MangoHud](https://github.com/flightlessmango/MangoHud): in-game FPS for the overlay (planned).
- [RNNoise](https://github.com/xiph/rnnoise): noise suppression.
- [wlr-randr](https://gitlab.freedesktop.org/emersion/wlr-randr): display control on wlroots compositors.
- [AsusCtl](https://gitlab.com/asus-linux/asusctl): for reference and inspiration.

The full list, with licences, is in [docs/mod/third-party.md](docs/mod/third-party.md).

### Privacy Policy
The app does not collect or send any personal information. It only connects to the internet to check GitHub for app updates and Asus's official website for BIOS and driver updates for your model.

### Disclaimers
"Asus", "ROG", "TUF" and "Armoury Crate" are trademarks of ASUSTeK Computer Inc. This project is not affiliated with or endorsed by Asus, and uses these names for identification only.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. OVERCLOCKING, UNDERVOLTING OR RAISING POWER LIMITS MAY CAUSE SYSTEM INSTABILITY OR HARDWARE DAMAGE. USE AT YOUR OWN RISK.

Licensed under GPL-3.0, the same as g-helper-linux and G-Helper.
