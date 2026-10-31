# MR1 support: type a search text into MR1 DX too

Status: ready-for-agent

## Problem Statement

The tool only types into MR2 DX. Organizers who run Monster Rancher 1 DX tournaments still click out every monster name on MR1 DX's keyboard by hand. The two games run the same multiplayer search flow from the same launcher, with the same resolutions and fullscreen option, but MR1 DX's keyboard looks different: its keys sit in other places, it has a different set of symbols, and its BS and Ent. are labelled `←` and `END`.

Both keyboards also have a middle dot key that the tool can't type yet. In MR2 DX it was left out as "unknown" (a comma or a raised dot); it turned out to be a middle dot, and MR1 DX has the same one.

## Solution

A small dropdown in the tool's window picks the game: "MR1 DX" or "MR2 DX", defaulting to MR2 DX, since most organizers play MR2. Go then finds that game's window and clicks that game's keyboard. The search text box keeps only characters the selected game has a key for, and the tooltip lists only that game's stand-ins. The tool remembers the game between runs. Switching games clears the search text box, since the organizer is starting over with other monster names.

The middle dot becomes a key in both games, typed with the stand-in `` ` `` or by pasting `·` or `・`.

## User Stories

1. As an organizer of an MR1 DX tournament, I want the tool to type into MR1 DX, so that I get the same speed-up as MR2 DX organizers.
2. As an organizer, I want to pick the game from a dropdown with "MR1 DX" and "MR2 DX", so that the tool clicks the right keyboard.
3. As an organizer, I want the dropdown to default to MR2 DX, so that most organizers never have to touch it.
4. As an organizer, I want the tool to remember the game I picked between runs, so that I only pick MR1 DX once.
5. As an organizer, I want the dropdown disabled while the tool is typing, so that the game can't change partway through a name.
6. As an organizer, I want switching games to clear the search text box, so that a name meant for the other game isn't sent by accident.
7. As an organizer, I want the search text box to drop characters the selected game has no key for, with the usual note, so that Go never fails halfway through a name.
8. As an organizer, I want the stand-ins tooltip to list only the stand-ins the selected game can use, so that I'm not offered a symbol the game doesn't have.
9. As an organizer, I want the game-not-found message to name the game I picked, so that I can tell when I've picked the wrong one.
10. As an organizer playing MR1 DX, I want the keys to land correctly at every window size, in fullscreen, and at Windows display scaling above 100%, just as in MR2 DX.
11. As an organizer playing MR1 DX, I want Go to clear the monster field before typing, so that the 1P search text doesn't get mixed into the 2P search.
12. As an organizer, I want to type the middle dot in either game, with `` ` `` on an ordinary keyboard or by pasting `·` or `・`, so that I can search for names that use it.

## Implementation Decisions

- **The game.** Two games, MR1 DX and MR2 DX (see **Game** in `GLOSSARY.md`). Each has a name shown in the dropdown, a window title, and a keyboard layout:

  | Game | Dropdown | Window title |
  |---|---|---|
  | MR1 DX | "MR1 DX" | `MF` |
  | MR2 DX | "MR2 DX" | `MF2` |

  Both run at the same seven 16:9 sizes, and both draw a 16:9 picture between black bars in fullscreen, so placing the picture and scaling keys stays exactly as it is.

- **Keyboard layouts, one per game.** The keyboard layout module now holds one layout per game instead of one fixed layout. Each layout answers the same questions as today: does a character have a key, where is a key's center in the 1600×900 reference picture, where are BS and Ent., and which stand-ins apply. The typist is given the selected game's layout, and the game port is given its window title.

- **MR1 DX's layout**, measured from `docs/screenshots/mr1/ingamekeyboard.png`. Like the MR2 screenshot, it's 1602×927: a 1600×900 client area plus the window border and title bar, so client coordinates are screenshot coordinates minus (1, 26). Measuring MR2's screenshot the same way matches its known positions within 2 px.
  - Two blocks of 5 columns × **9** rows. Columns are 75 apart, starting at x 278 (left block) and 728 (right block). Rows are 60 apart, starting at y 236.
  - Rows, with — for an empty cell:

    | Row | Left block | Right block |
    |---|---|---|
    | 0 | `A B C D E` | `a b c d e` |
    | 1 | `F G H I J` | `f g h i j` |
    | 2 | `K L M N O` | `k l m n o` |
    | 3 | `P Q R S T` | `p q r s t` |
    | 4 | `U V W X Y` | `u v w x y` |
    | 5 | `Z — — — —` | `z _ — — ←` |
    | 6 | `○ × ! ? ~` | `/ — — — —` |
    | 7 | `+ - % . ·` | `— — — — —` |
    | 8 | `1 2 3 4 5` | `6 7 8 9 0` |

  - **BS** is the `←` key at row 5, column 4 of the right block (x 1028, y 536): a grid key, unlike MR2's BS.
  - **Ent.** is the `END` key at x 1242, y 710 (measured). It's in the layout for a later ticket, but nothing clicks it.
  - The empty cells type a space in the game, but they're left out, so MR1 DX has no space key, the same as MR2 DX. `_` is a real underscore.
  - MR1 DX has no `△ □ ( ) ' " : =`. It has `_` and `/`, which MR2 DX doesn't.

