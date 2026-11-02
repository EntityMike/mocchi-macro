# 02: Fallback combinations and the hotkey label

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

- **Fallbacks.** At startup, try Ctrl+Alt+Shift+M, then Ctrl+Shift+F9, then Ctrl+Alt+Shift+F9, and keep the first that registers. All three behave as ticket 01's hotkey does.
- **The hotkey label.** It's always visible in the window: `Hotkey: Ctrl+Alt+Shift+M` (or whichever registered), or `Hotkey: none`. Place it sensibly in today's window, growing the window taller if needed. Its final place comes with the window-look spec.
- **One-time status line at startup**, in the problem red:
  - **On a fallback:** "Ctrl+Alt+Shift+M is taken by another program, so the hotkey is Ctrl+Shift+F9." (or Ctrl+Alt+Shift+F9).
  - **With none:** "All three hotkeys are taken by other programs. Use the Go button."
  - **With the default:** no message.

## Acceptance criteria

- [ ] With four copies of the tool started one after another, the copies show, in order:
  - [ ] Ctrl+Alt+Shift+M with no message
  - [ ] Ctrl+Shift+F9 with the fallback message
  - [ ] Ctrl+Alt+Shift+F9 with the fallback message
  - [ ] none, with the none message.
- [ ] In each copy, the label's combination is the one that brings that copy to the front.
- [ ] In the copy with none, Go still types correctly.
- [ ] After closing the first copy, a newly started copy gets Ctrl+Alt+Shift+M again.
- [ ] The first Go after a startup message replaces it with Go's own status, as usual.
- [ ] No control is cut off or overlapped at 100% and 125% scaling.

## User stories covered

7, 8, 9, 10
