// The real game port: finds the game window by its title and clicks it through user32.dll.
// The window calls follow the keyboard test that was proven against the game. The click has
// changed since: it waits for the cursor to settle before pressing, as its comments say.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace MocchiMacro
{
    public class User32GamePort : IGamePort
    {
        readonly string windowTitle;

        // Games often read the mouse once per frame (~16 ms at 60 fps). A down and up sent
        // back to back can both land between two frames, so the game never sees the press.
        const int ClickHoldMs = 50;

        // The game only takes a press on the key the cursor was already over in an earlier
        // frame, so the cursor rests on the key before the press. Without this, a press sent
        // right after the move was often lost in fullscreen, where frames are slower: 30 ms
        // fixed it at 3440x1441, and 50 leaves margin. It fits the 100 ms per key at the
        // highest typing rate, with the hold.
        const int CursorSettleMs = 50;

        readonly Stopwatch clock = Stopwatch.StartNew();
        IntPtr gameWindow;

        // The title of the game window to look for: "MF" for MR1 DX, "MF2" for MR2 DX.
        public User32GamePort(string windowTitle)
        {
            this.windowTitle = windowTitle;
        }

        public long NowMs => clock.ElapsedMilliseconds;

        public bool FindGame()
        {
            gameWindow = FindWindow(null, windowTitle);
            return gameWindow != IntPtr.Zero;
        }

        public bool GameStillExists() => IsWindow(gameWindow);

        public bool IsMinimized() => IsIconic(gameWindow);

        public void Restore() => ShowWindow(gameWindow, SW_RESTORE);

        public void BringToFront() => SetForegroundWindow(gameWindow);

        public bool IsInFront() => GetForegroundWindow() == gameWindow;

        public Rectangle ReadClientArea()
        {
            GetClientRect(gameWindow, out RECT client);
            var topLeft = new POINT();
            ClientToScreen(gameWindow, ref topLeft);
            return new Rectangle(topLeft.X, topLeft.Y, client.Right, client.Bottom);
        }

        public void Click(Point screenPoint)
        {
            SetCursorPos(screenPoint.X, screenPoint.Y);
            Thread.Sleep(CursorSettleMs);

            // The typist checked the game just before this click, but it can close or lose the
            // front during the settle, and the press would then land on the window behind it.
            // Skipped, the typist's next check stops typing.
            if (!GameStillExists() || !IsInFront())
            {
                return;
            }

            SendMouseButton(MOUSEEVENTF_LEFTDOWN);
            Thread.Sleep(ClickHoldMs);
            SendMouseButton(MOUSEEVENTF_LEFTUP);
        }

        public void Wait(int ms) => Thread.Sleep(ms);

        static void SendMouseButton(uint flags)
        {
            INPUT[] input = { new INPUT { type = INPUT_MOUSE, mi = new MOUSEINPUT { dwFlags = flags } } };
            SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
        }

        // ---- Win32 interop (user32.dll) ----

        [StructLayout(LayoutKind.Sequential)]
        struct POINT { public int X, Y; }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT { public int dx, dy; public uint mouseData, dwFlags, time; public IntPtr dwExtraInfo; }

        // INPUT is really a union of mouse, keyboard, and hardware input. Mouse is the largest
        // member, so declaring only it still gives the right struct size on 32- and 64-bit.
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT { public uint type; public MOUSEINPUT mi; }

        const uint INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const int SW_RESTORE = 9;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string className, string title);
        [DllImport("user32.dll")] static extern bool IsWindow(IntPtr hwnd);
        [DllImport("user32.dll")] static extern bool IsIconic(IntPtr hwnd);
        [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hwnd, int command);
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] static extern bool GetClientRect(IntPtr hwnd, out RECT rect);
        [DllImport("user32.dll")] static extern bool ClientToScreen(IntPtr hwnd, ref POINT point);
        [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")] static extern uint SendInput(uint count, INPUT[] inputs, int size);
    }
}
