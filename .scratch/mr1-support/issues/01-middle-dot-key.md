# 01: The middle dot key

Status: resolved

Parent: [spec](../spec.md)

## What to build

Make MR2 DX's last untypeable key typeable. The key left out as "unknown" (a comma or a raised dot) is a middle dot, and MR1 DX has the same one, so ticket 02 reuses what this ticket adds.

- Add `·` (U+00B7) to MR2 DX's keyboard layout at the left block's row 6, column 3, where the unknown key was. Its center is (495, 615.4) in the 1600×900 reference picture, on the grid like its neighbours.
- Two new stand-ins for it:

  | Stand-in | Becomes | Why |
  |---|---|---|
  | `` ` `` | · | A small raised mark, and the only free character on an ordinary keyboard that looks at all like a raised dot |
  | `・` (U+30FB) | · | The Japanese middle dot, so names pasted from Japanese text work |

- The tooltip adds `` ` · `` after the existing four. It doesn't list `・`, since it isn't on an ordinary keyboard.
- Update the layout's comments that call the key unknown, and the stand-ins comment that avoids the comma "because the unknown key may turn out to be one".

## Acceptance criteria

- [x] Typing `` Mo`chi `` in the box leaves `Mo·chi`, with no note.
- [x] Pasting `Mo・chi` or `Mo·chi` leaves `Mo·chi`.
- [x] With MR2 DX on the keyboard screen, Go types `Mo·chi` into the monster field.
- [x] The tooltip reads `` @ ○   * ×   ^ △   # □   ` · ``.
- [x] Test: the layout has a key for `·`. The old "unknown punctuation key has no key" case is replaced.
- [x] Test: at 1600×900 with the client area at 0,0, typing `·` clicks (495, 615) after the 12 BS clicks.
- [x] Test: `` ` `` and `・` map to `·`, and neither has a key itself.

## User stories covered

12 (MR2 DX)
