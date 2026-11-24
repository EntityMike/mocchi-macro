// Typist tests: they drive the typist through a fake game port and check only what the game
// would see, which screen points were clicked, in what order, and when.

using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class TypistTests
    {
        [TestMethod]
        public void ClearsWithTwelveBSClicksThenClicksEachKeyAtItsMeasuredPosition()
        {
            // The client area sits at 100,50 on screen, so every expected point is the key's
            // measured position in the 1600x900 screenshot plus that offset.
            var game = new FakeGamePort { ClientArea = new Rectangle(100, 50, 1600, 900) };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            var backspace = new Point(1297, 721);
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

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

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

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

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

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

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

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

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
        public void TypesTheMiddleDotAtItsKeyOnTheGrid()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 1600, 900) };

            new Typist(game, KeyboardLayout.Mr2Dx).Type("\u00B7", 10);

            // Left block row 6, column 3, at (495, 615.4) in the 1600x900 screenshot.
            Point[] expected = Enumerable.Repeat(new Point(1197, 671), 12)
                .Concat(new[] { new Point(495, 615) }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void InMr1DxClearsWithTwelveBSClicksThenClicksEachKeyAtItsMeasuredPosition()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 1600, 900) };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr1Dx).Type("Mocchi", 10);

            // MR1 DX's BS is the ← key on the grid, right block row 5, column 4, at its
            // position in docs/screenshots/mr1/ingamekeyboard.png minus the window border.
            Point[] expected = Enumerable.Repeat(new Point(1028, 536), 12).Concat(new[]
            {
                new Point(428, 356),  // M
                new Point(1028, 356), // o
                new Point(878, 236),  // c
                new Point(878, 236),  // c
                new Point(878, 296),  // h
                new Point(953, 296),  // i
            }).ToArray();
            Assert.AreEqual(TypingOutcome.Done, outcome);
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void InMr1DxAt854x480EachKeyLandsAtItsReferencePositionScaledToThatSize()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 854, 480) };

            new Typist(game, KeyboardLayout.Mr1Dx).Type("Mocchi", 10);

            // Reference x times 854/1600 and y times 480/900, fractions cut off.
            Point[] expected = Enumerable.Repeat(new Point(548, 285), 12).Concat(new[]
            {
                new Point(228, 189), // M
                new Point(548, 189), // o
                new Point(468, 125), // c
                new Point(468, 125), // c
                new Point(468, 157), // h
                new Point(508, 157), // i
            }).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void InMr1DxInFullscreenAt3440x1441KeysLandInsideA2562x1441PictureBetween439PxBars()
        {
            var game = new FakeGamePort { ClientArea = new Rectangle(0, 0, 3440, 1441) };

            new Typist(game, KeyboardLayout.Mr1Dx).Type("Mocchi", 10);

            // 439 plus reference x times 2562/1600, and reference y times 1441/900,
            // fractions cut off.
            Point[] expected = Enumerable.Repeat(new Point(2085, 858), 12).Concat(new[]
            {
                new Point(1124, 569), // M
                new Point(2085, 569), // o
                new Point(1844, 377), // c
                new Point(1844, 377), // c
                new Point(1844, 473), // h
                new Point(1964, 473), // i
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
                if (game.Clicks.Count == 15)
                {
                    game.ClientArea = new Rectangle(300, 200, 1600, 900);
                }
            };

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

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
        [DataRow(1, 1000)]
        [DataRow(4, 250)]
        [DataRow(10, 100)]
        public void EachClickIsOneIntervalAfterThePreviousAtTheTypingRate(int typingRate, int intervalMs)
        {
            var game = new FakeGamePort();

            new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", typingRate);

            // 12 BS clicks plus 6 keys, each one interval after the one before, counted from the
            // first click (which waits for the game to be in front).
            long firstMs = game.Clicks[0].AtMs;
            long[] expected = Enumerable.Range(0, 18).Select(n => (long)n * intervalMs).ToArray();
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.AtMs - firstMs).ToArray());
        }

        [TestMethod]
        public void WhenTheGameIsMinimizedItIsRestoredBeforeTheFirstClick()
        {
            var game = new FakeGamePort { Minimized = true };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("M", 10);

            // The restored 1600x900 positions: 12 BS, then M.
            Point[] expected = Enumerable.Repeat(new Point(1197, 671), 12)
                .Concat(new[] { new Point(420, 391) }).ToArray();
            Assert.AreEqual(TypingOutcome.Done, outcome);
            CollectionAssert.AreEqual(expected, game.Clicks.Select(c => c.Point).ToArray());
        }

        [TestMethod]
        public void TheFirstClickComesHalfASecondAfterTheGameIsInFront()
        {
            var game = new FakeGamePort { ComesToFrontAfterMs = 1200 };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.Done, outcome);
            Assert.AreEqual(1700, game.Clicks[0].AtMs);
        }

        [TestMethod]
        [DataRow(5100)]
        [DataRow(null)] // never
        public void ReportsTheGameNotInFrontWithinFiveSecondsAndClicksNothing(int? comesToFrontAfterMs)
        {
            var game = new FakeGamePort { ComesToFrontAfterMs = comesToFrontAfterMs };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameNotInFront, outcome);
            Assert.AreEqual(0, game.Clicks.Count);
        }

        [TestMethod]
        public void WhenTheGameClosesPartwayThroughItReportsSoAndNoClicksFollow()
        {
            var game = new FakeGamePort();
            game.AfterClick = () =>
            {
                if (game.Clicks.Count == 5)
                {
                    game.GameRunning = false;
                }
            };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameClosed, outcome);
            Assert.AreEqual(5, game.Clicks.Count);
        }

        [TestMethod]
        public void WhenTheGameLeavesTheFrontPartwayThroughItReportsSoAndNoClicksFollow()
        {
            var game = new FakeGamePort();
            game.AfterClick = () =>
            {
                if (game.Clicks.Count == 5)
                {
                    game.LeaveFront();
                }
            };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameLeftFront, outcome);
            Assert.AreEqual(5, game.Clicks.Count);
        }

        // The real port skips a press when the game has left the front or closed during the
        // click, so the last key may not have reached the game: that isn't Done.
        [TestMethod]
        [DataRow(true, TypingOutcome.GameClosed)]
        [DataRow(false, TypingOutcome.GameLeftFront)]
        public void WhenTheGameGoesDuringTheLastClickItIsNotReportedDone(bool closes, TypingOutcome expected)
        {
            var game = new FakeGamePort();
            game.AfterClick = () =>
            {
                if (game.Clicks.Count == 18)
                {
                    if (closes)
                    {
                        game.GameRunning = false;
                    }
                    else
                    {
                        game.LeaveFront();
                    }
                }
            };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(expected, outcome);
        }

        [TestMethod]
        public void WhenTheGameClosesWhileWaitingToComeToTheFrontItReportsTheGameClosed()
        {
            var game = new FakeGamePort { ComesToFrontAfterMs = null };
            game.AfterWait = () => game.GameRunning = false;

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameClosed, outcome);
            Assert.AreEqual(0, game.Clicks.Count);
        }

        [TestMethod]
        public void ReportsGameNotFoundAndClicksNothing()
        {
            var game = new FakeGamePort { GameRunning = false };

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type("Mocchi", 10);

            Assert.AreEqual(TypingOutcome.GameNotFound, outcome);
            Assert.AreEqual(0, game.Clicks.Count);
        }

        [TestMethod]
        [DataRow("Mo cchi")]
        [DataRow("Mocchié")] // é
        public void ReportsACharacterWithoutAKeyAndClicksNothingNotEvenBS(string searchText)
        {
            var game = new FakeGamePort();

            TypingOutcome outcome = new Typist(game, KeyboardLayout.Mr2Dx).Type(searchText, 10);

            Assert.AreEqual(TypingOutcome.CharacterWithoutKey, outcome);
            Assert.AreEqual(0, game.Clicks.Count);
        }
    }
}
