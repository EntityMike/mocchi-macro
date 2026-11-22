# 01: Type a search text end to end

Status: resolved

Parent: [spec](../spec.md)

## What to build

The thinnest path from the tool's window to letters in the game's monster field. Every later ticket builds on it.

- A main window titled "Mocchi Macro" with a search text box and a Go button. It replaces the template form.
- The keyboard layout: every key's center in the 1600×900 reference picture, plus whether a character has a key. Leave out the unknown punctuation key.
- The typist and its game port. Go finds the `MF2` game window and clicks BS 12 times. Then it clicks each key of the search text, at a fixed 10 keys per second scheduled against the clock, with a 50 ms hold per click. It reads the client area before every click.
- For now, assume the game runs in a window at 1600×900 and is already in front. Scaling, focus, and error handling come in later tickets. If the game isn't found, the typist reports that so the window doesn't crash, but no friendly message is needed yet.
- The real game port wraps `user32.dll` (see the spec). The fake game port records each click with a virtual clock.
- A new MSTest test project (`MSTest.TestFramework`, `MSTest.TestAdapter`).

## Acceptance criteria

- [x] With MR2 DX windowed at 1600×900 on the keyboard screen and in front, typing "Mocchi" and pressing Go leaves the monster field reading "Mocchi", even when it held other text first.
- [x] Ent. is never clicked.
- [x] Test: at 1600×900, 12 BS clicks come first, then each key at its measured position (the positions measured by the pre-repo keyboard test).
- [x] Test: at 10 keys/s, click *n* happens at *n* × 100 ms on the fake clock.
- [x] Test: the layout says letters, digits, and `○ × △ □` have keys, and space, accented letters, and the unknown punctuation key don't.
- [x] Tests run from Visual Studio's Test Explorer.

## User stories covered

1, 2, 9, 10, 24, 32, 33

## Comments

- 2026-10-30, implemented on branch `main-window`: `keyboard-test.cs` has no BS or Ent. positions, so they were measured from the 1600×900 screenshot `docs/screenshots/mr2/ingamekeyboard.png`: BS at x 1197 on the digits row, Ent. at x 1198 one row below, each within 3 px of the measured center.
- 2026-10-30: the test project uses MSTest's own runner (Microsoft.Testing.Platform), so it needs no `Microsoft.NET.Test.Sdk`. The tests pass when run from the command line; running them from Test Explorer still needs checking in Visual Studio.
- 2026-10-30: the in-game check is still to do by hand. If one old character survives in the monster field, see ticket 05's comment on the first click.
- 2026-10-30, by hand while checking 05: windowed at 1600×900 with text already in the monster field and another window over part of the game, Go cleared the field and left "Mocchi". Ticket 05 now brings the game to the front first, which was the cause of a BS click being lost.
- 2026-10-30, by hand: Run All in Visual Studio's Test Explorer passed 38 of 38.
