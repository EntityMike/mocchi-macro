# 02: DPI awareness from the manifest, no config file

Status: resolved

Parent: [spec](../spec.md)

## What to build

Make the build a single .exe that types correctly at any Windows display scaling.

- **Manifest:** add `<dpiAwareness>PerMonitorV2</dpiAwareness>` to `app.manifest`, in an `<application xmlns="urn:schemas-microsoft-com:asm.v3"><windowsSettings>` block with the `http://schemas.microsoft.com/SMI/2016/WindowsSettings` namespace on the element. Keep the Windows 10 `supportedOS` entry.
- **Rewrite the manifest's header comment.** The current one says DPI awareness lives in App.config and warns against putting it here. The new one says PerMonitorV2 is declared here so the .exe can ship alone, and that it's the manifest rather than a call in `Main` because the manifest always applies before any window exists.
- **Remove `App.config`** from the project and delete the file, so the build no longer writes `MocchiMacro.exe.config`.

After this ticket the tool window is scaled once at startup, as today, but no longer rescales when dragged to a monitor with different scaling. Ticket 03 adds that back.

## Acceptance criteria

- [x] A Release build's output folder has `MocchiMacro.exe` and no `MocchiMacro.exe.config`.
- [x] In the build output folder, the new .exe shows the typing rate and game saved by the previous build (with its config file) from that same folder. Saved settings are stored per .exe path, so this check must use the same folder.
- [x] Copied alone into an empty folder, the .exe starts, with the default typing rate and game.
- [x] With Windows scaling at 125% or 150% on the game's monitor, the lone .exe types correctly into MR2 DX windowed at 1600×900, into MR2 DX fullscreen, and into MR1 DX windowed.
- [x] At that scaling the tool's window text is sharp, not blurry.
- [x] Noted in a comment on this ticket: whether the occasional missed click at 125% still happens.
- [x] The automated tests pass unchanged.

## User stories covered

1, 2, 5

## Comments

Checked while implementing, at 100% scaling:

- The Release build writes `MocchiMacro.exe` and `MocchiMacro.pdb`, no `.exe.config`.
- The previous release build (76c4c6d, with its config file) and the new lone .exe, run in turn from one folder, used the same `user.config`. A game saved there as MR1 DX showed up in the new .exe. This ran in a test folder rather than `bin\Release`, whose saved settings held only defaults.
- Copied alone into an empty folder, the .exe starts with 10 keys per second and MR2 DX, and Windows reports the process as PerMonitorV2.
- All 81 automated tests pass, unchanged.

Checked by hand afterwards, at higher scaling:

- The lone .exe types correctly into MR2 DX windowed at 1600×900, into MR2 DX fullscreen, and into MR1 DX windowed.
- The tool's window text is sharp.
- The occasional missed click at 125% didn't happen.
