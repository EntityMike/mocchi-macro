# 04: Mascot animations

Status: ready-for-agent

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

- [ ] With the tool waiting, the mascot blinks every few seconds.
- [ ] While Go types a 12-character search text at 1 key/s, the typing loop runs the whole time and stops when typing ends.
- [ ] A Done outcome hops once, holds about 2 s, then idles.
- [ ] Go with the game closed, and Go with an empty box, each droop once, hold, then idle.
- [ ] Go pressed during the hold starts the typing loop at once.
- [ ] Animation stays crisp at 125% scaling, and the window stays responsive throughout.
- [ ] The automated tests pass unchanged.

## User stories covered

4, 5, 6
