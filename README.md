<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&height=180&color=0:1C1C1C,60:2E2E2E,100:FF2020&text=G-Helper%20Linux%20Mod&fontColor=F0F0F0&fontSize=52&fontAlignY=38&desc=Lightweight%20Armoury%20Crate%20alternative%20for%20Asus%20laptops%20on%20Linux&descSize=16&descAlignY=60&animation=fadeIn" width="100%" alt="G-Helper Linux Mod" />

<img src="install/ghelper.png" width="96" alt="G-Helper icon" />

### Everything [g-helper-linux](https://github.com/utajum/g-helper-linux) does, plus the [G-Helper for Windows](https://github.com/seerge/g-helper) features Linux was missing

[![GitHub release](https://img.shields.io/github/v/release/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&logo=github&color=3AAEEF&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/VasuPadsumbia/G-Helper-linux-mod/total?style=for-the-badge&logo=linux&logoColor=white&color=06B48A&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases)
[![License](https://img.shields.io/github/license/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&color=A8A8A8&cacheSeconds=3600)](LICENSE)
[![Stars](https://img.shields.io/github/stars/VasuPadsumbia/G-Helper-linux-mod?style=for-the-badge&logo=github&color=FF8000&cacheSeconds=3600)](https://github.com/VasuPadsumbia/G-Helper-linux-mod/stargazers)

[![Based on g-helper-linux](https://img.shields.io/badge/based%20on-g--helper--linux-2E2E2E?style=flat-square&logo=github)](https://github.com/utajum/g-helper-linux)
[![Feature reference](https://img.shields.io/badge/feature%20reference-G--Helper%20for%20Windows-FF2020?style=flat-square&logo=windows)](https://github.com/seerge/g-helper)
[![Made for ROG](https://img.shields.io/badge/tested%20on-ROG%20Flow%20X13%202022-FF2020?style=flat-square&logo=asus)](#)
[![No Docker](https://img.shields.io/badge/no-Docker%20%C2%B7%20daemons%20%C2%B7%20custom%20kernel-06B48A?style=flat-square)](#gift-advantages)

<a href="https://github.com/VasuPadsumbia/G-Helper-linux-mod/releases/latest"><img src="https://img.shields.io/badge/%E2%AC%87%EF%B8%8F%20Download-latest%20release-3AAEEF?style=for-the-badge" height="42" alt="Download latest release" /></a>
&nbsp;
<a href="#arrow_down-installation"><img src="https://img.shields.io/badge/%F0%9F%93%A6%20Install-one%20line-06B48A?style=for-the-badge" height="42" alt="One-line install" /></a>
&nbsp;
<a href="docs/mod/roadmap.md"><img src="https://img.shields.io/badge/%F0%9F%97%BA%EF%B8%8F%20Roadmap-what%E2%80%99s%20next-FF8000?style=for-the-badge" height="42" alt="Roadmap" /></a>

**[What's new](#sparkles-whats-new-in-the-mod)** ·
**[Features](#zap-features)** ·
**[GPU Tweaking](#video_game-gpu-tweaking)** ·
**[Install](#arrow_down-installation)** ·
**[FAQ](#wrench-troubleshooting--faq)** ·
**[Keybindings](#keyboard-keybindings)**

</div>

---

A small, fast **Armoury Crate alternative** for Asus ROG and TUF laptops on Linux. It is a modified version of [g-helper-linux](https://github.com/utajum/g-helper-linux) that keeps everything g-helper-linux does, and works towards **full feature parity with [G-Helper for Windows](https://github.com/seerge/g-helper)**. That includes its clean look, GPU overclocking that detects your GPU reliably, safe undervolting with automatic revert, an in-game metrics overlay, a richer tray and advanced GPU power controls.

It's developed and tested on the **ROG Flow X13 (2022)**, with a Ryzen 9 6900HS and an RTX 3050 Ti. It supports the same models as g-helper-linux: Zephyrus G14 / G15 / G16 / M16, Flow X13 / X16 / Z13, Strix and Scar, TUF, ProArt, ROG Ally and more.

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

| | Feature | Status |
|:-:|---|:-:|
| 🧰 | First-run fix: no "System files need attention" popup after install | ✅ Done |
| 🔋 | CPU power limits below the firmware minimum on AMD (down to 6 W) | ✅ Done |
| 🏷️ | Custom power shown in the mode title ("Mode: Turbo 15W"), fan speed in RPM | ✅ Done |
| 🖥️ | Refresh rate switching on GNOME 40–47 Wayland, and Auto that stays on | ✅ Done |
| 📈 | CPU power in the Monitor window | ✅ Done |
| 🔍 | GPU detection that works while the GPU sleeps, plus automatic rescan | 🚧 In progress |
| 🎨 | The Windows G-Helper look | 🚧 In progress |
| 🛡️ | 30-second test and automatic revert for GPU tuning | 🗓️ Planned |
| 📉 | One-click GPU undervolt | 🗓️ Planned |
| ⚡ | Advanced GPU power (opt-in, at your own risk) | 🗓️ Planned |
| 📊 | Metrics overlay (in-game FPS still to come) | 🚧 In progress |
| 🔋 | Richer tray: CPU / GPU / battery watts | 🗓️ Planned |
| 🌡️ | CPU temperature limit and safe undervolting | 🗓️ Planned |
| 🧩 | Custom performance modes: add, rename, remove | 🗓️ Planned |
| 🔌 | XG Mobile panel and per-app dGPU list | 🗓️ Planned |

### 🆚 How it compares

| | G-Helper for Windows | g-helper-linux | **G-Helper Linux Mod** |
|---|:-:|:-:|:-:|
| Performance and GPU modes, fan curves, power limits | ✅ | ✅ | ✅ |
| NVIDIA clock offsets | ✅ | ✅ (can miss a sleeping GPU) | ✅ (detects it reliably) 🚧 |
| Safe test-and-revert for overclocks and undervolts | ❌ | ❌ | 🗓️ |
| Custom performance modes (add / rename / remove) | ✅ | ❌ | 🗓️ |
| Metrics overlay | ✅ | ❌ | ✅ (FPS 🗓️) |
| Advanced GPU power ranges | ✅ | ❌ | 🗓️ |
| Windows G-Helper look | ✅ | ❌ | 🚧 |
| Runs on Linux | ❌ | ✅ | ✅ |


### ✅ First-run fix
Upstream's one-line installer left two system files in a form the app didn't recognise. Right after a fresh install, it showed a *"System files need attention"* popup about the hardware access rules and kernel modules. The mod's installer writes both exactly as the app expects. It also makes the `uinput` and `i2c-dev` modules load at every boot, not just during install.

### 🚧 GPU detection that doesn't give up
Linux puts the NVIDIA GPU to sleep when nothing uses it. Upstream only checks for the GPU once, at startup, without waking it. If the GPU was asleep at that moment, the app shows *"No NVIDIA GPU detected"* for the rest of the session, even while games are running on it. The mod:
- ✅ reads the GPU model from the driver without waking the GPU, so detection works while it's asleep;
- 🗓️ **re-checks automatically** after a driver install or update, after resume from sleep, and after a GPU mode change. A **Rescan** button is also planned;
- never hides GPU controls without a reason. It shows why they're unavailable (driver too old, Eco mode, helper missing) and how to fix it.

### 🗓️ GPU tuning safety
New clock offsets, power settings and undervolts run for a **30-second test**. If you don't confirm, or the app or system crashes, they revert to stock automatically.

### 🗓️ One-click GPU undervolt
This is G-Helper's undervolt recipe: a max clock cap plus a positive core offset. You get the same or better clocks at lower voltage, which on a power-limited laptop GPU means higher sustained performance.

### 🗓️ Advanced GPU power
An opt-in panel that uses G-Helper for Windows' ranges: Dynamic Boost up to 25 W, extra TGP up to 75 W and a temperature target from 75 to 87 °C. See [GPU Tweaking](#video_game-gpu-tweaking) for the details and risks.

### 🚧 Metrics overlay
The Windows G-Helper overlay brought to Linux. Open it with the **Overlay** button in the Laptop Keyboard header (right-click turns it on or off) or from the tray menu:
- ✅ Light, Default, Full and Complete presets, with the same per-preset defaults as Windows;
- ✅ temperatures, fan speed, power chart, power, load, RAM, battery and labels;
- ✅ size, transparency, CPU and GPU colours, screen corner and "only in games";
- ✅ always on top and click-through, so it never gets in the way of your game;
- 🗓️ in-game FPS, through [MangoHud](https://github.com/flightlessmango/MangoHud).

For Windows' `Ctrl + Shift + Alt + O`, add a keyboard shortcut in your desktop settings that runs `ghelper --overlay`.

### 🗓️ Richer tray
Optional tray icons for **CPU W, GPU W and battery W**, next to the existing temperature icons. A full summary tooltip shows temperatures, load, power, fan speed and battery rate.

### 🗓️ CPU temperature limit and safe undervolting
Adds a CPU temperature limit, like the Windows app. The same 30-second test is added to AMD Curve Optimizer and iGPU undervolting.

### 🚧 The Windows look
The flat dark G-Helper interface:
- square mode tiles with a coloured border on the active mode (green Silent, blue Balanced, red Turbo, orange Custom);
- the same Fans + Power layout as Windows.

### 🗓️ Custom performance modes
As in G-Helper for Windows, you'll be able to **add**, **rename** and **remove** your own modes in Fans + Power. Each new mode starts from Silent, Balanced or Turbo and gets its own fan curves, power limits and GPU settings.

### 🗓️ XG Mobile panel and per-app dGPU list
- XG Mobile dock status (attached / locked) with eGPU switching and a logout reminder.
- A list of apps that should always run on the NVIDIA GPU.

## :zap: Features

Everything below works today unless it's marked 🗓️ (planned). 🆕 marks what this mod adds on top of g-helper-linux. Click a group to expand it.

<details open>
<summary><b>🚀 Performance modes and power</b></summary>

- Silent, Balanced and Turbo, the modes built into your BIOS, plus custom modes
- Automatic mode switching when you plug in or go on battery
- CPU power limits: sustained (SPL / PL1), slow boost (SPPT / PL2) and fast boost (FPPT)
- 🆕 CPU power limits below the firmware minimum on AMD, down to 6 W, through ryzenadj
- 🆕 The custom power limit shown in the mode title, for example "Mode: Turbo 15W"
- CPU boost on / off and energy performance preference (EPP)
- Linux platform profile kept in sync with the mode
- Per-mode "auto apply" for power limits, fan curves and GPU settings
- Per-mode shell command hook, run on every mode switch
- 🗓️ Custom modes with Add, Rename and Remove buttons, as on Windows

</details>

<details>
<summary><b>🌀 Fans</b></summary>

- Custom fan curve editor: 8 points per fan, per mode, with drag-to-edit
- CPU, GPU and mid fan support where the laptop has them
- Experimental manual fan control (EC table) on supported models
- Fan speed hysteresis to avoid fans hunting up and down
- 🆕 Fan speed in RPM by default; click the reading to switch to percent

</details>

<details>
<summary><b>🎮 GPU</b></summary>

- GPU modes: Eco, Standard, Optimized (Eco on battery, Standard on AC) and Ultimate (MUX)
- Safe Eco switching: closes apps using the dGPU, or schedules the change for the next boot
- NVIDIA tuning: core and memory clock offsets, core clock lock, VRAM clock lock and power limit
- NVIDIA Dynamic Boost, temperature target, sustained TGP and max TGP (from the Asus firmware)
- Live GPU status: clocks, P-state, power, temperature, usage, VRAM, throttle reasons and PCIe link
- A list of the apps using the dGPU, which you can close
- AMD Radeon dGPU monitoring
- 🆕 NVIDIA GPU detected even while it sleeps (upstream missed it and showed "No NVIDIA GPU detected")
- 🗓️ Automatic driver rescan and a Rescan button
- 🗓️ 30-second test and automatic revert for overclocks
- 🗓️ One-click GPU undervolt (clock cap plus core offset)
- 🗓️ Advanced GPU power with G-Helper for Windows' ranges (opt-in, at your own risk)

</details>

<details>
<summary><b>🧠 CPU tuning and undervolting</b></summary>

- AMD Curve Optimizer undervolting for the CPU and iGPU, through ryzen_smu (optional module)
- Intel undervolting through MSR
- AMD temperature, current and power tuning through the bundled ryzenadj
- CPU temperature target
- 🗓️ 30-second test and automatic revert for undervolts

</details>

<details>
<summary><b>🔋 Battery and power management</b></summary>

- Battery charge limit (40–100 %) with one-click 60 / 80 / 100 % presets
- Battery information: health, cycle count, design capacity, charge and discharge rate
- Suspend mode choice (s2idle or deep sleep)
- Clamshell mode (don't sleep when the lid is closed)
- dGPU power daemon and USB power options on supported models
- Automatic PCIe power saving (ASPM) in Silent mode

</details>

<details>
<summary><b>🖥️ Display</b></summary>

- Refresh rate: Auto (60 Hz on battery, maximum on AC), 60 Hz or maximum
- 🆕 Refresh rate switching on GNOME 40–47 Wayland, through GNOME's own display API
- 🆕 Auto always turns on, like Windows, and hovering over the buttons explains them
- Panel overdrive
- Mini-LED modes on supported panels
- Optimal display brightness (Asus firmware)
- Display brightness and gamma
- Works with X11, KDE, GNOME and wlroots compositors

</details>

<details>
<summary><b>⌨️ Keyboard and lighting</b></summary>

- Aura RGB modes, colours, speed and brightness, including per-key RGB keyboards
- Lighting power zones for the keyboard, logo, lightbar, lid and rear light: awake, boot, sleep, shutdown and battery
- Keyboard backlight brightness, with separate levels on AC and battery
- Keep the keyboard backlight always on
- Status LED control

</details>

<details>
<summary><b>✨ AniMe Matrix and Slash</b></summary>

- AniMe Matrix: built-in animations, pictures, animated GIFs and clock
- 🗓️ Audio visualiser on the AniMe Matrix and Slash
- Slash lighting: animations, speed, brightness, and boot / sleep / shutdown / battery / lid behaviour

</details>

<details>
<summary><b>📊 Monitoring, overlay and tray</b></summary>

- Live CPU and GPU temperature, load and fan speed in the main window
- System monitor window with temperature, fan and power charts
- 🆕 CPU package power in the monitor (AMD APUs, no root needed)
- 🆕 Metrics overlay with Light, Default, Full and Complete presets: temperatures, fan speed, power chart, power, load, RAM and battery. Size, transparency, colours, screen corner and "only in games" are adjustable, and it's always on top and click-through
- 🗓️ In-game FPS in the overlay, through MangoHud
- Tray icon with the current mode, optional CPU and GPU temperature icons, and a dGPU status icon
- 🗓️ Tray icons for CPU, GPU and battery watts
- System information window: CPU, memory, graphics, storage, network, audio, battery and USB devices

</details>

<details>
<summary><b>🎹 Input and hotkeys</b></summary>

- ROG / M5 key, Fn+F4 and Fn+F5 / M4 bindings, each assignable to an action
- Function key remap (FN-Lock): F1–F12 as media keys, with a per-key map and a toggle hotkey
- NumberPad: the touchpad's built-in number pad
- On-screen keyboard (`ghelper --osk`)
- Camera, touchpad and touchscreen toggles
- 🆕 `ghelper --overlay` to toggle the overlay from a desktop shortcut

</details>

<details>
<summary><b>🎧 Audio</b></summary>

- Microphone mute toggle
- Audio effects through PipeWire: equaliser, RNNoise noise suppression, reverb, delay and vocoder
- Boot sound on / off

</details>

<details>
<summary><b>🔌 XG Mobile and handhelds</b></summary>

- XG Mobile dock: eGPU switching, dock lights and dock fan curve
- ROG Ally: controller modes, button mapping, stick calibration, APU memory and handheld settings
- Steam integration: add G-Helper to Steam, gamescope game mode
- 🗓️ XG Mobile panel with attach and lock status

</details>

<details>
<summary><b>🖱️ Peripherals</b></summary>

- Asus mice: DPI, polling rate, lighting and buttons on many ROG and TUF models
- Logitech mice, keyboards and headsets through HID++

</details>

<details>
<summary><b>⚙️ System, updates and integration</b></summary>

- BIOS and driver update checker for your exact model, using Asus's official site
- Self-updater with changelog
- One-click repair of the app's system files
- Start on login, start minimised, window always on top, temperatures in Fahrenheit, 22 icon sets and 38 languages
- Raw WMI mode for 2020–2021 laptops (experimental)
- NixOS module, and immutable / SteamOS support
- Experimental Lenovo support: IdeaPad, Legion, LOQ and Yoga
- 🆕 The Windows G-Helper look: mode colours, flat dark sections and white icons
- 🆕 Installer that doesn't trigger the "System files need attention" popup

</details>

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

<details>
<summary><b>"No NVIDIA GPU detected", but my games run on the NVIDIA GPU.</b></summary>

The GPU was asleep when G-Helper started, which is a known upstream limitation. The mod's detection fix (🚧) solves this. Until it ships, opening **Fans + Power** while a game or other app is using the GPU makes it try again, and it usually finds the GPU.

</details>

<details>
<summary><b>I don't see the GPU clock offset sliders.</b></summary>

Check three things:
- your NVIDIA driver is version 555 or newer (`nvidia-smi`);
- the GPU mode is not Eco;
- the installer has been run, because it installs the GPU helper the sliders need. Re-running the one-line install repairs it.

</details>

<details>
<summary><b>"System files need attention" popup.</b></summary>

Click **Install / Update** and it fixes itself. Installs made with the mod's installer don't show this popup.

</details>

<details>
<summary><b>My settings keep changing back.</b></summary>

Another control app is probably running. Check with `systemctl status asusd supergfxd`, and see [Coming from g-helper-linux or asusctl](#coming-from-g-helper-linux-or-asusctl).

</details>

<details>
<summary><b>Curve Optimizer (CPU undervolt) doesn't appear.</b></summary>

It needs the [ryzen_smu](https://github.com/amkillam/ryzen_smu) kernel module, which is installed separately. With Secure Boot on, that module must also be signed. The feature stays hidden until the module is loaded. See [docs/amd-undervolting.md](docs/amd-undervolting.md).

</details>

<details>
<summary><b>Refresh rate controls don't work on Wayland.</b></summary>

They depend on your desktop. KDE, GNOME, wlroots-based compositors and X11 are supported through their own tools. X11 gives the most complete control.

</details>

<details>
<summary><b>What do the AMD settings mean?</b></summary>

See [docs/mod/amd-explained.md](docs/mod/amd-explained.md). It's a plain-language explanation of ryzenadj, Curve Optimizer and AMD GPU support.

</details>

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

<div align="center">

---

Made on a ROG Flow X13 (2022) · Built on [g-helper-linux](https://github.com/utajum/g-helper-linux) by utajum · Inspired by [G-Helper](https://github.com/seerge/g-helper) by seerge

**[⬆ Back to top](#)**

</div>
