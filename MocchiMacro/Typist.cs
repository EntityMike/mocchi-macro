// Types a search text into the game's monster field by clicking keys on its keyboard, using
// the keyboard layout of the game it was given: it brings the game to the front (restoring it
// if minimized), clicks BS enough times to clear the field, then each key of the search text.
// It stops before Ent., so the organizer can check the text and run the search themselves. A
// search text with a character the keyboard has no key for is refused before any click. It
// reports how typing went, and clicks nothing more once something has gone wrong.

using System.Drawing;
using System.Linq;

namespace MocchiMacro
{
    public enum TypingOutcome
    {
        Done,
        GameNotFound,
        CharacterWithoutKey,
        GameNotInFront,
        GameClosed,
    }

    public class Typist
    {
        // The monster field holds at most 12 characters, so 12 BS always clears it.
        const int ClearCount = 12;

        // How long to wait for the game to come to the front, checking every so often.
        const int InFrontTimeoutMs = 5000;
        const int InFrontCheckMs = 50;

        // Once in front, the game needs a moment before it takes clicks.
        const int ReadyAfterInFrontMs = 500;

        readonly IGamePort game;
        readonly KeyboardLayout layout;

        public Typist(IGamePort game, KeyboardLayout layout)
        {
            this.game = game;
            this.layout = layout;
        }

        public TypingOutcome Type(string searchText, int typingRate)
        {
            // Checked before any click, BS included, so text the keyboard can't type leaves the
            // monster field as it was. The search text box already drops such characters; this
            // guards text that reaches the typist another way.
            if (!searchText.All(layout.HasKey)) return TypingOutcome.CharacterWithoutKey;
            if (!game.FindGame()) return TypingOutcome.GameNotFound;

            // Restore only a minimized game: restoring a maximized one would shrink it.
            if (game.IsMinimized()) game.Restore();

            // Clicks land on whatever window is in front at the key's spot, so the game must
            // really be in front before the first one.
            game.BringToFront();
            long askedMs = game.NowMs;
            while (!game.IsInFront())
            {
                if (!game.GameStillExists()) return TypingOutcome.GameClosed;
                if (game.NowMs - askedMs >= InFrontTimeoutMs) return TypingOutcome.GameNotInFront;
                game.Wait(InFrontCheckMs);
            }
            game.Wait(ReadyAfterInFrontMs);

            PointF[] keys = Enumerable.Repeat(layout.Backspace, ClearCount)
                .Concat(searchText.Select(layout.KeyCenter))
                .ToArray();

            // Schedule each click against the clock (click n at n intervals from the start)
            // instead of waiting a fixed gap, so late wake-ups don't add up.
            double intervalMs = 1000.0 / typingRate;
            long startMs = game.NowMs;
            for (int n = 0; n < keys.Length; n++)
            {
                long waitMs = startMs + (long)(n * intervalMs) - game.NowMs;
                if (waitMs > 0) game.Wait((int)waitMs);

                // Once the game is gone, a click would land on whatever was behind it.
                if (!game.GameStillExists()) return TypingOutcome.GameClosed;

                // Read the client area before every click, so typing follows the window if it moves.
                Rectangle picture = FindPicture(game.ReadClientArea());
                game.Click(ToScreen(keys[n], picture));
            }
            return TypingOutcome.Done;
        }

        // The game's 16:9 picture inside the client area. In a window it fills the client area;
        // in fullscreen on a monitor that isn't 16:9 it sits centered between black bars, at the
        // sides of a wider monitor or above and below a taller one. Bars are cut off to whole
        // pixels, so a window a fraction of a pixel off 16:9 (854x480) gets none.
        static Rectangle FindPicture(Rectangle client)
        {
            // How far the client area is off 16:9, kept in whole numbers: width - height * 16/9
            // times 9, which is also height - width * 9/16 times -16.
            int offBy = client.Width * 9 - client.Height * 16;
            if (offBy > 0)
            {
                int bar = offBy / 18;
                return new Rectangle(client.Left + bar, client.Top, client.Width - 2 * bar, client.Height);
            }
            else
            {
                int bar = -offBy / 32;
                return new Rectangle(client.Left, client.Top + bar, client.Width, client.Height - 2 * bar);
            }
        }

        // Scales a key's center from the reference picture to the actual picture, cutting
        // fractions off to whole pixels as the keyboard test did.
        static Point ToScreen(PointF key, Rectangle picture)
        {
            return new Point(
                picture.Left + (int)((double)key.X * picture.Width / KeyboardLayout.ReferenceWidth),
                picture.Top + (int)((double)key.Y * picture.Height / KeyboardLayout.ReferenceHeight));
        }
    }
}
