# 02: New layout, with the idle mascot

Status: ready-for-agent

Blocked by: 01, global-hotkey 02

Parent: [spec](../spec.md)

## What to build

Rework the main window's layout:

- **Font:** Segoe UI 9pt.
- **Width:** about 420 px of client area, so the title shows in full. Check it with "Mocchi Macro 1.0.0", the title the release spec sets.
- **Arrangement:** the mascot's area on the left, the controls in a column to its right, even margins, a bigger Go button. The hotkey label gets its final place.
- **Kept:** the fixed size and the missing maximize stay.

Show the mascot's idle still frame, from the embedded PNGs, at a whole-number scale:

- The scale is the window's scale times 3, rounded to the nearest whole number, with halves rounding down (3× at 100%, 4× at 125% and 150%).
- It's drawn without smoothing, so the pixels stay sharp.
- The mascot's area is sized from that scale.

There's no animation yet; that's ticket 04.

## Acceptance criteria

- [ ] At 100%, 125% and 150% scaling, nothing is cut off or overlapping, and the full title shows in the title bar.
- [ ] At each scaling, the mascot is crisp, with square pixels, not blurred.
- [ ] Go, Enter, the hotkey, the hotkey label, the typing rate, the game dropdown, the tooltip and the status line all work as before.
- [ ] The window still can't be resized or maximized.
- [ ] The automated tests pass unchanged.

## User stories covered

1, 2, 7
