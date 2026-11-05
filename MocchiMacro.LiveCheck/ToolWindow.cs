// The real tool, running as its own process and driven from outside through window messages, the
// way a person's clicks and typing would reach it.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MocchiMacro.LiveCheck
{
    sealed class ToolWindow : IDisposable
    {
        const int MascotSize = 96;

        readonly Process process;
        readonly IntPtr window;
        IntPtr mascot, goButton, searchBox, typingRateBox, statusLabel, gameBox;

        ToolWindow(Process process, IntPtr window)
        {
            this.process = process;
            this.window = window;
            FindControls();
        }

        // This runs the copy of the .exe next to this program. Windows keeps a program's settings
        // by its path, so whatever this check sets can't reach the settings of a copy in real use.
        public static ToolWindow Launch()
        {
            Process process = Process.Start(typeof(Game).Assembly.Location);
            process.WaitForInputIdle();
            while (process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(50);
                process.Refresh();
            }
            return new ToolWindow(process, process.MainWindowHandle);
        }

        // WinForms names its window classes after what's in them only loosely, but these parts hold:
        // the mascot is the one control at the mascot's size, and the status line is the tallest label.
        void FindControls()
        {
            var editBoxes = new List<IntPtr>();
            var labels = new List<IntPtr>();
            EnumChildWindows(window, (child, unused) =>
            {
                string className = ClassNameOf(child).ToUpperInvariant();
                Rectangle bounds = BoundsOf(child);
                if (className.Contains("BUTTON") && TextOf(child) == "Go") goButton = child;
                if (className.Contains("EDIT")) editBoxes.Add(child);
                if (className.Contains("COMBOBOX")) gameBox = child;
                if (className.Contains("STATIC")) labels.Add(child);
                if (bounds.Width == MascotSize && bounds.Height == MascotSize) mascot = child;
                return true;
            }, IntPtr.Zero);

            if (mascot == IntPtr.Zero)
                throw new InvalidOperationException("There's no control at the mascot's fixed size of " + MascotSize + " px.");

            // The search text box is much wider than the typing rate box's edit part.
            searchBox = editBoxes.OrderByDescending(box => BoundsOf(box).Width).First();
            typingRateBox = editBoxes.OrderByDescending(box => BoundsOf(box).Width).Last();
            statusLabel = labels.OrderByDescending(label => BoundsOf(label).Height).First();
        }

        public uint Dpi => GetDpiForWindow(window);
        public string Status => TextOf(statusLabel);
        public bool GoEnabled => IsWindowEnabled(goButton);
        public Size WindowSize => BoundsOf(window).Size;

        // Posted, not sent: Go's handler returns only once its work has started, and a sent message
        // would keep this program waiting on it.
        public void PressGo() => PostMessage(goButton, BM_CLICK, IntPtr.Zero, IntPtr.Zero);

        public void WaitForGoDisabled()
        {
            var clock = Stopwatch.StartNew();
            while (GoEnabled && clock.ElapsedMilliseconds < 2000) Thread.Sleep(1);
        }

        public void WaitForGoEnabled()
        {
            var clock = Stopwatch.StartNew();
            while (!GoEnabled && clock.ElapsedMilliseconds < 60000) Thread.Sleep(10);
        }

        public void SetSearchText(string text) => SendMessage(searchBox, WM_SETTEXT, IntPtr.Zero, text);

        public void SetTypingRate(int keysPerSecond) => SendMessage(typingRateBox, WM_SETTEXT, IntPtr.Zero, keysPerSecond.ToString());

        // Setting the selection from outside raises no event, so the box's owner is told as a real
        // pick would tell it. The dropdown lists the games in Game.All's order.
        public void SelectGame(Game game)
        {
            int index = Game.All.ToList().IndexOf(game);
            SendMessage(gameBox, CB_SETCURSEL, (IntPtr)index, IntPtr.Zero);
            IntPtr selectionChanged = (IntPtr)((CBN_SELCHANGE << 16) | GetDlgCtrlID(gameBox));
            SendMessage(GetParent(gameBox), WM_COMMAND, selectionChanged, gameBox);
        }

        public void MoveTo(int left, int top) => SetWindowPos(window, IntPtr.Zero, left, top, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);

        // A round trip through the window's message loop, which stalls when the window would.
        public long MeasureReplyMs()
        {
            var clock = Stopwatch.StartNew();
            SendMessageTimeout(window, WM_NULL, IntPtr.Zero, IntPtr.Zero, 0, 5000, out IntPtr unused);
            return clock.ElapsedMilliseconds;
        }

        // Copied out of the window even when something covers it, as the fake game window can.
        Bitmap CaptureWindow()
        {
            Rectangle bounds = BoundsOf(window);
            var picture = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(picture))
            {
                IntPtr deviceContext = graphics.GetHdc();
                PrintWindow(window, deviceContext, PW_RENDERFULLCONTENT);
                graphics.ReleaseHdc(deviceContext);
            }
            return picture;
        }

        public Bitmap CaptureMascot()
        {
            Rectangle windowBounds = BoundsOf(window), mascotBounds = BoundsOf(mascot);
            mascotBounds.Offset(-windowBounds.Left, -windowBounds.Top);
            using (Bitmap whole = CaptureWindow())
                return whole.Clone(mascotBounds, PixelFormat.Format32bppArgb);
        }

        public void SaveScreenshot(string path)
        {
            using (Bitmap whole = CaptureWindow()) whole.Save(path, ImageFormat.Png);
        }

        // Killed, not closed, so the tool never gets as far as saving this run's settings.
        public void Dispose()
        {
            if (!process.HasExited) process.Kill();
            process.Dispose();
        }

        static string ClassNameOf(IntPtr control)
        {
            var name = new StringBuilder(256);
            GetClassName(control, name, name.Capacity);
            return name.ToString();
        }

        // Asked by message, since GetWindowText can't read another program's controls.
        static string TextOf(IntPtr control)
        {
            var text = new StringBuilder(512);
            SendMessage(control, WM_GETTEXT, (IntPtr)text.Capacity, text);
            return text.ToString();
        }

        static Rectangle BoundsOf(IntPtr control)
        {
            GetWindowRect(control, out RECT rect);
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        // ---- Win32 interop (user32.dll) ----

        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int Left, Top, Right, Bottom; }

        delegate bool EnumWindowsCallback(IntPtr control, IntPtr parameter);

        const int WM_NULL = 0x0000, WM_SETTEXT = 0x000C, WM_GETTEXT = 0x000D, WM_COMMAND = 0x0111;
        const int BM_CLICK = 0x00F5, CB_SETCURSEL = 0x014E, CBN_SELCHANGE = 1;
        const uint PW_RENDERFULLCONTENT = 2;
        const uint SWP_NOSIZE = 0x0001, SWP_NOZORDER = 0x0004, SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")] static extern bool EnumChildWindows(IntPtr parent, EnumWindowsCallback callback, IntPtr parameter);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetClassName(IntPtr control, StringBuilder name, int capacity);
        [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr control, out RECT rect);
        [DllImport("user32.dll")] static extern uint GetDpiForWindow(IntPtr window);
        [DllImport("user32.dll")] static extern bool IsWindowEnabled(IntPtr control);
        [DllImport("user32.dll")] static extern IntPtr GetParent(IntPtr control);
        [DllImport("user32.dll")] static extern int GetDlgCtrlID(IntPtr control);
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr control, int message, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr control, int message, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr SendMessage(IntPtr control, int message, IntPtr wParam, string lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr SendMessage(IntPtr control, int message, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll")] static extern IntPtr SendMessageTimeout(IntPtr control, int message, IntPtr wParam, IntPtr lParam, uint flags, uint timeoutMs, out IntPtr result);
        [DllImport("user32.dll")] static extern bool SetWindowPos(IntPtr window, IntPtr insertAfter, int left, int top, int width, int height, uint flags);
        [DllImport("user32.dll")] static extern bool PrintWindow(IntPtr window, IntPtr deviceContext, uint flags);
    }
}
