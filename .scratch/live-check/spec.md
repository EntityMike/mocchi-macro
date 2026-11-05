# Live check: test the real window without the game

Status: ready-for-agent

Blocked by: window-look 04 (the check covers its mascot animations)

## Problem Statement

The automated tests drive the typist through a fake game port, so they never run the real window or use the mouse. Whether Go starts the right animation, or the clicks really land where they should, was only checked by hand with the game running. The game isn't always available: window-look ticket 02 couldn't check Go in a real game because the VS mode server was down.

## Solution

A program, run by hand, starts the real tool and drives it against a **fake game window**: a window with the game's title and client size that shows a screenshot of that game's keyboard. A person can watch the cursor land on each key, and the program checks the mascot's animations and each click's position itself, then passes or fails.

## User Stories

1. As the repo owner, I want to see the cursor land on the right keys without the game running, so that I get a general idea typing works before trying it in the game.
2. As the repo owner, I want a picture of where each click landed, so that I can check the keys after a run instead of watching it.
3. As a contributor changing the window, I want one command that checks the mascot and the clicks in the real window, so that I don't have to check them by hand each time.
4. As the repo owner, I want every key on both keyboards clicked, so that I can see the whole layout lines up, not just the keys in one name.

## Implementation Decisions

- **Its own project in the solution,** `MocchiMacro.LiveCheck`, with no new packages. It's built with everything else, so it can't stop compiling unnoticed, and it can use the tool's `Game` and `KeyboardLayout`.
  - Rejected: **a single script compiled with `csc.exe`,** as `art/make-art.cs` is. It's limited to C# 5, can break unnoticed, and has no easy way to read the tool's layouts.
- **Run by hand, not with the automated tests.** It takes over the mouse for about two minutes, and anything touching the mouse or focus during a run could fail it.
- **The fake game window:**
  - It's 1600×900, the size the key positions were measured in. The screenshots are 1602×927 with the window's border and title bar, so they're cropped to the client area.
  - It stays on top, so the typist's clicks can't reach another program.
  - It marks each click with a ring and saves a picture of the finished run.
- **Both games:** the full checks on MR2 DX at 1 key/s, slow enough to watch, then a typing run on MR1 DX, whose keys are in different places.
- **The whole keyboard, at the end,** on both games at 10 keys/s. The search text box holds 12 characters, so it takes several Go runs, with the rings building up into one picture of every key. The keys come from asking the tool's layout about every character, so a key added later is covered without changing the check.
- **The mascot's frames are identified by exact pixel match,** against frames built from the raw PNGs rather than with `MascotArt.LoadFrame`, which could otherwise match its own blur. So a frame that isn't crisp matches nothing and fails.
- **The real tool stays safe from the check, and the game from the tool:**
  - It runs its own copy of the `.exe`. Windows keeps a program's settings by its path, so a run can't touch the settings of a copy in real use.
  - It won't start while a real game is running, since Go would click into it.

## Testing Decisions

- A check that can't fail proves nothing, so it's checked by breaking the tool on purpose: a blurred mascot, clicks off their keys, a wrong hold. Each must make it fail.
- The rings on the saved pictures are judged by eye. The expected click positions come from the tool's own layouts, so only the screenshot under the rings shows those layouts match the game.

## Out of Scope

- Showing that the game accepts the clicks. The fake game window only records them, so a check in the real game is still needed.
- Running it automatically with the tests or on a build server.
- Clicking Ent. The typist never does, so the organizer can check the search text first.
