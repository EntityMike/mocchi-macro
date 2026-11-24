// The live check: runs the real tool against a fake game window and checks what its window really
// does, which the automated tests can't, since they use neither the window nor the mouse. It
// checks the mascot's animations frame by frame and where each click lands, for both games, ending
// with a run over every key on each keyboard.
//
// It takes over the mouse for about two minutes, so it's run by hand, not with the tests:
//   dotnet build mocchi-macro.slnx
//   MocchiMacro.LiveCheck/bin/Debug/net48/MocchiMacro.LiveCheck.exe
// Watch the cursor land on each key, or look afterward at the pictures it saves in live-check-output
// next to the .exe: red rings are clicks, and the blue ring is where the layout puts Ent. (END in
// MR1 DX), which no Go clicks. It exits with 0 if everything passed.
//
// It can't show that the game accepts the clicks: the fake game window only records them. And the
// expected click positions come from the tool's own layouts, so the screenshot under the marks is
// what shows those layouts match the game.

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
    static class Program
    {
        // 12 characters, so typing ends the way a full name does, with keys from all over each keyboard.
        const string SearchText = "Mocchi+Tiger";

        // The typist waits this long for the game to settle, and the last click takes this long.
        const int TypistStartMs = 500, LastClickMs = 100;
        const int BackspaceClicks = 12;

        // The most the search text box holds, so the most one Go can type.
        const int MaxSearchTextLength = 12;

        // How far measured times may be from the animation's, given timer ticks and captures.
        const int HoldToleranceMs = 300, TypingToleranceMs = 1000;

        // The animation's own times, from art/*.txt: the frames' length plus the 2 s hold.
        const int DoneToIdleMs = 510 + 2000, ProblemToIdleMs = 500 + 2000;
        const int TypingLoopMs = 800, LongestTypingFrameMs = 240;

        public static readonly string OutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "live-check-output");

        static int failures;

        [STAThread]
        static int Main()
        {
            // Without this, Windows scales this program's numbers at display scaling above 100%, and
            // neither the 1600x900 window nor the clicks' positions would mean what they say. It has
            // to come before anything reads the screen.
            SetProcessDPIAware();

            // Go would click into a real game window, whichever one it found first.
            if (Game.All.Any(game => FindWindow(null, game.WindowTitle) != IntPtr.Zero))
            {
                Console.WriteLine("A game is running, so the check didn't start: Go would click into it.");
                return 2;
            }

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            if (screen.Width < KeyboardLayout.ReferenceWidth || screen.Height < KeyboardLayout.ReferenceHeight + 50)
            {
                Console.WriteLine("The fake game window doesn't fit on the main screen, so clicks could miss it.");
                return 2;
            }

            Directory.CreateDirectory(OutputFolder);

            // Taken before the tool starts, so the tool shows its taken-hotkey startup message.
            bool holdingHotkey = RegisterHotKey(IntPtr.Zero, HotkeyId, MOD_ALT | MOD_CONTROL | MOD_SHIFT, (uint)Keys.M);

            using (var tool = ToolWindow.Launch())
            {
                Console.WriteLine("Display scaling: " + tool.Dpi * 100 / 96 + "%");
                tool.SaveScreenshot(Path.Combine(OutputFolder, "tool-window.png"));
                tool.SelectGame(Game.Mr2Dx);
                var watcher = new MascotWatcher(tool);

                CheckIdleWithStartupMessage(tool, watcher);
                if (holdingHotkey)
                {
                    UnregisterHotKey(IntPtr.Zero, HotkeyId);
                }

                CheckEmptyBox(tool, watcher);
                CheckGameNotFound(tool, watcher, Game.Mr2Dx);

                using (var fake = FakeGameWindow.Open(Game.Mr2Dx, tool))
                {
                    Console.WriteLine();
                    Console.WriteLine("Go on MR2 DX at 1 key/s");
                    CheckGoRun(tool, watcher, fake, Game.Mr2Dx, typingRate: 1, watchAfterMs: 1000, pressedDuringHold: false);
                    fake.SaveMarkedPicture(Path.Combine(OutputFolder, "mr2-clicks.png"));

                    Console.WriteLine();
                    Console.WriteLine("Go during the hold, on MR2 DX at 10 keys/s");
                    CheckGoRun(tool, watcher, fake, Game.Mr2Dx, typingRate: 10, watchAfterMs: 6000, pressedDuringHold: true);
                }

                tool.SelectGame(Game.Mr1Dx);
                using (var fake = FakeGameWindow.Open(Game.Mr1Dx, tool))
                {
                    Console.WriteLine();
                    Console.WriteLine("Go on MR1 DX at 3 keys/s");
                    CheckGoRun(tool, watcher, fake, Game.Mr1Dx, typingRate: 3, watchAfterMs: 1000, pressedDuringHold: false);
                    fake.SaveMarkedPicture(Path.Combine(OutputFolder, "mr1-clicks.png"));
                }

                CheckWholeKeyboard(tool, Game.Mr2Dx, "mr2-whole-keyboard.png");
                CheckWholeKeyboard(tool, Game.Mr1Dx, "mr1-whole-keyboard.png");

                Console.WriteLine();
                Check(watcher.UnknownFrames == 0, "Every frame matched its art exactly, so it's crisp (" + watcher.UnknownFrames + " didn't)");
                Check(watcher.SlowestReplyMs <= 200, "The window stayed responsive (slowest reply " + watcher.SlowestReplyMs + " ms)");
            }

            Console.WriteLine();
            Console.WriteLine(failures == 0 ? "All passed." : failures + " FAILED.");
            Console.WriteLine("Pictures: " + OutputFolder);
            return failures == 0 ? 0 : 1;
        }

        static void CheckIdleWithStartupMessage(ToolWindow tool, MascotWatcher watcher)
        {
            Console.WriteLine();
            Console.WriteLine("Idle, with a startup message showing");
            Check(tool.Status.Contains("taken"), "The startup message shows: " + tool.Status);

            List<(long Ms, string Frame)> timeline = watcher.Watch(7000);
            Show(timeline);
            Check(timeline.All(shown => shown.Frame.StartsWith("idle")), "Only idle frames, so no droop");

            // Timed by the closed-eyes frame, which is the longest of the blink's.
            var blinks = timeline.Where(shown => shown.Frame == "idle-3").ToList();
            Check(blinks.Count >= 2 && Near(blinks[1].Ms - blinks[0].Ms, 3220, HoldToleranceMs),
                "Blinks about every 3.2 s");
        }

        static void CheckEmptyBox(ToolWindow tool, MascotWatcher watcher)
        {
            Console.WriteLine();
            Console.WriteLine("Go with an empty box");
            tool.PressGo();
            CheckDroopedOnce(watcher);
            Check(tool.Status.Contains("Type a search text"), "Status: " + tool.Status);
        }

        static void CheckGameNotFound(ToolWindow tool, MascotWatcher watcher, Game game)
        {
            Console.WriteLine();
            Console.WriteLine("Go with the game closed");
            tool.SetSearchText(SearchText);
            tool.PressGo();
            CheckDroopedOnce(watcher);
            Check(tool.Status.Contains(game.Name + " not found"), "Status: " + tool.Status);
        }

        static void CheckDroopedOnce(MascotWatcher watcher)
        {
            List<(long Ms, string Frame)> timeline = watcher.Watch(3500);
            Show(timeline);

            var droop = timeline.SkipWhile(shown => shown.Frame != "problem-1").ToList();
            Check(FramesOf(droop) == "problem-1 problem-2 problem-3 idle-1", "Droops once, then idles");
            Check(droop.Count == 4 && Near(droop[3].Ms - droop[0].Ms, ProblemToIdleMs, HoldToleranceMs), "Holds about 2 s");
        }

        static void CheckGoRun(ToolWindow tool, MascotWatcher watcher, FakeGameWindow fake, Game game,
            int typingRate, int watchAfterMs, bool pressedDuringHold)
        {
            tool.SetSearchText(SearchText);
            tool.SetTypingRate(typingRate);
            fake.ClearClicks();
            tool.PressGo();
            tool.WaitForGoDisabled();

            // Stops a set time after Go comes back, so the hop is watched as well as the typing.
            long goBackMs = -1;
            List<(long Ms, string Frame)> timeline = watcher.Watch(120000, elapsedMs =>
            {
                if (goBackMs < 0 && tool.GoEnabled)
                {
                    goBackMs = elapsedMs;
                }

                return goBackMs >= 0 && elapsedMs - goBackMs >= watchAfterMs;
            });

            int hopAt = timeline.FindIndex(shown => shown.Frame == "done-1");
            if (hopAt < 0)
            {
                Show(timeline);
                Check(false, "Hops when typing ends. Status: " + tool.Status);
                return;
            }

            // A hop from before this Go may still be showing at the very start.
            var typing = timeline.Take(hopAt).SkipWhile(shown => shown.Frame.StartsWith("done")).ToList();
            var after = timeline.Skip(hopAt).ToList();
            long typingEndMs = timeline[hopAt].Ms;
            Show(typing.Take(3).ToList(), "  ...");
            Show(after);

            if (pressedDuringHold)
            {
                Check(typing.Count > 0 && typing[0].Ms <= 200, "Go cuts the hold short and the loop starts at once");
            }

            long expectedTypingMs = TypistStartMs + (BackspaceClicks + SearchText.Length - 1) * 1000 / typingRate + LastClickMs;
            Check(Near(typingEndMs, expectedTypingMs, TypingToleranceMs),
                "Typing took about as long as its clicks (" + typingEndMs + " ms, expected about " + expectedTypingMs + ")");
            CheckTypingLoop(typing, typingEndMs);

            bool sawIdle = after.Any(shown => shown.Frame == "idle-1");
            string expectedHop = "done-1 done-2 done-1 done-3" + (sawIdle ? " idle-1" : "");
            Check(FramesOf(after.Take(sawIdle ? 5 : 4).ToList()) == expectedHop
                && !after.Skip(4).Any(shown => shown.Frame.StartsWith("done")), "Hops once");
            if (sawIdle)
            {
                Check(Near(after[4].Ms - after[0].Ms, DoneToIdleMs, HoldToleranceMs), "Holds about 2 s, then idles");
                Check(after.Any(shown => shown.Frame == "idle-3"), "Blinks again after the hold");
            }

            Check(tool.Status.StartsWith("Done"), "Status: " + tool.Status);
            CheckClicks(fake.Clicks, ExpectedClicks(game, SearchText));
        }

        // Every key a search text can use, over as many Go runs as the box's limit takes. The marks
        // build up over the runs, so the saved picture shows every key clicked. It can't include
        // Ent., which the typist never clicks, so the organizer can check the text first; the
        // picture's blue ring shows where Ent. would be instead.
        static void CheckWholeKeyboard(ToolWindow tool, Game game, string pictureName)
        {
            const int typingRate = 10;

            // The layouts can say whether a character has a key but can't list them, so every
            // character is asked. In character order, which scatters the clicks across the keyboard.
            var keys = Enumerable.Range(char.MinValue, char.MaxValue + 1)
                .Select(code => (char)code).Where(game.Layout.HasKey).ToList();
            var searchTexts = Enumerable.Range(0, (keys.Count + MaxSearchTextLength - 1) / MaxSearchTextLength)
                .Select(run => new string(keys.Skip(run * MaxSearchTextLength).Take(MaxSearchTextLength).ToArray())).ToList();

            Console.WriteLine();
            Console.WriteLine("Every key on " + game.Name + " at " + typingRate + " keys/s: "
                + keys.Count + " keys over " + searchTexts.Count + " Go runs");

            tool.SelectGame(game);
            tool.SetTypingRate(typingRate);
            using (var fake = FakeGameWindow.Open(game, tool))
            {
                var expected = new List<PointF>();
                int doneRuns = 0;
                foreach (string searchText in searchTexts)
                {
                    tool.SetSearchText(searchText);
                    tool.PressGo();
                    tool.WaitForGoDisabled();
                    tool.WaitForGoEnabled();
                    if (tool.Status.StartsWith("Done"))
                    {
                        doneRuns++;
                    }

                    expected.AddRange(ExpectedClicks(game, searchText));
                }

                Check(doneRuns == searchTexts.Count, "Every Go run ended in Done (" + doneRuns + " of " + searchTexts.Count + ")");
                CheckClicks(fake.Clicks, expected);
                fake.SaveMarkedPicture(Path.Combine(OutputFolder, pictureName));
            }
        }

        // typing-1 is the same picture as idle-1, so MascotWatcher reports it as that.
        static void CheckTypingLoop(List<(long Ms, string Frame)> typing, long typingEndMs)
        {
            string[] loopFrames = { "idle-1", "typing-2", "typing-3", "typing-4" };

            // At the very start, a blink's frame can show for one tick before the loop takes over.
            Check(typing.Where(shown => shown.Ms > 100).All(shown => loopFrames.Contains(shown.Frame)),
                "Only the typing loop's frames while typing");

            int loops = typing.Count(shown => shown.Frame == "idle-1");
            Check(loops >= typingEndMs / TypingLoopMs - 2, "The loop ran the whole time (" + loops + " loops)");

            long longestGapMs = typing.Zip(typing.Skip(1), (a, b) => b.Ms - a.Ms).DefaultIfEmpty().Max();
            longestGapMs = Math.Max(longestGapMs, typingEndMs - typing.Last().Ms);
            Check(longestGapMs <= LongestTypingFrameMs + HoldToleranceMs, "The loop never stalled (longest frame " + longestGapMs + " ms)");
        }

        static IEnumerable<PointF> ExpectedClicks(Game game, string searchText) =>
            Enumerable.Repeat(game.Layout.Backspace, BackspaceClicks).Concat(searchText.Select(game.Layout.KeyCenter));

        static void CheckClicks(IReadOnlyList<Point> clicks, IEnumerable<PointF> expectedClicks)
        {
            var expected = expectedClicks.ToList();
            Check(clicks.Count == expected.Count, "Clicked every key (" + clicks.Count + " of " + expected.Count + ")");

            // The window is the reference size, so each click lands on the key's measured center,
            // less the fraction the typist cuts off.
            int misses = clicks.Zip(expected, (click, key) => Math.Abs(click.X - key.X) <= 1 && Math.Abs(click.Y - key.Y) <= 1)
                .Count(onKey => !onKey);
            Check(misses == 0, "Every click landed on its key (" + misses + " missed)");
        }

        static string FramesOf(List<(long Ms, string Frame)> timeline) => string.Join(" ", timeline.Select(shown => shown.Frame));

        static bool Near(long actual, long expected, int tolerance) => Math.Abs(actual - expected) <= tolerance;

        static void Show(List<(long Ms, string Frame)> timeline, string suffix = "")
        {
            Console.WriteLine("  " + string.Join(", ", timeline.Select(shown => shown.Frame + " at " + shown.Ms)) + suffix);
        }

        static void Check(bool passed, string what)
        {
            Console.WriteLine((passed ? "  pass  " : "  FAIL  ") + what);
            if (!passed)
            {
                failures++;
            }
        }

        // ---- Win32 interop (user32.dll) ----

        // Any number works, since the hotkey is only held for this program's first check.
        const int HotkeyId = 1;
        const uint MOD_ALT = 0x0001, MOD_CONTROL = 0x0002, MOD_SHIFT = 0x0004;

        [DllImport("user32.dll")] static extern bool SetProcessDPIAware();
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string className, string title);
        [DllImport("user32.dll")] static extern bool RegisterHotKey(IntPtr hwnd, int id, uint modifiers, uint key);
        [DllImport("user32.dll")] static extern bool UnregisterHotKey(IntPtr hwnd, int id);
    }
}
