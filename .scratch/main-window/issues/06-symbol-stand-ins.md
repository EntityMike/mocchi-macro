# 06: Stand-ins for the symbol keys

Status: ready-for-agent

Blocked by: 04

Parent: [spec](../spec.md)

## What to build

Let the organizer type the keyboard's four symbol keys from an ordinary keyboard. Today `○ × △ □` can only be pasted or picked from Windows' symbol panel.

- Four **stand-ins**, characters the keyboard has no key for, are swapped in the search text box for the symbol keys they stand for:

  | Stand-in | Becomes | Reminder |
  |---|---|---|
  | `@` | ○ (U+25CB) | round |
  | `*` | × (U+00D7) | multiplication sign in code |
  | `^` | △ (U+25B3) | pointed top |
  | `#` | □ (U+25A1) | boxy |

- The swap happens in the box, as the stand-in is typed or pasted, so the box always shows exactly what Go will type. The typist never sees a stand-in, and its "character without a key" check is unchanged.
- A swap is one character for one character, so the caret doesn't move and the note under the box doesn't mention it.
- Swaps happen before characters without a key are dropped and before the 12-character limit is applied. A typed stand-in counts as a character that has a key, so at 12 characters it does nothing, like any other typed key (no "Cut to 12" note).
- The stand-in table lives in the keyboard layout, next to the keys it maps to.
- A tooltip on the search text box lists the stand-ins: `@ ○   * ×   ^ △   # □`.
- Add **Stand-in** to `GLOSSARY.md` under Typing: a character the keyboard has no key for, which the search text box swaps for the symbol key it stands for.

## Decisions

- **Swap in the box, not at Go.** The organizer checks the box before running the search, so it must match what lands in the monster field character for character. Swapping at Go would leave `@` in the box while the game shows ○. It would also put the stand-in rule in two places: the box would have to let `@` through, and the typist would have to translate it.
- **These four characters.** Each looks a little like its symbol, and all four are Shift + a number key on a US keyboard. The comma is avoided, because the keyboard's unknown punctuation key may turn out to be a comma.

## Acceptance criteria

- [ ] Typing `Mocchi@` leaves `Mocchi○` in the box, with no note.
- [ ] Typing `*`, `^` and `#` gives `×`, `△` and `□`.
- [ ] Pasting `@*^#` leaves `○×△□`; pasting `Mo cchi@` leaves `Mocchi○` and a note naming only the space.
- [ ] Typing a stand-in in the middle of the text leaves the caret just after the new symbol.
- [ ] At 12 characters, typing a stand-in does nothing and shows no note.
- [ ] Hovering over the box shows the stand-in tooltip.
- [ ] Test: each stand-in maps to its symbol key, and no stand-in has a key itself.
- [ ] `GLOSSARY.md` defines **Stand-in**.

## User stories covered

8 (extends it: the symbols can be typed, not only pasted)
