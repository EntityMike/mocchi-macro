# 03: Remember the game, and clear the box on a switch

Status: resolved

Blocked by: 02

Parent: [spec](../spec.md)

## What to build

- **Remember the game.** Save the picked game in the user settings next to the typing rate, and restore it at startup. Save once, on close, through the save that's already there. Saving on every change risks anti-ransomware software taking the tool for ransomware, as happened with the typing rate. A missing or unknown saved value means MR2 DX.
- **Clear the box on a switch.** Changing the game clears the search text box completely, which also clears the dropped-characters note. The status line is left alone. Organizers rarely switch, and when they do, they're starting over with other monster names, so keeping the old text would only invite sending a name meant for the other game.

## Acceptance criteria

- [x] Pick MR1 DX, close the tool, start it again: MR1 DX is still picked, and its tooltip shows.
- [x] On a first run, with no saved settings, MR2 DX is picked.
- [x] A hand-edited settings file with an unknown game value starts on MR2 DX, without an error.
- [x] With text in the box, switching to the other game leaves the box and the note empty, and the status line unchanged.
- [x] Picking the game that's already selected doesn't clear the box.

## User stories covered

4, 6
