# 02: Fallback combinations and the hotkey label

Status: resolved

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

- **Fallbacks.** At startup, try Ctrl+Alt+Shift+M, then Ctrl+Shift+F9, then Ctrl+Alt+Shift+F9, and keep the first that registers. All three behave as ticket 01's hotkey does.
- **The hotkey label.** It's always visible in the window: `Hotkey: Ctrl+Alt+Shift+M` (or whichever registered), or `Hotkey: none`. Place it sensibly in today's window, growing the window taller if needed. Its final place comes with the window-look spec.
- **One-time status line at startup**, in the problem red:
  - **On a fallback:** "Ctrl+Alt+Shift+M is taken by another program, so the hotkey is Ctrl+Shift+F9." (or Ctrl+Alt+Shift+F9).
  - **With none:** "All three hotkeys are taken by other programs, so there's no hotkey. Typing still works." (Reworded on 2026-11-05; see the spec's Further Notes.)
  - **With the default:** no message.

## Acceptance criteria

- [x] With four copies of the tool started one after another, the copies show, in order:
  - [x] Ctrl+Alt+Shift+M with no message
  - [x] Ctrl+Shift+F9 with the fallback message
  - [x] Ctrl+Alt+Shift+F9 with the fallback message
  - [x] none, with the none message.
- [x] In each copy, the label's combination is the one that brings that copy to the front.
- [x] In the copy with none, Go still types correctly.
- [x] After closing the first copy, a newly started copy gets Ctrl+Alt+Shift+M again.
- [x] The first Go after a startup message replaces it with Go's own status, as usual.

## User stories covered

7, 8, 9, 10

## Comments

- 2026-11-02: scaling dropped from the acceptance criteria; this ticket doesn't need it checked.
- 2026-11-02, scripted: all criteria pass. Four copies started one after another showed Ctrl+Alt+Shift+M with no message, Ctrl+Shift+F9 and Ctrl+Alt+Shift+F9 with the fallback message, and none with the none message. Pressing each combination brought its own copy to the front from minimized. After closing the first copy, a new one got Ctrl+Alt+Shift+M. In the copy with none, Go replaced the startup message with "Typing...", then "Done.", and typed "Mocchi" into MR2 DX's monster field with the game in front.
