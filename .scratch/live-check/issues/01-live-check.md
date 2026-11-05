# 01: Live check of the real window

Status: ready-for-agent

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

- [ ] It builds with the solution and adds no new packages.
- [ ] All its checks pass against the tool as it stands.
- [ ] Breaking the tool makes it fail: a blurred mascot, clicks off their keys, a wrong hold.
- [ ] The saved pictures show the rings on the right keys, on both games' keyboards.
- [ ] Every key a search text can use gets clicked, on both games at 10 keys/s.
- [ ] A run doesn't touch the settings of a copy of the tool in real use.
- [ ] The automated tests pass unchanged.

## User stories covered

1, 2, 3, 4
