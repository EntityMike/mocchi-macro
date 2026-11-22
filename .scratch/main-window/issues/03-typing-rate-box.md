# 03: Typing rate box

Status: resolved

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Let the organizer set the typing rate instead of the fixed 10 keys per second.

- A number box with arrows (`NumericUpDown`) at the bottom of the window. It takes whole numbers 1–10, default 10.
- Go passes its value to the typist as the typing rate.
- The typing rate is saved in the project's existing user settings when it changes, and restored at startup.

## Acceptance criteria

- [x] The box accepts only whole numbers 1–10, and changes with the arrows.
- [x] On first run it shows 10.
- [x] After changing it and restarting the tool, it shows the changed value.
- [x] Test: at typing rates 1 and 4, click *n* happens at *n* × 1000/rate ms on the fake clock, BS clicks included.
- [x] By hand: at typing rate 2, "Mocchi" visibly types about two keys per second.

## User stories covered

11, 12, 13, 14, 15

## Comments

- 2026-10-30, implemented on branch `main-window`: the number box keeps a typed fraction as its value while showing it rounded (typing 2.5 shows 3 but holds 2.5). The window rounds the value to match, so the rate used and saved is always the whole number shown.
- 2026-10-30: the typing rate is a user-scoped `TypingRate` setting. It is saved to `user.config` under `%LOCALAPPDATA%\MocchiMacro` on every change, and clamped to 1–10 when it is read back.
- 2026-10-30: checked by driving the built app without the game: it shows 10 on first run, the arrows step it, 0 becomes 1, 99 becomes 10, `abc` is rejected, and a changed value comes back after a restart. The by-hand check at typing rate 2 against the game is still to do.
- 2026-10-30, found while checking 06: saving on every change froze the tool. A quick run of arrow clicks rewrote `user.config` six times in under a second. Anti-ransomware software read that as ransomware, since reading a file, writing a new copy, and replacing the original is what encryption looks like, and suspended the process. The typing rate is now saved once, when the window closes, and still restored at startup. Checked with a copy of the built app: six quick Down presses wrote nothing while it was open, it wasn't suspended, the value was saved on close, and it came back after a restart. A crash or a killed process loses a rate changed in that session.
- 2026-10-30, by hand: at typing rate 2, "Mocchi" typed at about two keys per second.
