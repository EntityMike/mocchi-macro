# Main window: type a search text into the keyboard

Status: ready-for-agent

## Problem Statement

The organizer has to click out every monster name, letter by letter, on MR2 DX's keyboard during multiplayer battle setup. They type name after name during a tournament, often on stream, which slows down every battle. To save clicks they type as few letters as possible, and then the search turns up near-matching or identical names.

## Solution

A small window the organizer can put on any monitor. It has a box for the search text, a **Go** button, and a box for the typing rate (keys per second, default 10). When the organizer presses Go, the tool finds the MR2 DX game window and brings it to the front. It clears the monster field by clicking **BS**, then clicks out the search text on the keyboard at the chosen typing rate. It stops before **Ent.**, so the organizer can check the text and run the search themselves. A status line says what happened, or what went wrong.

## User Stories

1. As an organizer, I want to type a search text into a box in the tool's window, so that I don't have to click it out letter by letter on the keyboard.
2. As an organizer, I want a Go button that types my search text into the game, so that I can start typing with one click.
3. As an organizer, I want pressing Enter in the search text box to do the same as Go, so that I can type a name and send it without reaching for the mouse.
4. As an organizer, I want the search text box to hold at most 12 characters, so that I can't enter more than the monster field fits.
5. As an organizer, I want the search text box to ignore characters the keyboard doesn't have (space, accented letters, and so on), so that Go never fails halfway through a name.
6. As an organizer, I want a short note when the box drops a character I typed or pasted, so that my text never changes without me knowing why.
7. As an organizer, I want to paste a name into the search text box and have it cleaned the same way as typed text, so that I can copy names from the bracket site.
8. As an organizer, I want to type `○ × △ □` and the other symbols the keyboard has (for example by pasting them), so that I can search for monsters whose names use them.
9. As an organizer, I want Go to clear the monster field before typing, so that the 1P search text left in the field doesn't get mixed into the 2P search.
10. As an organizer, I want Go to stop before clicking Ent., so that I can check the monster field before the search runs.
11. As an organizer, I want to set the typing rate in keys per second, so that I can slow it down if my PC drops clicks.
12. As an organizer, I want the typing rate to default to 10 keys per second, so that typing is fast without any setup.
13. As an organizer, I want the typing rate box to accept only whole numbers from 1 to 10, so that I can't enter a rate the tool hasn't been tested at, or a value that isn't a number.
14. As an organizer, I want up and down arrows on the typing rate box, so that I can change it without typing.
15. As an organizer, I want the tool to remember my typing rate between runs, so that I only find the right rate for my PC once.
16. As an organizer, I want the search text to be empty when the tool starts, so that I don't send an old name by accident.
17. As an organizer, I want the search text to stay in the box after Go, so that I can send it again if something went wrong.
18. As an organizer, I want Go to bring the game window to the front itself, so that the first key isn't lost on focusing the game.
19. As an organizer, I want Go to restore a minimized fullscreen game, so that typing works when I played fullscreen and clicked over to the tool.
20. As an organizer, I want the keys to land in the right place whatever window size the game runs at (any of its seven 16:9 sizes), so that I don't have to calibrate anything.
21. As an organizer, I want the keys to land correctly in fullscreen on a monitor that isn't 16:9, so that the black bars around the picture don't throw the clicks off.
22. As an organizer, I want the keys to land correctly at Windows display scaling above 100%, so that the tool works on high-DPI monitors.
23. As an organizer, I want the keys to land correctly even if I move the game window while it's typing, so that a nudge doesn't ruin the name.
24. As an organizer, I want the actual typing rate to match the rate I set, so that the timing I tested is the timing I get.
25. As an organizer, I want a clear message when the game isn't running, so that I know to start it instead of wondering why nothing happened.
26. As an organizer, I want a clear message when the game didn't come to the front, telling me to click the game and try again, so that I can recover quickly.
27. As an organizer, I want typing to stop with a message if the game window closes partway through, so that the tool doesn't keep clicking on whatever is behind it.
28. As an organizer, I want Go and the inputs disabled while the tool is typing, so that I can't start a second run on top of the first.
29. As an organizer, I want the tool's window to stay responsive while it types, so that it doesn't look frozen.
30. As an organizer, I want the status line to say when typing is done, so that I know I can look at the game.
31. As an organizer, I want Go to do nothing but show a message when the search text is empty, so that it doesn't just clear the monster field.
32. As an organizer, I want the tool to run as its own window that I can put on any monitor, so that it doesn't get in the way of the stream layout.
33. As an organizer, I want the window titled "Mocchi Macro", so that I can find it in the taskbar.

## Implementation Decisions

