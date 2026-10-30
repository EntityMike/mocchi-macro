// Typist tests: they drive the typist through a fake game port and check only what the game
// would see, which screen points were clicked, in what order and when.

using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class TypistTests
    {
        [TestMethod]
        public void ClearsWithTwelveBackspacesThenClicksEachKeyAtItsMeasuredPosition()
        {
            // The client area sits at 100,50 on screen, so every expected point is the key's
            // measured position in the 1600x900 screenshot plus that offset.
            var game = new FakeGamePort { ClientArea = new Rectangle(100, 50, 1600, 900) };

            TypingOutcome outcome = new Typist(game).Type("Mocchi", 10);

            Point backspace = new Point(1297, 721);
            Point[] expected = Enumerable.Repeat(backspace, 12).Concat(new[]
            {
                new Point(520, 441),  // M
                new Point(1135, 441), // o
                new Point(985, 330),  // c
                new Point(985, 330),  // c
                new Point(985, 385),  // h
                new Point(1060, 385), // i
            }).ToArray();
            Assert.AreEqual(TypingOutcome.Done, outcome);
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void AtTenKeysPerSecondClickNHappensAtNTimes100Ms()
        {
            var game = new FakeGamePort();

            new Typist(game).Type("Mocchi", 10);

            // 12 BS clicks plus 6 keys, each 100 ms after the one before.
            long[] expected = Enumerable.Range(0, 18).Select(n => n * 100L).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.AtMs).ToArray());
        }

        [TestMethod]
        public void ReportsGameNotFoundAndClicksNothing()
        {
            var game = new FakeGamePort { GameRunning = false };

            TypingOutcome outcome = new Typist(game).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameNotFound, outcome);
            Assert.AreEqual(0, game.Clicks.Count);
        }
    }
}
