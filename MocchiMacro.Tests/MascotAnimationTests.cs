// Mascot animation tests: which frame shows at which moment, with no window or real clock
// involved. The expected frames and times are worked out from the "play" lines in art/*.txt.

using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class MascotAnimationTests
    {
        [TestMethod]
        public void IdleStandsStillThenBlinksEveryFewSeconds()
        {
            var mascot = new MascotAnimation();

            Assert.AreEqual("idle-1", mascot.FrameAt(0));
            Assert.AreEqual("idle-1", mascot.FrameAt(2999));
            Assert.AreEqual("idle-2", mascot.FrameAt(3000));
            Assert.AreEqual("idle-3", mascot.FrameAt(3060));
            Assert.AreEqual("idle-2", mascot.FrameAt(3160));

            // The blink is over at 3220, and the next one comes 3 s after that.
            Assert.AreEqual("idle-1", mascot.FrameAt(3220));
            Assert.AreEqual("idle-1", mascot.FrameAt(6219));
            Assert.AreEqual("idle-2", mascot.FrameAt(6220));
        }

        [TestMethod]
        public void TypingLoopsAtItsOwnSpeedForAsLongAsItRuns()
        {
            var mascot = new MascotAnimation();
            mascot.PlayTyping(1000);

            Assert.AreEqual("typing-1", mascot.FrameAt(1000));
            Assert.AreEqual("typing-2", mascot.FrameAt(1200));
            Assert.AreEqual("typing-3", mascot.FrameAt(1290));
            Assert.AreEqual("typing-4", mascot.FrameAt(1380));
            Assert.AreEqual("typing-3", mascot.FrameAt(1620));
            Assert.AreEqual("typing-2", mascot.FrameAt(1710));

            // One loop is 800 ms, so it comes round again, and is still going 30 loops later. That's
            // 24 s, about what a 12-character search text at 1 key/s takes with its 12 BS clicks.
            Assert.AreEqual("typing-1", mascot.FrameAt(1800));
            Assert.AreEqual("typing-4", mascot.FrameAt(1000 + 30 * 800 + 380));
        }

        [TestMethod]
        public void DoneHopsOnceHoldsItsLastFrameThenIdles()
        {
            var mascot = new MascotAnimation();
            mascot.PlayTyping(0);
            mascot.PlayDone(1000);

            Assert.AreEqual("done-1", mascot.FrameAt(1000));
            Assert.AreEqual("done-2", mascot.FrameAt(1080));
            Assert.AreEqual("done-1", mascot.FrameAt(1280));
            Assert.AreEqual("done-3", mascot.FrameAt(1360));

            // The hop's last frame shows for its 150 ms, then holds for 2 s more.
            Assert.AreEqual("done-3", mascot.FrameAt(1510));
            Assert.AreEqual("done-3", mascot.FrameAt(3509));

            // Idle starts from its beginning, so the first blink is a full 3 s away.
            Assert.AreEqual("idle-1", mascot.FrameAt(3510));
            Assert.AreEqual("idle-1", mascot.FrameAt(6509));
            Assert.AreEqual("idle-2", mascot.FrameAt(6510));
        }

        [TestMethod]
        public void ProblemDroopsOnceHoldsItsLastFrameThenIdles()
        {
            var mascot = new MascotAnimation();
            mascot.PlayProblem(1000);

            Assert.AreEqual("problem-1", mascot.FrameAt(1000));
            Assert.AreEqual("problem-2", mascot.FrameAt(1150));
            Assert.AreEqual("problem-3", mascot.FrameAt(1300));

            // The droop's last frame shows for its 200 ms, then holds for 2 s more.
            Assert.AreEqual("problem-3", mascot.FrameAt(3499));
            Assert.AreEqual("idle-1", mascot.FrameAt(3500));
        }

        [TestMethod]
        public void TypingCutsTheHoldShort()
        {
            var mascot = new MascotAnimation();
            mascot.PlayDone(1000);
            Assert.AreEqual("done-3", mascot.FrameAt(2000));

            mascot.PlayTyping(2000);

            Assert.AreEqual("typing-1", mascot.FrameAt(2000));
            Assert.AreEqual("typing-2", mascot.FrameAt(2200));
        }

        // Go with an empty box during a hold ends in a problem, so it droops rather than typing.
        [TestMethod]
        public void ProblemCutsTheHoldShort()
        {
            var mascot = new MascotAnimation();
            mascot.PlayDone(1000);
            mascot.PlayProblem(2000);

            Assert.AreEqual("problem-1", mascot.FrameAt(2000));
            Assert.AreEqual("problem-3", mascot.FrameAt(4399));
            Assert.AreEqual("idle-1", mascot.FrameAt(4550));
        }

        // MascotAnimation's tables are copies of the "play" lines in art/*.txt. This fails when
        // someone changes one side without the other.
        [TestMethod]
        public void EveryAnimationMatchesThePlayLineInItsArtFile()
        {
            AssertMatchesArtFile("idle", new MascotAnimation(), loops: true);

            var typing = new MascotAnimation();
            typing.PlayTyping(0);
            AssertMatchesArtFile("typing", typing, loops: true);

            var done = new MascotAnimation();
            done.PlayDone(0);
            AssertMatchesArtFile("done", done, loops: false);

            var problem = new MascotAnimation();
            problem.PlayProblem(0);
            AssertMatchesArtFile("problem", problem, loops: false);
        }

        static void AssertMatchesArtFile(string animation, MascotAnimation mascot, bool loops)
        {
            // Such as "play 1 3000, 2 60": frame 1 shows for 3000 ms, then frame 2 for 60 ms.
            string testsFolder = Path.GetDirectoryName(typeof(MascotAnimationTests).Assembly.Location);
            string playLine = File.ReadLines(Path.Combine(testsFolder, "art", animation + ".txt"))
                .Single(line => line.StartsWith("play "));
            var steps = playLine.Substring("play ".Length).Split(',')
                .Select(step => step.Trim().Split(' '))
                .Select(parts => (Frame: animation + "-" + parts[0], Ms: int.Parse(parts[1])))
                .ToList();

            long startMs = 0;
            foreach ((string Frame, int Ms) step in steps)
            {
                Assert.AreEqual(step.Frame, mascot.FrameAt(startMs), animation + " at " + startMs + " ms");
                long lastMs = startMs + step.Ms - 1;
                Assert.AreEqual(step.Frame, mascot.FrameAt(lastMs), animation + " at " + lastMs + " ms");
                startMs += step.Ms;
            }

            // Just past the end, so a table with an extra frame fails too.
            string frameAfterEnd = loops ? steps.First().Frame : steps.Last().Frame;
            Assert.AreEqual(frameAfterEnd, mascot.FrameAt(startMs), animation + " just past its end");
        }
    }
}
