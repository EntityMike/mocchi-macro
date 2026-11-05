// A window that passes for the game: it has the game window's title and client size, and shows a
// screenshot of that game's keyboard, so a person can watch the cursor land on each key. It marks
// and records where each click lands, and does nothing else with it.
// It stays on top of everything, so the typist's clicks can't reach another program.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MocchiMacro.LiveCheck
{
    sealed class FakeGameWindow : Form
    {
        // The screenshots are 1602x927, the client area plus the window's border and title bar. The
        // key positions were measured from this part, so it's what shows at the window's own size.
        static readonly Rectangle ClientAreaInScreenshot =
            new Rectangle(1, 26, KeyboardLayout.ReferenceWidth, KeyboardLayout.ReferenceHeight);

        const int MarkRadius = 12;
        const int ScreenMargin = 12;

        readonly Bitmap screenshot;
        readonly List<Point> clicks = new List<Point>();

        FakeGameWindow(Game game)
        {
            screenshot = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshots", game.WindowTitle + ".png"));
            Text = game.WindowTitle;
            ClientSize = ClientAreaInScreenshot.Size;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            TopMost = true;
            DoubleBuffered = true;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(screen.Right - Width - ScreenMargin, screen.Top + (screen.Height - Height) / 2);
        }

        // Opens on its own thread, since a window needs a message loop running to take clicks. It's
        // brought to the front with a real click, because a program in the background can't just
        // take the front, and the tool sits to its left where it can be seen.
        public static FakeGameWindow Open(Game game, ToolWindow tool)
        {
            FakeGameWindow window = null;
            using (var shown = new ManualResetEventSlim())
            {
                var messageLoop = new Thread(() =>
                {
                    window = new FakeGameWindow(game);
                    window.Shown += (sender, e) => shown.Set();
                    Application.Run(window);
                });
                messageLoop.SetApartmentState(ApartmentState.STA);
                messageLoop.IsBackground = true;
                messageLoop.Start();
                shown.Wait();
            }

            Rectangle bounds = window.Bounds;
            tool.MoveTo(Math.Max(Screen.PrimaryScreen.WorkingArea.Left, bounds.Left - tool.WindowSize.Width - ScreenMargin), bounds.Top);

            ClickAt(new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2));
            IntPtr handle = (IntPtr)window.Invoke((Func<IntPtr>)(() => window.Handle));
            if (GetForegroundWindow() != handle)
                throw new InvalidOperationException("The fake game window didn't come to the front.");
            window.ClearClicks();
            return window;
        }

        public IReadOnlyList<Point> Clicks => (List<Point>)Invoke((Func<List<Point>>)(() => clicks.ToList()));

        public void ClearClicks() => Invoke((Action)(() =>
        {
            clicks.Clear();
            Invalidate();
        }));

        public void SaveMarkedPicture(string path) => Invoke((Action)(() =>
        {
            using (var picture = new Bitmap(ClientSize.Width, ClientSize.Height))
            {
                using (Graphics graphics = Graphics.FromImage(picture)) Draw(graphics);
                picture.Save(path);
            }
        }));

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            clicks.Add(e.Location);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw(e.Graphics);
        }

        void Draw(Graphics graphics)
        {
            graphics.DrawImage(screenshot, ClientRectangle, ClientAreaInScreenshot, GraphicsUnit.Pixel);
            using (var mark = new Pen(Color.Red, 3))
            {
                foreach (Point click in clicks)
                    graphics.DrawEllipse(mark, click.X - MarkRadius, click.Y - MarkRadius, MarkRadius * 2, MarkRadius * 2);
            }
        }

        // Disposing comes from the check's thread, but a window can only be closed from its own.
        // Closing there also disposes it, which comes back here, and ends its message loop. So this
        // returns with the window gone and the game's title free for the next one.
        protected override void Dispose(bool disposing)
        {
            if (disposing && InvokeRequired)
            {
                Invoke((Action)Close);
                return;
            }
            if (disposing) screenshot.Dispose();
            base.Dispose(disposing);
        }

        static void ClickAt(Point screenPoint)
        {
            SetCursorPos(screenPoint.X, screenPoint.Y);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(300);
        }

        // ---- Win32 interop (user32.dll) ----

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002, MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")] static extern void mouse_event(uint flags, int x, int y, uint data, IntPtr extraInfo);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    }
}
