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
        public void At854x480EachKeyLandsAtItsReferencePositionScaledToThatSize()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 854, 480) };

            new Typist(game).Type("Mocchi", 10);

            // Reference x times 854/1600 and y times 480/900, fractions cut off.
            Point[] expected = Enumerable.Repeat(new Point(638, 358), 12).Concat(new[]
            {
                new Point(224, 208), // M
                new Point(552, 208), // o
                new Point(472, 149), // c
                new Point(472, 149), // c
                new Point(472, 179), // h
                new Point(512, 179), // i
            }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void At1920x1080EachKeyLandsAtItsReferencePositionScaledToThatSize()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 1920, 1080) };

            new Typist(game).Type("Mocchi", 10);

            // Reference position times 1.2, fractions cut off.
            Point[] expected = Enumerable.Repeat(new Point(1436, 805), 12).Concat(new[]
            {
                new Point(504, 470),  // M
                new Point(1242, 470), // o
                new Point(1062, 336), // c
                new Point(1062, 336), // c
                new Point(1062, 403), // h
                new Point(1152, 403), // i
            }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void InFullscreenAt3440x1441KeysLandInsideA2562x1441PictureBetween439PxBars()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 3440, 1441) };

            new Typist(game).Type("Mocchi", 10);

            // 439 plus reference x times 2562/1600, and reference y times 1441/900,
            // fractions cut off.
            Point[] expected = Enumerable.Repeat(new Point(2355, 1074), 12).Concat(new[]
            {
                new Point(1111, 627), // M
                new Point(2096, 627), // o
                new Point(1856, 448), // c
                new Point(1856, 448), // c
                new Point(1856, 537), // h
                new Point(1976, 537), // i
            }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void InFullscreenAt1920x1200KeysLandInsideA1920x1080PictureBetween60PxBars()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 1920, 1200) };

            new Typist(game).Type("Mocchi", 10);

            // The 1920x1080 positions, moved down past the 60 px bar on top.
            Point[] expected = Enumerable.Repeat(new Point(1436, 865), 12).Concat(new[]
            {
                new Point(504, 530),  // M
                new Point(1242, 530), // o
                new Point(1062, 396), // c
                new Point(1062, 396), // c
                new Point(1062, 463), // h
                new Point(1152, 463), // i
            }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void WhenTheClientAreaMovesPartwayThroughLaterClicksFollowIt()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(100, 50, 1600, 900) };
            // After the 12 BS clicks and "Moc", the window is dragged to 300,200.
            game.AfterClick = () =>
            {
                if (game.Clicks.Count == 15) game.ClientArea = new Rectangle(300, 200, 1600, 900);
            };

            new Typist(game).Type("Mocchi", 10);

            Point[] expected = Enumerable.Repeat(new Point(1297, 721), 12).Concat(new[]
            {
                new Point(520, 441),  // M
                new Point(1135, 441), // o
                new Point(985, 330),  // c
                new Point(1185, 480), // c, now offset by 300,200
                new Point(1185, 535), // h
                new Point(1260, 535), // i
            }).ToArray();
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
