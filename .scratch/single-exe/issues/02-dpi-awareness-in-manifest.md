# 02: DPI awareness from the manifest, no config file

Status: ready-for-agent

Parent: [spec](../spec.md)

## What to build

Make the build a single .exe that types correctly at any Windows display scaling.

- **Manifest:** add `<dpiAwareness>PerMonitorV2</dpiAwareness>` to `app.manifest`, in an `<application xmlns="urn:schemas-microsoft-com:asm.v3"><windowsSettings>` block with the `http://schemas.microsoft.com/SMI/2016/WindowsSettings` namespace on the element. Keep the Windows 10 `supportedOS` entry.
- **Rewrite the manifest's header comment.** The current one says DPI awareness lives in App.config and warns against putting it here. The new one says PerMonitorV2 is declared here so the .exe can ship alone, and that it's the manifest rather than a call in `Main` because the manifest always applies before any window exists.
- **Remove `App.config`** from the project and delete the file, so the build no longer writes `MocchiMacro.exe.config`.

After this ticket the tool window is scaled once at startup, as today, but no longer rescales when dragged to a monitor with different scaling. Ticket 03 adds that back.

## Acceptance criteria

- [ ] A Release build's output folder has `MocchiMacro.exe` and no `MocchiMacro.exe.config`.
- [ ] In the build output folder, the new .exe shows the typing rate and game saved by the previous build (with its config file) from that same folder. Saved settings are stored per .exe path, so this check must use the same folder.
- [ ] Copied alone into an empty folder, the .exe starts, with the default typing rate and game.
- [ ] With Windows scaling at 125% or 150% on the game's monitor, the lone .exe types correctly into MR2 DX windowed at 1600×900, into MR2 DX fullscreen, and into MR1 DX windowed.
- [ ] At that scaling the tool's window text is sharp, not blurry.
- [ ] Noted in a comment on this ticket: whether the occasional missed click at 125% still happens.
- [ ] The automated tests pass unchanged.

## User stories covered

1, 2, 5
