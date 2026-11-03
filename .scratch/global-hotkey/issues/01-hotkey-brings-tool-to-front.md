# 01: The hotkey brings the tool to the front

Status: resolved

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

- [x] With MR2 DX windowed and the tool behind it, Ctrl+Alt+Shift+M brings the tool to the front with the search text selected, and typing replaces it.
- [x] The same works with the tool on another monitor and another program in front.
- [x] Enter after the hotkey types the search text into the monster field, and the game is in front when typing ends.
- [x] With MR2 DX fullscreen, the hotkey brings up the tool over the game, and Enter brings the game back to the front and types correctly.
- [x] With the tool minimized, the hotkey restores it.
- [x] Pressing the hotkey while typing changes nothing, and the name comes out right.
- [x] Holding the hotkey down doesn't fire it repeatedly.
- [x] After the tool closes, Ctrl+Alt+Shift+M works in other programs again.
- [x] The automated tests pass unchanged.

## User stories covered

1, 2, 3, 4, 5, 6

## Comments

- 2026-11-02: Ctrl+Shift+M, the first default, was already taken on the dev PC, by another program's default hotkeys. Such programs often claim most Ctrl+Shift+letters, so the order is now Ctrl+Alt+Shift+M, Ctrl+Shift+F9, Ctrl+Alt+Shift+F9 (see the spec's combinations table).
- 2026-11-02, scripted: the focus check passes with synthetic key presses from another program in front. The hotkey restored the minimized tool, brought it to the front with the search text selected, and the combination was free again after the tool closed. Behind the game and on another monitor still need checking by hand.
- 2026-11-02, by hand: everything passes except fullscreen, where keys typed hit or miss at any typing rate.
- 2026-11-02, scripted against MR2 DX fullscreen at 3440x1441: the cause was the click, not the hotkey. The typist pressed the mouse button in the same instant it moved the cursor, and the game seems to take a press only on the key the cursor was already over in an earlier frame. Fullscreen's slower frames made the misses common: 4 of 5 runs dropped keys, also with no tool or hotkey involved. A longer hold at a slower rate still failed 2 of 4. Waiting 30 ms between the move and the press passed 6 of 6. The click now waits 50 ms, for margin, and the full hotkey flow passed 8 of 8. The automated tests can't cover this: they swap out the real port that clicks.
- 2026-11-02, by hand and scripted: all criteria pass. A fullscreen game doesn't minimize when the tool comes up, as first assumed; it stays behind the tool, so the fullscreen criterion was reworded to match. Enter brought the game back and typed correctly, 8 of 8 scripted.
