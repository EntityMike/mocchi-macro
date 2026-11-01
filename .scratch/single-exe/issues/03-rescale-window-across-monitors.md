# 03: Rescale the tool window across monitors

Status: ready-for-agent

Blocked by: 01, 02

Parent: [spec](../spec.md)

## What to build

Put back what the config file did for the tool's own window: keep it the right size when it sits on a monitor whose scaling differs from the main one. WinForms only does this itself when the config file turns it on, so the main window does it.

- **On Windows' "DPI changed" message** (`WM_DPICHANGED`, 0x02E0), handled in the main window's `WndProc`:
  - The new DPI is the low word of `wParam`.
  - Set the window's font to the designer's font size × new DPI ÷ system DPI, keeping its family and style. With `AutoScaleMode.Font`, this makes WinForms rescale every control.
  - Then move the window to the rectangle `lParam` points to, and return 0.
  - Don't pass the message on to the base `WndProc`.
- **At startup,** once the window has a handle: if the DPI of its monitor (`GetDpiForWindow`) differs from the system DPI (`GetDpiForSystem`), set the font the same way. No rectangle to apply here.
- **Always work from the designer's font size and the system DPI,** never from the current font, so moving back and forth doesn't drift. The system DPI is what WinForms scaled the window for at startup, which is why it's the base.
- Comment the handler: WinForms 4.8 does this itself only when `MocchiMacro.exe.config` enables it, and the tool ships without one (see the spec).

## Acceptance criteria

- [ ] With two monitors at different scaling (for example 100% and 150%), dragging the tool's window from one to the other resizes it so it looks the same size on both. Its text stays sharp, and no controls are clipped or overlapping.
- [ ] After dragging it back and forth five times, it's the same size each time it returns to a monitor.
- [ ] Started on the monitor that isn't the main one, at different scaling from the main one, the window opens at the right size there.
- [ ] With both monitors at the same scaling, the window looks and behaves as before.
- [ ] Typing still works after the window has been dragged between monitors.

## User stories covered

3, 4
