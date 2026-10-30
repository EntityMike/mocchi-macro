# 05: Bring the game to the front, report status and failures

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make Go work when the game isn't already in front, and tell the organizer what happened.

- Before the first click, the typist restores the game window if it's minimized and brings it to the front. It waits up to 5 s for the game to really be in front, then 500 ms more.
- The typist reports one of four outcomes: done, game not found, game didn't come to the front, or game window closed partway through. It sends no clicks after a failure.
- A status line in the window shows the outcome in plain words, including ticket 04's "search text has a character without a key". The game-not-in-front message tells the organizer to click the game and press Go again.
- Go with an empty search text only shows a message, and doesn't clear the monster field.
- While typing, Go and both input boxes are disabled. The typist runs off the UI thread so the window stays responsive. When typing ends, Go and the boxes are re-enabled.

## Acceptance criteria

- [ ] Test: when the game isn't found, the outcome is "game not found" and nothing is clicked.
- [ ] Test: when the game doesn't come to the front within 5 s on the fake clock, the outcome says so and nothing is clicked.
- [ ] Test: when the game window closes partway through, the outcome says so and no clicks follow.
- [ ] Test: when the game is minimized, it's restored before the first click.
- [ ] By hand: with the game closed, Go shows "game not found" in the status line.
- [ ] By hand: with the game windowed behind the tool, Go brings it to the front and types correctly.
- [ ] By hand: in fullscreen, clicking Go on another monitor restores the game and types correctly. If Windows blocks this, record the result in this ticket's comments. The fallback is clicking the game and pressing Go again; the hotkey ticket fixes it properly later.
- [ ] By hand: the window can be dragged while typing, and Go is disabled until typing ends.
- [ ] By hand: Go with an empty box shows a message and the monster field is unchanged.

## User stories covered

18, 19, 25, 26, 27, 28, 29, 30, 31

## Comments

- 2026-10-30, from implementing 01: the "game not found" test already exists (`ReportsGameNotFoundAndClicksNothing` in `TypistTests.cs`), and the typist already returns that outcome. That acceptance item only needs the status line message.
- 2026-10-30, from implementing 01: until this ticket brings the game to the front, pressing Go leaves the tool in front, so the first BS click may only activate the game instead of deleting a character. With 12 characters already in the monster field, one can survive. This can fail ticket 01's "even when it held other text first" check by hand; this ticket is the fix.
- 2026-10-30, from implementing 01: Go still runs the typist on the UI thread, so the window freezes while typing. Moving it off is already in this ticket's scope.
- 2026-10-30, by hand while checking 04: with another window (Visual Studio) between the tool and the game, Go clicked into that window instead of the game. The typist aims at the game's position on screen but doesn't bring the game to the front, so the clicks land on whatever window covers that spot. Bringing the game to the front before the first click, as this ticket asks, fixes it.
