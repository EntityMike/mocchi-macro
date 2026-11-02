# Single .exe: ship the tool as one file

Status: resolved

## Problem Statement

The tool is meant to reach organizers as a single .exe they download and run. Today the build also writes `MocchiMacro.exe.config` next to it, and that file holds the setting that makes the tool DPI-aware (PerMonitorV2). An organizer who gets the .exe on its own loses that setting without knowing it. At Windows display scaling above 100%, the tool then sees the game window in scaled-down coordinates instead of real pixels, its clicks can land in the wrong places (most likely with several monitors at different scaling), and its own window is drawn blurry.

The setting can't just be copied into the .exe. The config file actually turns on two things:

1. **Per-monitor DPI awareness for the whole process**, so Windows reports real pixels for the game window and the cursor. This is what typing depends on, and it can be declared in the .exe's embedded manifest instead.
2. **WinForms' own high-DPI handling**, which rescales the tool's window when it's dragged to a monitor with a different scaling. .NET Framework 4.8's WinForms reads this only from the config file. Declaring awareness in the manifest or in code doesn't turn it on, and there's no supported switch that does.

## Solution

First, the main window gets a fixed size: the organizer can't resize or maximize it, only minimize it. Its controls don't stretch, so other sizes only add empty space or cut controls off.

The tool turns on per-monitor DPI awareness from its embedded manifest, and `App.config` is deleted, so the build writes only `MocchiMacro.exe`. Typing works exactly as it does today, at any scaling, on any monitor.

The second part isn't replaced. The tool's window is scaled once at startup for the main monitor's scaling, as before, and keeps that size in pixels on any other monitor. On a monitor at different scaling it looks smaller or larger than on the main one. A window that rescales itself was tried (ticket 03) and dropped: it clipped controls at common scalings.

## User Stories

1. As an organizer, I want to download one .exe and run it, so that there's no second file to keep next to it or lose.
2. As an organizer at Windows display scaling above 100%, I want the tool's clicks to land on the right keys, windowed and fullscreen, so that the one-file tool types as well as today's build does.
3. As an organizer with monitors at different scaling, I want the tool's window to keep a sensible size and sharp text when I drag it from one monitor to another, so that it doesn't turn tiny or huge. *Not covered: dropped with ticket 03.*
4. As an organizer whose tool window opens on a monitor at different scaling from the main one, I want it to open at the right size there, so that I don't have to drag it away and back. *Not covered: dropped with ticket 03.*
5. As an organizer updating from a build that came with `MocchiMacro.exe.config`, I want my saved typing rate and game to carry over, so that dropping the config file changes nothing I set.
6. As an organizer, I want the tool's window to stay at its own size, with no resizing or maximizing, so that its controls are never cut off or lost in empty space.

## Implementation Decisions

- **A fixed-size main window.** Its border style is fixed (`FixedSingle`), so its edges can't be dragged, and its maximize button is off, which also blocks maximizing from the title bar, the keyboard and the window menu. Minimize stays: it doesn't change the layout, and the organizer may want the tool out of the way between battles. Snapping to a screen edge doesn't resize a fixed-border window either.
- **DPI awareness in the manifest.** `app.manifest` declares `<dpiAwareness>PerMonitorV2</dpiAwareness>` in the Windows settings section. It keeps the Windows 10 `supportedOS` entry. Its header comment, which says DPI awareness lives in App.config and warns against setting it in the manifest, is rewritten to say why it's set here: the .exe ships alone, and Microsoft recommends the manifest over calling `SetProcessDpiAwarenessContext` at startup, because the manifest is guaranteed to apply before any window exists.
  - Rejected: **the call at the top of `Main`** has the same effect, but it fails silently if anything ever sets awareness first.
  - Rejected: **no DPI awareness.** Windows then translates the game window's coordinates through one scale factor for the whole tool, which is approximate on one monitor and wrong across monitors at different scaling, and the window is drawn blurry.
- **No App.config.** It's removed from the project, so the build no longer writes `MocchiMacro.exe.config`. Nothing else is lost.
  - Its `supportedRuntime` line isn't needed: a .NET Framework 4.x .exe with no config runs on the CLR it was built for, and keeps 4.8's behaviour through the target framework attribute MSBuild builds into it.
  - User settings don't need it either: their defaults come from the settings class, and saved values go to `user.config` under `%LOCALAPPDATA%` as before. The settings' storage folder is named after the .exe's path and version, not the config file, so existing saved settings carry over.
- **No rescaling across monitors.** WinForms scales the window once at startup for the system DPI (the main monitor's scaling at sign-in), as before. Moved to a monitor at different scaling, or started there, the window keeps that size in pixels, so it looks smaller or larger there.
  - Rejected: **the main window rescaling itself** by resizing its font on Windows' "DPI changed" message and at startup ([ticket 03](issues/03-rescale-window-across-monitors.md)). It was built and dropped. At 125%, 175% and 200% the controls outgrew the window and were clipped on the right, and it didn't work well when tried by hand. The ticket's comments have the details for any future attempt.
- **No ADR.** Moving the setting back into a config file would be easy if ever needed, so this decision is recorded here and in the manifest's comment.

## Testing Decisions

- Nothing here is in the typist or keyboard layouts, which the automated tests cover; they keep passing unchanged. The fixed window, the manifest and the missing config file are checked by hand, like the rest of the main window.
- **Fixed size:** dragging the window's edges and corners doesn't resize it. Maximize is unavailable from the title bar button, a double-click on the title bar, Win + ↑ and the window menu. Snapping to a screen edge doesn't resize it. Minimize and restore still work.
- **One file:** a Release build's output has `MocchiMacro.exe` and no `MocchiMacro.exe.config`. In the same folder as the previous build, it shows the typing rate and game saved by that build. Copied alone into an empty folder, it starts, with defaults, since saved settings are stored per .exe path.
- **Typing at scaling above 100%,** from the lone .exe, with Windows scaling at 125% or 150% on the monitor the game is on:
  - MR2 DX windowed at 1600×900, and MR2 DX fullscreen.
  - MR1 DX once, windowed.
  - Note whether the occasional missed click seen at 125% still happens. It's not fixed here, but this change could affect it.

## Out of Scope

- **The occasional missed click at 125% scaling** (seen in earlier testing by hand). This spec only records whether it still happens.
- **The release itself:** the README, the download, the update notice, and whether the `.pdb` ships.
- **Keeping saved settings across version updates and .exe moves.** It had a ticket, dropped on 2026-11-20 with nothing replacing it.
- **Rescaling the tool's window across monitors at different scaling** (user stories 3 and 4). See "No rescaling across monitors" above.
- **Turning on WinForms' high-DPI handling** some other way, such as setting its private configuration field through reflection. It works today, but a .NET update could break it silently.

## Further Notes

- **How this was checked.** While specifying this, small probe programs confirmed:
  - **Manifest or code awareness without the config file:** the process was per-monitor aware, but WinForms' DPI-change handling stayed off.
  - **No config file at all:** user settings still loaded their defaults, saved, and reloaded.

  The probes ran at 100% scaling.
