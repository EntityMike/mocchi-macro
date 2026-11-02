# 01: The hotkey brings the tool to the front

Status: ready-for-agent

Parent: [spec](../spec.md)

## What to build

Register Ctrl+Alt+Shift+M as a global hotkey when the main window is created, with no auto-repeat, and release it when the window closes. On the hotkey:

- **While Go is typing:** do nothing.
- **Otherwise:**
  - restore the window if it's minimized
  - bring it to the front and activate it
  - focus the search text box and select all its text.

This ticket has no fallbacks and no label. If Ctrl+Alt+Shift+M is taken, the tool runs without a hotkey and says nothing; ticket 02 adds that.

First check that Windows lets the tool take focus from a hotkey. Bring it up from another program on another monitor and from behind the game. If it doesn't work, stop and record what happens in this ticket's comments.

## Acceptance criteria

- [ ] With MR2 DX windowed and the tool behind it, Ctrl+Alt+Shift+M brings the tool to the front with the search text selected, and typing replaces it.
- [ ] The same works with the tool on another monitor and another program in front.
- [ ] Enter after the hotkey types the search text into the monster field, and the game is in front when typing ends.
- [ ] With MR2 DX fullscreen, the hotkey brings up the tool over the game, and Enter brings the game back to the front and types correctly.
- [ ] With the tool minimized, the hotkey restores it.
- [ ] Pressing the hotkey while typing changes nothing, and the name comes out right.
- [ ] Holding the hotkey down doesn't fire it repeatedly.
- [ ] After the tool closes, Ctrl+Alt+Shift+M works in other programs again.
- [ ] The automated tests pass unchanged.

## User stories covered

1, 2, 3, 4, 5, 6
