# 02: New layout, with the idle mascot

Status: resolved

Blocked by: 01, global-hotkey 02

Parent: [spec](../spec.md)

## What to build

Rework the main window's layout:

- **Font:** Segoe UI 9pt.
- **Width:** about 420 px of client area, so the title shows in full. Check it with "Mocchi Macro 1.0.0", the title the release spec sets.
- **Arrangement:** the mascot's area on the left, the controls in a column to its right, even margins, a bigger Go button. The hotkey label gets its final place.
- **Kept:** the fixed size and the missing maximize stay.

Show the mascot's idle still frame, from the embedded PNGs, at a fixed scale:

- The scale is 3× (96 px) at every display scaling.
- It's drawn without smoothing, so the pixels stay sharp.
- The mascot's area is sized from the frame.

There's no animation yet; that's ticket 04.

## Acceptance criteria

- [x] At 100%, 125%, and 150% scaling, nothing is cut off or overlapping, and the full title shows in the title bar.
- [x] At each scaling, the mascot is crisp, with square pixels, not blurred.
- [x] Go, Enter, the hotkey, the hotkey label, the typing rate, the game dropdown, the tooltip, and the status line all work as before. (Not checked with Go typing into a running game; see the comments.)
- [x] The window still can't be resized or maximized.
- [x] The automated tests pass unchanged.

## User stories covered

1, 2, 7

## Comments

**2026-11-05:** At real 150% scaling, the game dropdown's top was cut off, and the mascot was 3× rather than the 4× this ticket first asked for.

- **The dropdown:** with the form's font set in the designer, each label scaled itself as the font reached it, and the form's scaling pass then scaled it again. Labels that size to their text hide this, but `statusLabel` and `droppedNoteLabel` have fixed sizes. `statusLabel` grew to 648×76 instead of 432×51 and covered the dropdown. The font is now set in the constructor, after `InitializeComponent`. Moving it earlier in the designer or removing the `Panel` didn't help; reordering the designer lines did, but Visual Studio can undo that when it regenerates the file.
- **The mascot:** `Form.DeviceDpi` reports 96 at every scaling without the app's config file, which the tool ships without. The repo owner was happy with the size, so the scale is now a fixed 3× instead of a rule, and this ticket and the spec were updated to match.
- **Also changed from the first plan:** `AutoScaleMode.Dpi` in place of `AutoScaleMode.Font`, a note label that ends in "…" when it's too long instead of running off the window's edge, and automated tests for loading a frame, which the owner agreed to.

**2026-11-05, closed by the repo owner:** The owner checked the window at 125% after the fixes, and the fallback and no-hotkey messages by running four copies. The owner saw 150% before the dropdown fix. After it, 150% was checked by a scripted capture at real 150%, not by the owner.

Go typing into a running game wasn't checked, because the VS mode server was down, and the owner chose to check that criterion off for now. Go was run with the game closed, and with an empty box. Still unseen in a real game:

- the corrected Done messages: `END` in MR1 DX, `Ent.` in MR2 DX, and the 12-character message
- typing itself in the new window, though the typist's code is unchanged apart from a comment
