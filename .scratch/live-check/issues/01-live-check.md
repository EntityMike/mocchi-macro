# 01: Live check of the real window

Status: resolved

Blocked by: window-look 04

Parent: [spec](../spec.md)

## What to build

A program, run by hand, that checks what the real tool's window does:

- **Runs the real tool** and drives it from outside, through window messages.
- **A fake game window:** 1600×900, with the game's window title and a screenshot of that game's keyboard, so a person can watch the cursor land on each key. It stays on top, so clicks can't reach another program.
- **Marks and checks each click:** a ring where it landed, a saved picture of the finished run, and a failure if a click misses its key's expected position.
- **Checks the mascot frame by frame:** idle and blinks, a droop on an empty box and on a closed game, the typing loop, the hop, the hold, and Go cutting the hold short.
- **Both games:** the full checks on MR2 DX, then a typing run on MR1 DX.
- **The whole keyboard, at the end:** every key a search text can use, on both games at 10 keys/s, over as many Go runs as the box's 12 characters take, with a saved picture of each keyboard's rings.
- **Passes or fails on its own,** with an exit code.

It refuses to start while a real game is running.

## Acceptance criteria

- [x] It builds with the solution and adds no new packages.
- [x] All its checks pass against the tool as it stands.
- [x] Breaking the tool makes it fail: a blurred mascot, clicks off their keys, a wrong hold.
- [x] The saved pictures show the rings on the right keys, on both games' keyboards.
- [x] Every key a search text can use gets clicked, on both games at 10 keys/s.
- [x] A run doesn't touch the settings of a copy of the tool in real use.
- [x] The automated tests pass unchanged.

## User stories covered

1, 2, 3, 4

## Comments

**2026-11-05:** Built. Run at real 125% scaling: all checks pass, and each of those breaks made it fail. The rings sit on the right keys for `Mocchi+Tiger` and BS on both keyboards.

**2026-11-05:** Added the whole-keyboard run the repo owner asked for: 80 keys on MR2 DX and 74 on MR1 DX, each over 7 Go runs. Every typeable key gets a ring on the saved pictures, centered on its cell, so small marks like `.` and `_` sit a little off it. Only Ent. has none, since the typist never clicks it. Clicks 5 px off failed these runs too, and no settings file was written during a run.

What it can't show:

- that the game accepts the clicks, since the fake game window only records them
- that the layouts match the game, from its pass or fail alone. The expected positions come from the tool's own layouts, so that's judged by eye, from the rings on the screenshots.

**2026-11-05, closed by the repo owner:** All the criteria are checked at real 125% scaling. The full check also passes at real 100%, where the deliberate breaks weren't repeated.
