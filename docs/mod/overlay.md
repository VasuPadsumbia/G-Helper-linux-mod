# Hardware overlay and tray

## What Windows G-Helper's overlay offers (target)

The Windows overlay comes from `app/Overlay/HardwareOverlay.cs` and `app/OverlayConfig.cs`.
- Four presets: **Light, Default, Full, Complete**.
- Items you can switch on or off: **FPS, Temperatures, Fan, Chart, Power, Load, RAM,
  Battery, Labels**.
- **Size** (scale percent), **Transparency**, **CPU colour**, **GPU colour**, **Reset**.
- **Position**: an anchor corner, which screen, and X/Y offsets.
- **"Overlay only in games"**.
- FPS comes from Windows ETW (`EtwFpsMonitor.cs`).

We keep the same settings keys (`overlay_mode`, `overlay_alpha`, `overlay_anchor`,
`overlay_scale_percent`, `overlay_screen`, `overlay_offset_x/y`, …), so the meaning is
identical. The settings window copies OverlayConfig's layout.

## How it works on Linux

Linux has no ETW. On Wayland, apps also can't simply place an always-on-top window
wherever they like. So the overlay has two backends, and the app picks one automatically.

**1. Desktop overlay (native, our own window).** It shows every item except FPS.
- On X11 it is an always-on-top, borderless, transparent Avalonia window. It's made
  click-through with the XShape input region, through P/Invoke.
- On Wayland with KDE, wlroots compositors, Hyprland or COSMIC, it is a small layer-shell
  surface on the "overlay" layer with no input region. It's drawn by a tiny native helper
  under `overlay-helper/`, in C, in the same style as `audio-helper/`, because Avalonia can't
  create layer-shell surfaces yet. The app streams the stats to it over a pipe.
- On GNOME (Wayland), where there is no layer-shell, it falls back to an XWayland window marked
  "above". It may disappear over exclusive-fullscreen games.

**2. In-game overlay (MangoHud).** This adds FPS and frame time.
- FPS can only be measured inside the game's graphics API, which is what MangoHud does as a
  Vulkan/OpenGL layer. We don't reimplement it.
- G-Helper writes a MangoHud config that mirrors the user's chosen preset, items, colours,
  position and scale. MangoHud then looks and behaves like the G-Helper overlay inside games.
  It works on X11, Wayland and gamescope.
- If MangoHud isn't installed, the FPS checkbox is greyed out with a hint. It's an optional
  runtime dependency, linked in third-party.md.
- Steam launch options (`mangohud %command%`) can be added through upstream's existing Steam
  integration.

**"Only in games"** is decided from these signals: gamemode active (the GameMode D-Bus),
a gamescope session, a process on the dGPU (upstream's `NvidiaProcessScanner`), or a
Steam-launched process.

## Data sources

Every source below already exists upstream (`Helpers/SystemTelemetry.cs`, `TempHelper.cs`,
GPU controls):
- CPU temperature, load, clock and package power, from hwmon `k10temp`/`coretemp`, `/proc/stat`
  and RAPL / `amd_energy` / ryzenadj.
- GPU temperature, load, clock, power and VRAM, from NVML (gpu-helper) or amdgpu hwmon.
- Fan RPM from asus-wmi hwmon. RAM from `/proc/meminfo`.
- Battery charge and charge/discharge rate from `power_supply`.

## Tray

Upstream's tray shows the performance mode icon, optional CPU and GPU temperature icons, and a
tooltip. We extend it as follows:
- The tooltip shows the full Windows-style summary: CPU temperature, load and power; GPU
  temperature, load and power; fan RPM; battery charge and rate.
- New optional tray icons show the same small text, for **CPU W, GPU W and battery W**,
  using the same SkiaSharp renderer as the temperature icons.
- Each tray item is a checkbox in Extra settings, like upstream's CPU and GPU icon toggles.
- The menu gains "Show overlay" and an overlay preset switch.

## Hotkey

A configurable key toggles the overlay and cycles through the presets. It uses upstream's key
binding system (`Input/InputDispatcher.cs`) and defaults to the same combination as Windows
G-Helper.
