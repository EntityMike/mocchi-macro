# 01: Fixed-size main window

Status: resolved

Parent: [spec](../spec.md)

## What to build

Stop the organizer from resizing or maximizing the tool's window. Its controls sit at fixed places and don't stretch, so a larger window only adds empty space, and a smaller one cuts controls off. (It was also meant to keep ticket 03's rescaling simple, but that ticket was dropped.)

- **Fixed border:** set the main window's `FormBorderStyle` to `FixedSingle`, in the designer, so its edges and corners can't be dragged. The size grip goes with it.
- **No maximize:** set `MaximizeBox` to false. This also stops maximizing by double-clicking the title bar, by Win + ↑, and from the window menu.
- **Minimize stays.** It doesn't change the window's layout, and the organizer may want the tool out of the way between battles.
- Snapping to a screen edge (Win + ← / →) or to a snap layout doesn't resize a window with a fixed border either.

## Acceptance criteria

- [x] Dragging any edge or corner of the window doesn't resize it, and the cursor doesn't change to a resize arrow there.
- [x] The title bar shows minimize and close, with maximize missing or grayed out.
- [x] Double-clicking the title bar, Win + ↑, and the window menu (Alt + Space) don't maximize it.
- [x] Win + ← / → and dragging it to a screen edge don't resize it.
- [x] Minimizing and restoring still work, and the window comes back at the same size and place.
- [x] Everything else in the window works as before.

## User stories covered

6
