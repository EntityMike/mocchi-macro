# 04: README for organizers

Status: ready-for-agent

Blocked by: 01, 02, window-look 04 (the screenshot shows the title and the footer band)

Parent: [spec](../spec.md)

## What to build

Rewrite `README.md` for organizers, covering the twelve points in the spec's README list, in that order:

1. What the tool does
2. Download link
3. Requirements
4. How to use it, hotkey loop first, then what the spec lists around it: the keyboard screen and game dropdown, leaving the mouse alone, the 12-character limit, dropped characters and stand-ins, the status line and mascot, and fullscreen on stream
5. The hotkeys and what they take from other programs
6. Typing rate advice
7. SmartScreen
8. Antivirus
9. The update notice, and that it's the tool's only network use
10. Where settings are saved
11. Not-affiliated line, art license, and that the game screenshots aren't covered by it
12. Building from source.

Use the glossary's words where they fit: organizer, game, keyboard, key, search text, stand-in, typing rate, hotkey, mascot, monster field, Ent., release, update notice.

- **Download link:** `https://github.com/EntityMike/mocchi-macro/releases/latest`.
- **Screenshot:** the animated screenshot of the tool typing is recorded by the repo owner, with any screen recorder they already have. Save it under `docs/`. Until it exists, leave a marked spot for it.

## Acceptance criteria

- [ ] The README covers all twelve points, in order, and reads well to someone who has never seen the tool.
- [ ] Someone who has never seen the tool could type a search text from the how-to alone, in either game, including one with a symbol.
- [ ] The hotkey section lists Ctrl+Alt+Shift+M, Ctrl+Shift+F9 and Ctrl+Alt+Shift+F9 in order, and says the one in use doesn't reach other programs while the tool runs.
- [ ] The SmartScreen steps match what Windows 11 shows for the downloaded .exe.
- [ ] The animated screenshot is in `docs/` and shows in the README.
- [ ] It renders correctly on GitHub (checked once the repo is public, in ticket 05).

## User stories covered

2, 3, 4
