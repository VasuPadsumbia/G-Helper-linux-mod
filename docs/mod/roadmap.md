# Roadmap

## Phase 0: set up the fork
- [x] Import utajum/g-helper-linux with its full history, using `upstream` as the remote
  and `master` as the branch.
- [x] Point the installer, updater, changelog, appdata and release badges at
  VasuPadsumbia/G-Helper-linux-mod.
- [ ] Push to GitHub and run the release workflow once, because the one-liner installer
  downloads the release binary.
- [ ] Build and install on the Flow X13. Confirm that the upstream features work.

## Phase 1: probe and driver re-scan
- [ ] Add a read-only `ghelper --probe` that dumps ASUS attributes with their ranges, NVML
  capabilities, power limits, the driver version, DRM card owners and XG Mobile state.
- [ ] Add `GpuDriverWatcher`, a Rescan button, and an "unavailable because…" status line.

## Phase 2: GPU tuning and power
- [ ] Add the `Gpu/Tuning` test-and-revert session and the crash flag.
- [ ] Add the GPU undervolt preset.
- [ ] Add the advanced GPU power unlock, with its disclaimer, read-back and temperature guard.
- [ ] Add AMD dGPU overdrive.

## Phase 3: Windows look
- [ ] Apply the palette and mode-tile style in `GHelperTheme.axaml` and `WindowsLook.axaml`.
- [ ] Rework the main window and the Fans + Power layout to match Windows.

## Phase 4: overlay and tray
- [ ] Add the overlay settings window, which is a copy of OverlayConfig.
- [ ] Add the X11 overlay window, then the layer-shell `overlay-helper`, then the GNOME
  fallback.
- [ ] Generate the MangoHud config for in-game FPS.
- [ ] Add the richer tray tooltip and the power tray icons.

## Phase 5: undervolting and remaining parity
- [ ] Add the CPU temperature limit, test-and-revert for CPU and iGPU undervolt, and a ryzen_smu
  setup guide.
- [ ] Add the XG Mobile panel and the per-app offload list.
- [ ] Have the installer detect asusd and supergfxd.
- [ ] Work through the Windows parity checklist: visual modes, ELMB, clamshell mode,
  touchscreen, peripherals, FnLock and hotkeys.
