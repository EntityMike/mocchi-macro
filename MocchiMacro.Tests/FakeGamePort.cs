// A stand-in for the game used by the typist tests. It records every click with the time
// on a virtual clock, and Wait just moves that clock forward, so tests run instantly.

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

        // The game window's client area on screen. Defaults to the 1600x900 reference size.
        public Rectangle ClientArea = new Rectangle(0, 0, 1600, 900);

        public readonly List<RecordedClick> Clicks = new List<RecordedClick>();

        public long NowMs { get; private set; }

        public bool FindGame() => GameRunning;

        public Rectangle ReadClientArea() => ClientArea;

        public void Click(Point screenPoint) => Clicks.Add(new RecordedClick { Point = screenPoint, AtMs = NowMs });

        public void Wait(int ms) => NowMs += ms;
    }
}