- **Target game:** MR2 DX only, found by its window title `MF2`. MR1 DX (`MF1`) comes later, since its graphics and some details differ.
- **Keyboard layout module.** Holds the keyboard's layout as measured in the keyboard test, in a 1600×900 reference picture: two blocks of 5 columns × 8 rows, each with its own left edge, plus BS and Ent. The keyboard test never clicked BS or Ent., so their centers are measured from the same 1600×900 screenshot it used (`docs/screenshots/mr2/ingamekeyboard.png`). It answers two questions: does a character have a key, and where is that key's center in reference coordinates? The left block's punctuation key whose character isn't known yet is left out, so it can't be typed. Ent. is in the layout so a later ticket can use it, but nothing clicks it yet.
- **Typist module (the deep module).** It has one operation: type this search text into the game at this typing rate. In order, it:
  1. Checks that every character of the search text has a key. If one doesn't, it clicks nothing, not even BS. The search text box already removes such characters, but this guards text that reaches the typist another way.
  2. Brings the game window to the front, restoring it first if it's minimized. It waits up to 5 s for the game to really be in front, then 500 ms more so the game is ready.
  3. Clicks BS 12 times to clear the monster field.
  4. Clicks each key of the search text.

  It reports one of five outcomes: done, search text has a character without a key, game not found, game didn't come to the front, or game window closed partway through.
  - It scales key positions from the reference picture to the picture's actual size and position, cutting fractions off to whole pixels as the keyboard test does. In a window, the picture fills the client area. In fullscreen on a monitor that isn't 16:9, the picture is the largest 16:9 area that fits, centered between black bars.
  - It reads the game window's client area again before every click, so typing still lands if the window moves.
  - It schedules each click against a clock (click *n* at *n* × interval from the start) instead of sleeping a fixed gap, so late wake-ups don't add up.
  - Each click holds the mouse button down for 50 ms. Games read the mouse once per frame, so a press and release sent back to back can both fall between two frames and be missed.
  - The BS clears count toward the typing rate like any other key.
- **Game port (the one test seam).** The typist talks to the game only through a small interface. It can:
  - find the game window
  - bring it to the front
  - check that it still exists
  - read its client area's size and screen position
  - click a screen point
  - read the clock
  - wait

  The real version wraps `user32.dll` through P/Invoke: `FindWindow`, `IsIconic`/`ShowWindow`, `SetForegroundWindow`/`GetForegroundWindow`, `IsWindow`, `GetClientRect` + `ClientToScreen`, `SetCursorPos` + `SendInput`. Tests use a fake that records each click with a virtual clock, so they run instantly.
- **DPI awareness:** the app declares itself PerMonitorV2 DPI aware, so the coordinates it reads and clicks are real pixels at Windows scaling above 100%.
- **Main window.** It replaces the template form:
  - Title "Mocchi Macro".
  - **Search text box:** at most 12 characters. Characters without a key are removed as they're typed or pasted, and a note under the box says which ones were dropped. It starts empty and keeps its text after Go.
  - **Go button:** it's the form's accept button, so Enter in the search text box runs it. If the search text is empty it only shows a message. Otherwise it disables Go and both input boxes, runs the typist off the UI thread, then re-enables them.
  - **Typing rate box:** a number box with arrows (`NumericUpDown`) at the bottom of the window, whole numbers 1–10, default 10.
  - **Status line:** shows the typist's outcome in plain words. The game-not-in-front message tells the organizer to click the game and press Go again.
- **Saved settings:** the typing rate is saved in the project's existing user settings and restored at startup. The search text isn't saved.
- **Default typing rate:** the hub's suggested default of 1 key per second is replaced by 10. The keyboard test typed correctly every time at 10 keys/s, and rates above that felt less certain.

## Testing Decisions

- A good test drives the typist through its one operation, using a fake game port, and checks only what the game would see: which screen points were clicked, in what order, and when, plus the outcome reported. Tests don't reach into the layout tables or the scaling math directly. That way the inside can be reorganized freely.
- Test framework: a new MSTest test project (the `MSTest.TestFramework` and `MSTest.TestAdapter` NuGet packages), approved for this spec.
- What the typist tests cover:
  - At a 1600×900 client area, each key is clicked at its measured position, for example "Mocchi" lands where the keyboard test clicked.
  - At 854×480 and 1920×1080, positions scale from the reference.
  - At 3440×1441 (fullscreen on a 21:9 monitor), clicks land inside a 2562×1441 picture centered with 439 px bars.
  - Twelve BS clicks come before the first character, and Ent. is never clicked.
  - At typing rate *r*, click *n* happens at *n* × 1000/*r* ms on the fake clock.
  - If the client area moves partway through, later clicks follow it.
  - Game not found, not in front within 5 s, and closed partway through each report their outcome, and no clicks happen after the failure.
  - A search text with a character that has no key reports that outcome, and nothing is clicked.
- The keyboard layout's "does this character have a key" check is tested for the cases the search text box relies on: letters, digits, `○ × △ □`, space, accented letters, and the unknown punctuation key.
- The main window isn't tested automatically. Check it by hand against the game in windowed mode at 1600×900, plus one other size, and once in fullscreen.
- Prior art: there are no tests in the repo yet. The throwaway keyboard test run before this repo existed (`keyboard-test.cs`, not kept here) held the measured key positions and the click, timing and focus behavior proven against the game. It's the reference for the expected click positions, except BS and Ent., which come from the screenshot named under the keyboard layout module.

## Out of Scope

- The global hotkey.
- The preset list of names loaded ahead of time.
- Clicking Ent. after typing, and an option to type full names.
- MR1 DX.
- Calibration (built-in positions work at every tested size).
- The unknown punctuation key on the left block.
- Typing rates above 10 or between whole numbers.
- Bracket tracking and anything else under the hub's scope expansion.
- Signing the .exe.

## Further Notes

- In fullscreen, clicking Go on another monitor takes focus away from the game, so the game minimizes. The typist restores it and brings it to the front. The keyboard test couldn't do that from Git Bash, because Windows only lets the program you last used change which window is in front. Clicking Go makes this tool that program, so it should work, but this is untested. If it fails, the organizer clicks the game and presses Go again, and the hotkey ticket later fixes it properly.
- Typing takes over the mouse, which the hub accepts.
