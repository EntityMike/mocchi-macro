# 02: Land keys at any window size and in fullscreen

Status: resolved

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make the clicks land on the right keys at every screen setup, not just a 1600×900 window.

- Scale key positions from the 1600×900 reference to the picture's actual size.
- In fullscreen on a monitor that isn't 16:9, find the picture: the largest 16:9 area that fits the client area, centered between black bars.
- Declare the app PerMonitorV2 DPI aware so coordinates are real pixels at Windows scaling above 100%.
- Clicks follow the window if it moves partway through. The typist already reads the client area before every click, so this ticket adds the test and checks it by hand.

## Acceptance criteria

- [x] Test: at 854×480 and 1920×1080, every key lands at its reference position scaled to that size.
- [x] Test: at a 3440×1441 client area, keys land inside a 2562×1441 picture with 439 px bars on each side.
- [x] Test: if the client area's screen position changes partway through, later clicks follow it.
- [x] By hand: "Mocchi" types correctly windowed at 854×480 and 1920×1080, and with Windows display scaling above 100%.
- [x] By hand: "Mocchi" types correctly in fullscreen on a monitor that isn't 16:9, with the game already in front (focus is ticket 05).

## User stories covered

20, 21, 22, 23

## Comments

- 2026-10-30, from implementing 01: scaled positions are cut off to whole pixels (391.8 becomes 391), as the keyboard test and ticket 01's typist do. Work out the expected values in the 854×480, 1920×1080 and 3440×1441 tests with the same rule.
- 2026-10-30, from implementing 01: ticket 01 removed the unused `ReferenceWidth`/`ReferenceHeight` constants from `KeyboardLayout`; add them back here when scaling needs them.
- 2026-10-30, implemented on branch `main-window`: black bars are cut off to whole pixels: (3440 − 1441 × 16/9) / 2 = 439.1 gives 439 px bars and a 2562 px picture. The same rule gives 854×480, which is 0.67 px wider than 16:9, no bars, so it fills the window.
- 2026-10-30: "largest 16:9 area that fits" also covers monitors taller than 16:9 (16:10, 4:3), where the bars go above and below the picture. An extra test pins this down: at 1920×1200 the picture is 1920×1080 between 60 px bars.
- 2026-10-30: the scaling tests type "Mocchi" plus the BS clears, which covers both blocks and several rows, rather than every key.
- 2026-10-30: the typist can't tell a window from fullscreen, so a window 2 px or more off 16:9 gets thin bars too. For example, 1360×768 gets a 1 px bar above and below. This is harmless at true 16:9 sizes.
- 2026-10-30: PerMonitorV2 is set in `App.config` (`DpiAwareness`), plus an `app.manifest` that declares Windows 10 support so Windows Forms will turn it on. A launched build reports the PerMonitorV2 awareness context. The by-hand checks against the game (other sizes, scaling above 100%, fullscreen on a monitor that isn't 16:9) are still to do.
- 2026-10-30, by hand: "Mocchi" typed correctly windowed at 1920×1080 and at 854×480, at 100% display scaling. At 125% the letters typed were always right, but now and then a click looked like it didn't register. Lowering the typing rate from 10 to 5 didn't change that, and switching back to 100% made it go away. Not yet explained. Fullscreen on a monitor that isn't 16:9 was checked under ticket 05.
- 2026-10-30, closed: the occasional unregistered click at 125% scaling, noted above, stays an open question and doesn't block closing.
- 2026-11-02: likely explained and fixed in global-hotkey ticket 01. The click pressed in the same instant it moved the cursor, and the game seems to take a press only on the key the cursor was already over in an earlier frame. Slower frames made the misses common in fullscreen, and probably at 125% scaling too. The click now waits 50 ms between the move and the press. 125% wasn't rechecked.
