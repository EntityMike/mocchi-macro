# 03: Typing rate box

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Let the organizer set the typing rate instead of the fixed 10 keys per second.

- A number box with arrows (`NumericUpDown`) at the bottom of the window. It takes whole numbers 1–10, default 10.
- Go passes its value to the typist as the typing rate.
- The typing rate is saved in the project's existing user settings when it changes, and restored at startup.

## Acceptance criteria

- [ ] The box accepts only whole numbers 1–10, and changes with the arrows.
- [ ] On first run it shows 10.
- [ ] After changing it and restarting the tool, it shows the changed value.
- [ ] Test: at typing rates 1 and 4, click *n* happens at *n* × 1000/rate ms on the fake clock, BS clicks included.
- [ ] By hand: at typing rate 2, "Mocchi" visibly types about two keys per second.

## User stories covered

11, 12, 13, 14, 15

## Comments

- 2026-10-30, implemented on branch `main-window`: the number box keeps a typed fraction as its value while showing it rounded (typing 2.5 shows 3 but holds 2.5). The window rounds the value to match, so the rate used and saved is always the whole number shown.
- 2026-10-30: the typing rate is a user-scoped `TypingRate` setting. It is saved to `user.config` under `%LOCALAPPDATA%\MocchiMacro` on every change, and clamped to 1–10 when it is read back.
- 2026-10-30: checked by driving the built app without the game: it shows 10 on first run, the arrows step it, 0 becomes 1, 99 becomes 10, `abc` is rejected, and a changed value comes back after a restart. The by-hand check at typing rate 2 against the game is still to do.
