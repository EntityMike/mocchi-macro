// A stand-in for the game used by the typist tests. It records every click with the time
// on a virtual clock, and Wait just moves that clock forward, so tests run instantly. Tests can
// change the game partway through (for example move the window) from AfterClick.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace MocchiMacro.Tests
{
    public struct RecordedClick
    {
        public Point Point;
        public long AtMs;
    }

    public class FakeGamePort : IGamePort
    {
        public bool GameRunning = true;

        // While minimized, the game reports what Windows does for a minimized window: an
        // empty client area parked at -32000,-32000.
        public bool Minimized;

        // How long after being asked the game really is in front; null for never.
        public int? ComesToFrontAfterMs = 0;
        long? askedToFrontAtMs;

        // The game window's client area on screen. Defaults to the 1600x900 reference size.
        public Rectangle ClientArea = new Rectangle(0, 0, 1600, 900);

        public readonly List<RecordedClick> Clicks = new List<RecordedClick>();

        // Runs after each click is recorded, so a test can change the game partway through.
        public Action AfterClick = () => { };

        // Runs after each wait, so a test can change the game while the typist waits.
        public Action AfterWait = () => { };

        public long NowMs { get; private set; }

        public bool FindGame() => GameRunning;

        public bool GameStillExists() => GameRunning;

        public bool IsMinimized() => Minimized;

        public void Restore() => Minimized = false;

        public void BringToFront() => askedToFrontAtMs = NowMs;

        public bool IsInFront() => askedToFrontAtMs.HasValue && ComesToFrontAfterMs.HasValue
            && NowMs >= askedToFrontAtMs + ComesToFrontAfterMs;

        public Rectangle ReadClientArea() => Minimized ? new Rectangle(-32000, -32000, 0, 0) : ClientArea;

        public void Click(Point screenPoint)
        {
            Clicks.Add(new RecordedClick { Point = screenPoint, AtMs = NowMs });
            AfterClick();
        }

        public void Wait(int ms)
        {
            NowMs += ms;
            AfterWait();
        }
    }
}
