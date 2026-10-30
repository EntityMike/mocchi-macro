// Types a search text into the game's monster field by clicking keys on its keyboard:
// first BS enough times to clear the field, then each key of the search text. It stops
// before Ent., so the organizer can check the text and run the search themselves.

using System.Drawing;
using System.Linq;

namespace MocchiMacro
{
    public enum TypingOutcome
    {
        Done,
        GameNotFound,
    }

    public class Typist
    {
        // The monster field holds at most 12 characters, so 12 BS always clears it.
        const int ClearCount = 12;

        readonly IGamePort game;

        public Typist(IGamePort game)
        {
            this.game = game;
        }

        public TypingOutcome Type(string searchText, int keysPerSecond)
        {
            if (!game.FindGame()) return TypingOutcome.GameNotFound;

            PointF[] keys = Enumerable.Repeat(KeyboardLayout.Backspace, ClearCount)
                .Concat(searchText.Select(KeyboardLayout.KeyCenter))
                .ToArray();

            // Schedule each click against the clock (click n at n intervals from the start)
            // instead of waiting a fixed gap, so late wake-ups don't add up.
            double intervalMs = 1000.0 / keysPerSecond;
            long startMs = game.NowMs;
            for (int n = 0; n < keys.Length; n++)
            {
                long waitMs = startMs + (long)(n * intervalMs) - game.NowMs;
                if (waitMs > 0) game.Wait((int)waitMs);

                // Read the client area before every click, so typing follows the window if it moves.
                Rectangle client = game.ReadClientArea();
                game.Click(new Point(client.Left + (int)keys[n].X, client.Top + (int)keys[n].Y));
            }
            return TypingOutcome.Done;
        }
    }
}
