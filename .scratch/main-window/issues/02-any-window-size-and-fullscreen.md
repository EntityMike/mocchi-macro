# 02: Land keys at any window size and in fullscreen

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make the clicks land on the right keys at every screen setup, not just a 1600×900 window.

- Scale key positions from the 1600×900 reference to the picture's actual size.
- In fullscreen on a monitor that isn't 16:9, find the picture: the largest 16:9 area that fits the client area, centered between black bars.
- Declare the app PerMonitorV2 DPI aware so coordinates are real pixels at Windows scaling above 100%.
- Clicks follow the window if it moves partway through. The typist already reads the client area before every click, so this ticket adds the test and checks it by hand.

## Acceptance criteria

- [ ] Test: at 854×480 and 1920×1080, every key lands at its reference position scaled to that size.
- [ ] Test: at a 3440×1441 client area, keys land inside a 2562×1441 picture with 439 px bars on each side.
- [ ] Test: if the client area's screen position changes partway through, later clicks follow it.
- [ ] By hand: "Mocchi" types correctly windowed at 854×480 and 1920×1080, and with Windows display scaling above 100%.
- [ ] By hand: "Mocchi" types correctly in fullscreen on a monitor that isn't 16:9, with the game already in front (focus is ticket 05).

## User stories covered

20, 21, 22, 23

## Comments

- 2026-10-30, from implementing 01: scaled positions are cut off to whole pixels (391.8 becomes 391), as the keyboard test and ticket 01's typist do. Work out the expected values in the 854×480, 1920×1080 and 3440×1441 tests with the same rule.
- 2026-10-30, from implementing 01: ticket 01 removed the unused `ReferenceWidth`/`ReferenceHeight` constants from `KeyboardLayout`; add them back here when scaling needs them.
