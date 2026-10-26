# UI: the Windows G-Helper look

## Goal

The app should look like G-Helper for Windows, while the code, layout files and window
structure stay those of g-helper-linux. g-helper-linux already copies the Windows layout:
the main window has Performance Mode, GPU Mode, Laptop Screen, Keyboard, AniMe Matrix and
Battery sections in the same order, and there is a separate Fans + Power window. What differs is
the styling. Upstream uses rounded Fluent controls, extra subtitle lines and a blue accent.
Windows G-Helper uses flat, square, dark tiles with a coloured border on the active mode.

So this is mainly a **theme job**, done in `src/UI/Styles/GHelperTheme.axaml`, plus small layout
tweaks. It doesn't need a rewrite.

## Palette (taken from G-Helper `app/UI/RForm.cs`)

These are the dark-theme values:
- Window background `#1C1C1C`. Main button `#2E2E2E`, secondary button `#242424`.
- Text `#F0F0F0`. Borders `#373737`, with a secondary border of `#2A2A2A`.
- Chart background `#232323`, chart grid `#464646`.

These colours mark the active mode, and are also used for the tray icon and overlay accents:
- Eco / Silent `#06B48A` (green).
- Standard / Balanced `#3AAEEF` (blue).
- Turbo `#FF2020` (red).
- Custom / Fans + Power `#FF8000` (orange).
- Inactive grey `#A8A8A8`.

The light theme follows the light values in the same file. Theme selection follows the system,
the same as Windows G-Helper.

## Rules

- Mode buttons are square tiles with an icon above the label. The active tile gets a 2 px
  border in its mode colour and a faint gradient of that colour. This matches `RButton.cs`.
- Section headers are bold, with an icon on the left and live status (temperature and fan) on
  the right, as in "Performance Mode … CPU: 48°C  Fan: 28%".
- The main window drops upstream's extra one-line descriptions, such as "dGPU direct — bypass
  iGPU…". Those move to tooltips.
- The Fans + Power window follows the Windows layout. Power limits (PPT) go in a left column
  with vertical sliders. CPU and GPU fan curve charts go on the right. Factory Defaults,
  Auto Apply and Apply Custom Curve buttons run along the bottom. The GPU section (offsets,
  Dynamic Boost, temperature target, advanced power) sits under the PPT column, as in Windows.
- Font: Segoe UI if installed, otherwise the system sans-serif. Upstream already does this.
- Upstream-only windows, such as Audio DSP, Steam, Lenovo and System Monitor, get the same
  palette but keep their layout.

## Merge strategy

Theme changes go in `GHelperTheme.axaml` and a new `src/UI/Styles/WindowsLook.axaml` included
from it. AXAML layout edits to `MainWindow.axaml` and `FansWindow.axaml` are unavoidable, so
we keep them small and note them in `CHANGELOG.md` so that upstream merges are easy to review.
