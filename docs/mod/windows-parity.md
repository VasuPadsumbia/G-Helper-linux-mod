# Windows parity checklist

This list compares every setting key used by G-Helper for Windows (`app/Settings.cs`, `Extra.cs`,
`Fans.cs`, `OverlayConfig.cs`) with the g-helper-linux code. It covers only what is **missing on
Linux**. Everything else already exists.

## Done in the mod
- [x] CPU power limits below the asus-armoury minimum on AMD, through ryzenadj (floor 6 W).
- [x] The custom SPL shown in the mode title ("Mode: Turbo 15W").
- [x] Fan speed in RPM by default; clicking the readout toggles between RPM and percent.
- [x] Refresh rate on GNOME 40-47 Wayland, through the Mutter DisplayConfig D-Bus API.
- [x] Windows look, step 1: mode colours, palette, flat sections, mono icons, hover-only GPU hint.

## Overlay (`OverlayConfig.cs`, `Overlay/HardwareOverlay.cs`)
Settings keys, kept identical to Windows:
- `overlay` (on/off), `overlay_game_only`, `overlay_mode` (0 Default, 1 Light, 2 Full, 3 Complete),
  `overlay_scale_percent` (100), `overlay_alpha` (128), `overlay_color_cpu`, `overlay_color_gpu`.
- Per-mode blocks, stored as `ModeKey(key, mode)`: `overlay_show_fps`, `overlay_show_temp`,
  `overlay_show_fans`, `overlay_show_chart`, `overlay_show_power`, `overlay_show_usage`,
  `overlay_show_ram`, `overlay_names` (labels), and `overlay_show_battery` (on / off / auto).
- Hotkeys: Ctrl+Shift+Alt+O toggles the overlay. With Ctrl+Shift+Alt held, drag moves it, a click
  switches mode, the wheel resizes it, and a wheel click resets the size.

Linux design: see [overlay.md](overlay.md). FPS comes from MangoHud.

## Fans + Power (`Fans.cs`)
- [ ] Custom modes: Add, Rename and Remove, each based on Silent, Balanced or Turbo (`mode_name`, `Modes.Add`).
- [ ] `limit_cputemp`: CPU temperature limit.
- [ ] `limit_gpucpu`: GPU-to-CPU Dynamic Boost (`PPT_GPUCPU9C`).
- [ ] `limit_crossload`: cross-loading processor power (`PPT_CROSS9F`).
- [ ] Windows layout: PPT column with vertical sliders, CPU and GPU fan charts side by side.

## Extra settings (`Extra.cs`)
- [ ] `keyboard_timeout` / `keyboard_ac_timeout`: keyboard backlight timeout on battery and on AC.
- [ ] `kill_gpu_apps`: close dGPU apps automatically when switching to Eco.
- [ ] `optimized_usbc`: Optimized mode keeps the dGPU on when a USB-C display is connected.
- [ ] `elmb`: ELMB (motion blur reduction) on supported panels.
- [ ] `vram_mem`: iGPU VRAM (UMA) size, if asus-armoury exposes it.
- [ ] `nv_platform`: NVIDIA platform controller (Windows-specific; check for a Linux equivalent).
- [ ] `standby_networking`: Windows Modern Standby only, so not applicable.

## Main window (`Settings.cs`)
- [ ] Visual modes: `visual`, `gamut`, `color_temp`, `hide_visual`. These are Asus Splendid colour
      profiles. On Linux this needs ICC profiles or a colour transform; it needs research.
- [ ] `sensors_always`, `sensor_timer`: keep sensors updating when the window is hidden, and the polling interval.
