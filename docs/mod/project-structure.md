# Project structure

The layout is upstream g-helper-linux's layout. This page only lists what the mod adds or
changes. For the full tree, see the PROJECT STRUCTURE section of the README.

## New (mod)

```
src/Gpu/GpuDriverWatcher.cs        re-probe GPU on driver / module / resume / mode change
src/Gpu/Tuning/                    test-and-revert session, crash flag, power unlock guard
src/Overlay/                       overlay model, presets, data sampling, MangoHud config writer
src/UI/Views/OverlayWindow.axaml   overlay (X11 / GNOME fallback)
src/UI/Views/OverlayConfigWindow.axaml   settings, mirrors Windows OverlayConfig
src/UI/Views/XgMobileWindow.axaml  XG Mobile panel
src/UI/Views/OffloadWindow.axaml   per-app dGPU offload list
src/UI/Styles/WindowsLook.axaml    Windows G-Helper palette + tile styles
overlay-helper/                    C layer-shell overlay surface for Wayland (like audio-helper/)
docs/mod/                          these design documents
```

## Changed upstream files (keep edits small)

```
README.md                          MOD EDITION section, install URLs, our banner/badges;
                                   upstream screenshot, star history, website and donation badges removed
screenshot.png, assets/star-history/, .github/workflows/star-history.yml   removed (upstream's data)
.github/workflows/release.yml      repo URLs; paths-ignore so docs/images/CI-only pushes don't release
.gitignore                         CLAUDE.md, .claude/
install/install.sh                 REPO=VasuPadsumbia/G-Helper-linux-mod
install/ghelper.appdata.xml        bugtracker URL
src/UI/Views/UpdatesWindow.axaml.cs, ChangelogWindow.axaml.cs   repo URLs
.github/ISSUE_TEMPLATE/issue.md, nixos/flake.nix   repo URLs
src/UI/Styles/GHelperTheme.axaml   include WindowsLook (planned)
src/UI/Views/MainWindow.axaml, FansWindow.axaml(.cs)   layout + GPU rows (planned)
src/Helpers/TraySystemMonitor.cs   extra tray items (planned)
src/App.axaml.cs                   start watcher + overlay (planned)
build.sh, build-appimage.sh        build overlay-helper (planned)
```

## Rules

- New features go in new files and folders, and are hooked in with a few lines.
- Vendored third-party code goes under `vendor/<name>/` with its `LICENSE`, a `VERSION` file
  (giving the source URL and commit) and `update.sh`, following upstream's pattern.
- Strings go through `src/I18n/Labels.cs`, English first.
