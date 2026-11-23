# Typing clicks the keyboard with the real mouse

The game's keyboard can't be typed on with key presses: it only takes mouse clicks on its keys, which is why organizers click names out by hand and why the tool exists. So the typist moves the real cursor onto each key and clicks it (`SetCursorPos` and `SendInput`), and everything about typing follows from that.

## Consequences

- The organizer has to leave the mouse alone while Go types, and the README says so.
- Each click holds the button down for 50 ms, since the game reads the mouse once per frame and can miss a press and release sent back to back.
- Clicks land by position, so the tool needs each game's key positions (`KeyboardLayout`) and has to find the picture inside the game window before every click.
- The game has to be in front to take the clicks, so Go brings it there and confirms it got there.
