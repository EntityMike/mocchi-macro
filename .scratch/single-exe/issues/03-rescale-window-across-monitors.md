# 03: Rescale the tool window across monitors

Status: wontfix

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

## Comments

Not doing this. It was implemented as written above, and the user tested it by hand on two monitors: it didn't work well as written. The code was discarded; the notes below are what was learned, for if it's ever picked up again.

### What the implementation looked like

All in `MainWindow.cs`, with `designerFont = Font` saved in the constructor right after `InitializeComponent()`:

```csharp
protected override void OnHandleCreated(EventArgs e)
{
    base.OnHandleCreated(e);
    int dpi = GetDpiForWindow(Handle);
    if (dpi != GetDpiForSystem()) ScaleFontTo(dpi);
}

protected override void WndProc(ref Message m)
{
    if (m.Msg == WM_DPICHANGED) // 0x02E0
    {
        ScaleFontTo((int)(m.WParam.ToInt64() & 0xFFFF));
        RECT suggested = Marshal.PtrToStructure<RECT>(m.LParam);
        Bounds = Rectangle.FromLTRB(suggested.Left, suggested.Top, suggested.Right, suggested.Bottom);
        m.Result = IntPtr.Zero;
        return;
    }
    base.WndProc(ref m);
}

void ScaleFontTo(int dpi)
{
    float size = designerFont.Size * dpi / GetDpiForSystem();
    Font = new Font(designerFont.FontFamily, size, designerFont.Style, designerFont.Unit);
}

[DllImport("user32.dll")] static extern int GetDpiForWindow(IntPtr hwnd);
[DllImport("user32.dll")] static extern int GetDpiForSystem();
```

### What was checked, and how

The development machine's two monitors share one scaling, so the handler was checked by a throwaway PowerShell probe instead of a real drag. The probe loaded `MocchiMacro.exe` into a PerMonitorV2 process, showed `MainWindow`, and sent it `WM_DPICHANGED` itself. The startup path wasn't exercised that way.

- **No drift at 150%.** Five round trips between 96 and 144 DPI brought the window and every control back to exactly the same bounds each time. Each step was a clean ×1.5, from 6×13 to 9×20 font metrics.
- **Clipping at the other common scalings.** This is the main flaw. With `AutoScaleMode.Font`, WinForms resizes the controls by how much the font's measured character size grew. That size is whole pixels, so the factor isn't the DPI ratio. Windows' suggested rectangle, meanwhile, grows the window by exactly the DPI ratio. Measured at 96 DPI system scaling (client width at the designer's size is 300, and the controls end at x = 288):

  | Scaling | Font metrics | Controls end at | Window's inside width (suggested) |
  |---|---|---|---|
  | 125% | 8×17 | 384 | 375, clips 9 px |
  | 150% | 9×20 | 432 | 450, fits |
  | 175% | 11×24 | 528 | 525, clips 3 px |
  | 200% | 13×26 | 624 | 600, clips 24 px |

  Under the font-based size alone (no suggested rectangle), the window grows with the controls, at 400, 450, 550 and 650 wide inside.
- **The two paths disagree.** At startup the window's size came from the font rescale, but after a drag it came from Windows' rectangle. So opening on a monitor and dragging onto it could give different sizes.

### What would need rethinking

- **Size from the font, position from Windows.** Taking only the suggested rectangle's top-left and letting the font rescale set the size would fix the clipping and make both paths agree. The risk is that the window is then a few percent larger than Windows planned for. Right at the edge of a higher-scaling monitor on the left, the window could flip between scalings mid-drag. That's untested.
- **Order inside the handler.** The ticket says font first, then the rectangle. So the window first grows in place at its old top-left, while it still straddles the two monitors, and only then moves. That in-between resize could set off another DPI change near a monitor edge. WinForms' own handler does it the other way round: it moves to the suggested rectangle first, then rescales, with layout suspended.
- **Scaling controls by the exact DPI ratio** (`Scale(SizeF)`), as WinForms itself does when the config file turns it on, avoids the rounded-metrics mismatch, but each move rescales from the current sizes, so rounding can drift. The font approach was chosen to avoid that drift.

### Smaller things noticed

- `new Font(family, size, style, unit)` sets the GDI character set to the default (1) rather than the default font's. On Japanese Windows that can be Shift-JIS (128). Text likely looks the same, but on return to the system DPI the font no longer equals the designer's. The overload that also takes `GdiCharSet` and `GdiVerticalFont` keeps them.
- Each rescale makes a new `Font` without disposing the old one. It's a small GDI leak per monitor move. `designerFont` itself must never be disposed.
- `GetDpiForWindow` and `GetDpiForSystem` need Windows 10 version 1607 or later. On older Windows they throw at startup, where the config file approach just did nothing.
- Swallowing `WM_DPICHANGED` leaves `Form.DeviceDpi` at its startup value. Nothing reads it today.
- The search box's tooltip isn't covered by the font rescale.
- `GetDpiForSystem` is the right base: in a PerMonitorV2 process the screen DC reports the system DPI, which is what WinForms measured the font against at startup.
- Handle recreation is safe: rescaling always starts from `designerFont`, and setting an equal font does nothing.

### Learned since, from window-look ticket 02 (2026-11-05)

- The window now uses `AutoScaleMode.Dpi`, not `AutoScaleMode.Font`, and sets its font in the constructor, after `InitializeComponent`. The notes above describe the window as it was then, so anyone picking this up should re-measure.
- `Form.DeviceDpi` reports 96 at every scaling without the config file, not the DPI WinForms scaled for. Use `GetDpiForSystem` or `CurrentAutoScaleDimensions` instead.
