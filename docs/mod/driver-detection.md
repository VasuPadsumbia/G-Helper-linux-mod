# GPU driver detection and re-scan

## The problem

On your machine, the GPU overclock rows never appeared even though the NVIDIA driver was
installed. Upstream's code explains why that can happen.

- At startup the app creates an NVIDIA control once. If NVIDIA isn't found, it falls back to
  AMD or to no dGPU. `RefreshGpuControlIfMissing()` only retries when **no** control exists.
  It never retries when the control exists but cannot do anything.
- The overclock rows depend on one probe, `gpu-helper nvml-info`. That probe runs **without** a
  password prompt (`allowPkexec: false`), so it only works if the sudoers drop-in for gpu-helper
  is installed and current. Otherwise it returns nothing.
- When the probe returns nothing, the rows are **hidden** without any message. They are only
  rebuilt when the Fans window loads.
- When the dGPU is asleep or in Eco mode, the probe is skipped on purpose and the old result is
  used. At startup there is no old result, so the rows stay hidden.

So driver installs, driver updates, a gpu-helper repair, waking the GPU, or switching from Eco
all happen without the UI ever finding out.

## Design: `Gpu/GpuDriverWatcher` (new)

The watcher is a small background service that tells the GPU layer to probe again whenever the
driver situation might have changed.

These events trigger a probe:
- udev events for the `module` subsystem (nvidia / amdgpu loaded or unloaded) and for `pci`
  bind/unbind on the dGPU. This uses the netlink monitor that upstream already uses for hot-plug.
- inotify on `/dev` for `nvidia*` device nodes appearing.
- inotify on the directories holding `libnvidia-ml.so.1`, which catches driver package updates.
- Resume from suspend, via logind `PrepareForSleep`.
- A GPU mode change (Eco → Standard / Optimized / Ultimate) once it completes.
- The installer's repair step finishing, which fixes a missing gpu-helper or sudoers file.
- Opening the Fans + Power window.
- A **Rescan** button.

On a trigger, the watcher waits two seconds, because drivers settle in bursts. Then it:
1. Rebuilds the GPU control even if one exists, because the vendor or driver may have changed.
2. Clears the NVML cache (`InvalidateNvmlCache`) and runs the probe again.
3. Raises `GpuCapabilitiesChanged`. Open windows reload their GPU rows, and the tray and overlay
   update.

## Design: never hide, always explain

The GPU tuning rows are always visible on machines with a dGPU. When they can't be used, they
are disabled and a status line says why, along with the fix:
- "No NVIDIA driver loaded". The fix is to install the driver and then click Rescan.
- "Driver 5xx is too old for clock offsets (needs 555+)."
- "dGPU is off (Eco mode)". It offers to switch to Standard.
- "dGPU is asleep". It offers to wake it and probe.
- "gpu-helper missing or not permitted". It offers the "Repair system files" action from
  upstream's installer.
- "NVML error N", with the raw error, for anything else.

The full probe result also goes into upstream's Diagnostics export, so bug reports contain it.

## AMD

The same watcher handles amdgpu. Its overdrive (`pp_od_clk_voltage`) only exists when
`amdgpu.ppfeaturemask` has the overdrive bit set. If that bit is missing, the status line
explains how to set it.
