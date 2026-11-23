# Key positions are measured once, in a 1600×900 picture, and scaled

`KeyboardLayout` holds each game's key centers as measured in a 1600×900 screenshot of its keyboard (`docs/screenshots/`). Before every click, the typist finds the game's 16:9 picture inside the game window, which has black bars around it in fullscreen on a monitor that isn't 16:9, and scales the key's position into it. The organizer never calibrates, and the same numbers work at every window size, in fullscreen, and at any display scaling.

## Considered Options

- **Calibration,** with the organizer clicking a few keys to show the tool where they are. Rejected: it's a setup step for every organizer, and the built-in positions land correctly at every size tested.

## Consequences

- Every key position in both layouts is in 1600×900 coordinates, never screen coordinates. A new game's layout is measured the same way.
- It relies on both games drawing the keyboard at the same place in the picture at every size. A game that laid out its keyboard differently at some size would need more than scaling.
