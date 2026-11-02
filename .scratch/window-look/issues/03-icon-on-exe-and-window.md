# 03: The icon on the .exe and the window

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make `Mocchi.ico` from ticket 01 both:

- the project's application icon, so the .exe shows it in Explorer and on the Desktop
- the main window's icon, embedded in the .exe and not loaded from a file next to it, so the title bar, taskbar and Alt+Tab show it.

## Acceptance criteria

- [ ] The Release `MocchiMacro.exe` shows the Mocchi face in Explorer at small, medium, large and extra large icon sizes, and as a Desktop shortcut.
- [ ] The running tool shows it in the title bar, the taskbar and Alt+Tab.
- [ ] The build output is still the single `MocchiMacro.exe`, with no `.ico` beside it.

## User stories covered

3
