# 04: Mascot animations

Status: resolved

Blocked by: 02

Parent: [spec](../spec.md)

## What to build

Animate the mascot from ticket 01's frames and timings:

- **Idle:** the still frame, with a blink every few seconds.
- **Typing:** while Go types, the curl loop repeats at its own steady speed, not tied to key clicks.
- **Done:** when Go's outcome is Done, the hop plays once.
- **Problem:** when a Go ends in a problem (any other outcome, or an empty box), the droop plays once.
- **After Done or Problem:** the last frame holds for about 2 seconds, then idle resumes. A new Go cuts the hold short and starts the typing loop.
- **Startup status messages,** such as a taken hotkey, don't trigger Problem.

## Acceptance criteria

- [x] With the tool waiting, the mascot blinks every few seconds.
- [x] While Go types a 12-character search text at 1 key/s, the typing loop runs the whole time and stops when typing ends.
- [x] A Done outcome hops once, holds about 2 s, then idles.
- [x] Go with the game closed, and Go with an empty box, each droop once, hold, then idle.
- [x] Go pressed during the hold starts the typing loop at once.
- [x] Animation stays crisp at 125% scaling, and the window stays responsive throughout.
- [x] The automated tests pass unchanged.

## User stories covered

4, 5, 6

## Comments

**2026-11-05:** Implemented. The repo owner agreed to a small `MascotAnimation` class with its own tests, though the spec has the window checked by hand. It holds the timing rules; the window only tells it what happened and draws the frame it names.

Checked by a scripted run of the real .exe at real 100% and 125% scaling, matching each captured frame exactly against the PNGs:

- idle blinks about every 3.2 s, with a startup message showing and no droop
- Go with an empty box, and Go with the game closed, each droop once, then idle about 2.5 s later
- 12 characters at 1 key/s into a fake game window titled `MF2` loop for all 23.6 s, then hop once and hold
- Go during that hold starts the loop at once
- the window answered at once throughout.

**2026-11-05, closed by the repo owner:** All the criteria are checked, by those scripted runs rather than in a real game. Typing into a real game in the new window is still unseen, as in ticket 02.
