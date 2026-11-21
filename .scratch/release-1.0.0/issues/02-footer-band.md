# 02: Footer row and footer band

Status: resolved

Parent: [spec](../spec.md)

## What to build

**The art first.** A still strip of grass with a few small flowers, built from a small set of tiles, drawn as text pixel grids in one file in `art/` with the mascot's `palette.txt`:

- **The tiles:** about four, each 6 pixels tall and around 20 wide, such as plain grass, a tuft and two kinds of flower.
- **Their edges:** every tile shares the same left and right edge columns, so any tile can sit beside any other without a seam.
- **The order:** a line in the same file lists the tiles in a hand-picked order, such as `1 2 1 1 3 1 4 …`. It's long enough to fill the row at 200% scaling (840 px, about 14 tiles at 3×), and starts over if the row is wider.
- **Generating it:** `make-art.cs` turns each tile into a PNG in `art/png/` like the mascot frames (its list of grids gains the file).

The repo owner approves the art from previews before any window code uses it:

- the tiles, enlarged
- the band in its order at 3×, across 420, 525, 630 and 840 px (the window's width at 100%, 125%, 150% and 200%).

**Then the window.** It grows one row taller, about 18 px at 100%, across its full width under the mascot and the controls. The row shows the band:

- The tiles are embedded as resources like the mascot frames and enlarged 3× without smoothing, as `MascotArt` does.
- The row's image is built once at startup, tile after tile in the file's order, as wide as the row, so it fills the row at any display scaling.
- The band sits on the row's bottom edge. Above 100% the row grows with the window (about 27 px tall at 150%) while the band stays 18 px, and the grass belongs at the very bottom of the window, with no gap under it.
- The order lives in the grid file and in the code. Like the mascot's `play` lines, a test fails if the two differ.

The row is where ticket 03's link goes, in place of the band. This ticket only adds the band.

## Acceptance criteria

- [x] The repo owner approved the band's art from the previews.
- [x] Running `make-art.cs` again on unchanged grids gives byte-for-byte the same files.
- [x] The window is one row taller and still fixed-size, and the band fills the row's full width.
- [ ] At real 100%, 125% and 150% scaling, the band is crisp, sits on the window's bottom edge, its tiles meet without a visible seam, and nothing is cut off or overlapping.
- [x] The test comparing the band's order in the grid file and in the code passes, and fails if either one changes alone.
- [ ] Everything else in the window works as before: Go, Enter, the hotkey, the typing rate, the game dropdown and the mascot.
- [x] The existing automated tests pass unchanged.

## User stories covered

10

## Comments

**2026-11-20:** Built. The repo owner approved the band's art from the previews, after the blue flower's pale top petal was made the same blue as the others (white or pale blue vanishes against the window's grey). Running `make-art.cs` twice gave byte-for-byte the same files, and the mascot frames and icon came out unchanged. `make-art.cs` also refuses tiles whose edge columns differ from tile 1's. The order test fails when either the order line or the code's copy changes alone. At real 100% scaling, the band fills the row's width on the window's bottom edge, with no visible seam and nothing cut off or overlapping. Still to check by hand: 125% and 150% scaling, and Go, Enter, the hotkey, the typing rate, the game dropdown and the mascot in the new window. The automated tests pass unchanged.

**2026-11-20, closed by the repo owner:** Closed with two criteria unchecked: the band at real 125% and 150% scaling, and a hands-on pass of Go, Enter, the hotkey, the typing rate, the game dropdown and the mascot in the new window.
