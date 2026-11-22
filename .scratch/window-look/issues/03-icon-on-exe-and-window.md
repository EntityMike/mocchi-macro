# 03: The icon on the .exe and the window

Status: resolved

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make `Mocchi.ico` from ticket 01 both:

- the project's application icon, so the .exe shows it in Explorer and on the Desktop
- the main window's icon, embedded in the .exe and not loaded from a file next to it, so the title bar, taskbar, and Alt+Tab show it.

## Acceptance criteria

- [x] The Release `MocchiMacro.exe` shows the Mocchi face in Explorer at small, medium, large, and extra large icon sizes, and as a Desktop shortcut.
- [x] The running tool shows it in the title bar, the taskbar, and Alt+Tab.
- [x] The build output is still the single `MocchiMacro.exe`, with no `.ico` beside it.

## User stories covered

3

## Comments

**2026-11-05, closed by the repo owner:** The owner did the hand checks and the display scaling checks, and they looked good.

The spec's Out of Scope said Windows shrinks the 32 px icon for the title bar at 125% and 150%. In review, asking the icon for 20 or 24 px returned the 16 px face, so that face is probably what gets enlarged there instead, and the spec now says so. No sizes were added, as the title bar looked fine to the owner.
