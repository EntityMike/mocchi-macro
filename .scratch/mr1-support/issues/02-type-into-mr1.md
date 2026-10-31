# 02: Pick the game and type into MR1 DX

Status: resolved

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

A game dropdown, and everything Go needs to type into MR1 DX as well as MR2 DX.

- **One keyboard layout per game.** The keyboard layout module holds an MR1 DX layout next to the MR2 DX one, each answering the same questions as today: does a character have a key, where is its center in the 1600×900 reference picture, where are BS and Ent., and which stand-ins apply. MR1 DX's layout is the table in the spec: 9 rows, columns 75 apart from x 278 and 728, rows 60 apart from y 236. BS is `←` at (1028, 536) and Ent. is `END` at (1242, 710). The empty cells are left out.
- **Stand-ins per game.** MR1 DX gets `@`→○, `*`→×, `` ` ``→· and `・`→·. MR2 DX keeps all six. In MR1 DX, `^` and `#` are characters without a key, so the box drops them.
- **Typist and game port per game.** The typist is given the selected game's layout: its "character without a key" check and its BS position come from that layout. The game port is given the game's window title: `MF` for MR1 DX, `MF2` for MR2 DX.
- **Game dropdown** (a drop-down list, no typing) at the bottom of the window next to the typing rate. It holds "MR1 DX" and "MR2 DX" and starts on MR2 DX. Go disables it along with the other inputs while typing.
- **The search text box and tooltip follow the selected game:** text is cleaned against its layout, and the tooltip lists its stand-ins (without `・`).
- **Game-not-found message** names the selected game: "MR1 DX not found. Start it, then press Go again." (or "MR2 DX ...").

## Acceptance criteria

- [x] With MR1 DX picked, windowed at 1600×900 on the keyboard screen, Go types each of these search texts into the monster field, replacing whatever was there: `ABCDEFGHIJKL`, `MNOPQRSTUVWX`, `YZabcdefghij`, `klmnopqrstuv`, `wxyz_/@*~!?+`, `` -%.`12345678 `` and `90`. Between them, every MR1 DX key gets typed.
- [x] The same for at least one text at one other window size, and once in fullscreen.
- [x] END is never clicked.
- [x] With MR1 DX picked, typing `(Tiger)△` in the box leaves `Tiger` with the note naming `(`, `)` and `△`. `^` and `#` are dropped as well.
- [x] The tooltip reads `` @ ○   * ×   ` · `` for MR1 DX and `` @ ○   * ×   ^ △   # □   ` · `` for MR2 DX.
- [x] With MR1 DX picked and only MR2 DX running (or neither), Go says "MR1 DX not found. Start it, then press Go again."
- [x] The dropdown is disabled while typing.
- [x] Everything that worked in MR2 DX still does.
- [x] Test: MR1 DX at 1600×900, client area at 0,0: twelve clicks at (1028, 536), then "Mocchi" at M (428, 356), o (1028, 356), c (878, 236), c (878, 236), h (878, 296), i (953, 296).
- [x] Test: MR1 DX at 854×480 and at 3440×1441 scale and shift positions from the reference, the same way as MR2 DX's.
- [x] Test: MR1 DX's layout has keys for letters, digits and `○ × _ / · ~ % ! ? + - .`, and none for `△ □ ( ) ' " : =`, space or accented letters.
- [x] Test: each game's stand-ins map to a key that game has, and no stand-in has a key itself.
- [x] The existing MR2 DX tests pass unchanged, apart from passing in MR2 DX's layout.

## User stories covered

1, 2, 3, 5, 7, 8, 9, 10, 11, 12 (MR1 DX)

## Comments

- From ticket 01: the tooltip keeps `・` out by listing only the ASCII stand-ins (`.Where(s => s.StandIn < 128)` in `MainWindow`), not by naming `・`. When the tooltip switches to the selected game's stand-ins, keep that filter so `・` stays out for both games.