- **The middle dot.** It's `·` (U+00B7) in both layouts: MR2 DX's left block row 6, column 3, the key left out as unknown until now, and MR1 DX's left block row 7, column 4. Neither layout has an unknown key any more.

- **Stand-ins, per game.** Each layout lists the stand-ins whose symbol it has:

  | Stand-in | Becomes | MR1 DX | MR2 DX |
  |---|---|---|---|
  | `@` | ○ | yes | yes |
  | `*` | × | yes | yes |
  | `^` | △ | no | yes |
  | `#` | □ | no | yes |
  | `` ` `` | · | yes | yes |
  | `・` (U+30FB) | · | yes | yes |

  In MR1 DX, `^` and `#` aren't swapped. They have no key, so the box drops them with the usual note. The backtick is a small raised mark, has no key in either game, and is the only free character that looks at all like a raised dot. The free Shift + number keys (`$`, `&`) don't look like one, and a comma sits low. The `・` stand-in lets names pasted from Japanese text work. The tooltip lists the selected game's stand-ins, as today, except `・`: it isn't on an ordinary keyboard, so listing it wouldn't help.

- **Main window.**
  - **Game dropdown:** a drop-down list (no typing) at the bottom of the window next to the typing rate, with "MR1 DX" and "MR2 DX". It defaults to MR2 DX, and Go disables it with the other inputs while typing.
  - **Switching games** clears the search text box completely, which also clears the dropped-characters note. The status line is left alone.
  - The search text box cleans text against the selected game's layout: stand-ins, dropping and the 12-character limit, as today.
  - **Game-not-found message:** it names the selected game: "MR1 DX not found. Start it, then press Go again." (or "MR2 DX ..."). It doesn't look for the other game.
- **Saved settings:** the game is saved in the user settings next to the typing rate, once, on close, and restored at startup. A missing or unknown saved value means MR2 DX.
- **Both monster fields hold 12 characters** (MR1 DX's results list shows a 12-character name), so the box's 12-character limit and the 12 BS clicks to clear stay the same for both games.

## Testing Decisions

- As in the main-window spec, tests drive the typist through its one operation with the fake game port, and check only what the game would see: which points were clicked, in what order and when, and the outcome.
- What the new typist tests cover, with MR1 DX's layout:
  - At 1600×900, twelve `←` clicks at (1028, 536), then each key at its measured position. For example, "Mocchi" lands at M (428, 356), o (1028, 356), c (878, 236), c, h (878, 296), i (953, 296).
  - At 854×480 and at 3440×1441 fullscreen, positions scale and shift from the reference the same way as MR2 DX's.
  - END is never clicked.
- Keyboard layout tests:
  - MR1 DX has keys for letters, digits, `○ × _ / · ~ % ! ?`, and none for `△ □ ( ) ' " : =`, space, or accented letters.
  - MR2 DX now has a key for `·`.
  - Each game's stand-ins map to a symbol key that game has, and no stand-in has a key itself.
- The existing MR2 DX tests keep passing unchanged, apart from the old "the unknown punctuation key has no key" case, which becomes the `·` case above.
- The main window isn't tested automatically. Check it by hand:
  - **MR1 DX:** windowed at 1600×900, one other size, and fullscreen once. Across a few search texts, type every MR1 DX key at least once. For example `ABCDEFGHIJKL`, `MNOPQRSTUVWX`, `YZabcdefghij`, `klmnopqrstuv`, `wxyz_/@*~!?+`, `` -%.`12345678 `` and `90`.
  - **MR2 DX:** the `` ` `` stand-in typing `·` into the game.
  - **The dropdown:** remembered across a restart, clearing the box on a switch, and disabled while typing.
  - **Messages and tooltip:** the not-found message names the game, and the tooltip changes with the game.
- No throwaway test script: the tool itself is the check, since it already types into MR2 DX.

## Out of Scope

- The empty cells on MR1 DX's keyboard (they type a space).
- Detecting which game is running, or hinting that the other game is open when the selected one isn't found.
- Everything left out of the main-window spec: the global hotkey, the preset list, clicking Ent., typing full names, calibration, and the release.

## Further Notes

- MR1 DX's flow from the main menu to a battle is in `docs/screenshots/mr1/1-mainmenu.png` to `9-beginbattle.png`, matching MR2 DX's set in `docs/screenshots/mr2/`. The tool only acts on the keyboard screen, so the other screens' differences don't matter to it.
- `GLOSSARY.md` was updated while this was decided: **Game**, MR1 DX's window title `MF`, `←` and `END` as MR1 DX's BS and Ent., and a keyboard per game.
