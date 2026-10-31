// The games' on-screen keyboards, one layout per game, as measured in 1600x900 screenshots of
// each game's client area (docs/screenshots/mr1/ingamekeyboard.png and
// docs/screenshots/mr2/ingamekeyboard.png). Each layout answers whether a character has a key,
// where each key's center is, and which symbol key a stand-in character stands for.

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MocchiMacro
{
    public class KeyboardLayout
    {
        // The size of the picture the key positions were measured in.
        public const int ReferenceWidth = 1600, ReferenceHeight = 900;

        // Stand-ins: characters neither keyboard has a key for, which the search text box swaps
        // for symbol keys, so they can be typed on an ordinary keyboard or pasted. Each looks a
        // little like its symbol. The first four are Shift + a number key on a US keyboard; the
        // backtick is the only free key left that looks at all like a raised dot, since a comma
        // sits low. The Japanese middle dot is there so names pasted from Japanese text work.
        // Each layout keeps the ones whose symbol it has a key for. A list, not a dictionary, so
        // the order is fixed for the box's tooltip. Declared before the layouts, which read it
        // as they're built.
        static readonly (char StandIn, char Symbol)[] AllStandIns =
        {
            ('@', '\u25CB'), // ○ round
            ('*', '\u00D7'), // × multiplication sign in code
            ('^', '\u25B3'), // △ pointed top
            ('#', '\u25A1'), // □ boxy
            ('`', '\u00B7'), // · small raised mark
            ('\u30FB', '\u00B7'), // · the same dot in Japanese text
        };

        public static readonly KeyboardLayout Mr1Dx = BuildMr1Dx();
        public static readonly KeyboardLayout Mr2Dx = BuildMr2Dx();

        readonly Dictionary<char, PointF> keys;

        public IReadOnlyList<(char StandIn, char Symbol)> StandIns { get; }
        public PointF Backspace { get; }
        public PointF Enter { get; }

        KeyboardLayout(Dictionary<char, PointF> keys, PointF backspace, PointF enter)
        {
            this.keys = keys;
            Backspace = backspace;
            Enter = enter;
            StandIns = AllStandIns.Where(s => keys.ContainsKey(s.Symbol)).ToArray();
        }

        public bool HasKey(char c) => keys.ContainsKey(c);

        // The symbol a stand-in stands for, or the character itself if it isn't one of this
        // layout's stand-ins.
        public char SwapStandIn(char c)
        {
            foreach (var (standIn, symbol) in StandIns)
                if (c == standIn) return symbol;
            return c;
        }

        // The key's center in the 1600x900 reference picture.
        public PointF KeyCenter(char c) => keys[c];

        // MR1 DX's screenshot is 1602x927, the client area plus the window border and title bar,
        // so its positions were measured minus (1, 26).
        static KeyboardLayout BuildMr1Dx()
        {
            const float leftBlockX = 278, rightBlockX = 728, columnPitch = 75;
            const float topRowY = 236, rowPitch = 60;

            // A space marks a cell left out. The keyboard's empty cells type a space in the game,
            // but they're left out, so MR1 DX has no space key, like MR2 DX. Right block row 5,
            // column 4 is ←, MR1 DX's BS, which is left out as well so it can't be typed as a
            // character. Left block row 6 is circle, cross and three punctuation keys; row 7 ends
            // in the middle dot.
            string[] leftBlock = { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXY", "Z    ", "\u25CB\u00D7!?~", "+-%.\u00B7", "12345" };
            string[] rightBlock = { "abcde", "fghij", "klmno", "pqrst", "uvwxy", "z_   ", "/    ", "     ", "67890" };
            var keys = GridKeys(leftBlock, rightBlock, leftBlockX, rightBlockX, columnPitch, topRowY, rowPitch);

            // BS is the ← key on the grid. Ent. is the END key below the grid, measured.
            var backspace = new PointF(rightBlockX + 4 * columnPitch, topRowY + 5 * rowPitch);
            var enter = new PointF(1242, 710);
            return new KeyboardLayout(keys, backspace, enter);
        }

        static KeyboardLayout BuildMr2Dx()
        {
            const float leftBlockX = 270, rightBlockX = 735, columnPitch = 75;
            const float topRowY = 280, rowPitch = 55.9f;

            // Left block row 6 has the middle dot, and right block row 5 is z then circle, cross,
            // triangle, square.
            string[] leftBlock = { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXY", "Z!?()", "'\":\u00B7.", "01234" };
            string[] rightBlock = { "abcde", "fghij", "klmno", "pqrst", "uvwxy", "z\u25CB\u00D7\u25B3\u25A1", "+-=~%", "56789" };
            var keys = GridKeys(leftBlock, rightBlock, leftBlockX, rightBlockX, columnPitch, topRowY, rowPitch);

            // BS sits right of the grid on the digits row, and Ent. one row below it. Their x is
            // measured; their y is the grid row's, which is within 3 px of the measured centers.
            var backspace = new PointF(1197, topRowY + 7 * rowPitch);
            var enter = new PointF(1198, topRowY + 8 * rowPitch);
            return new KeyboardLayout(keys, backspace, enter);
        }

        // Places two 5-column blocks of keys, one string per row. The gap between the blocks is
        // wider than between keys, so each block gets its own left edge. Spaces are cells left out.
        static Dictionary<char, PointF> GridKeys(string[] leftBlock, string[] rightBlock,
            float leftBlockX, float rightBlockX, float columnPitch, float topRowY, float rowPitch)
        {
            var keys = new Dictionary<char, PointF>();
            for (int row = 0; row < leftBlock.Length; row++)
            {
                float y = topRowY + row * rowPitch;
                for (int col = 0; col < 5; col++)
                {
                    if (leftBlock[row][col] != ' ') keys[leftBlock[row][col]] = new PointF(leftBlockX + col * columnPitch, y);
                    if (rightBlock[row][col] != ' ') keys[rightBlock[row][col]] = new PointF(rightBlockX + col * columnPitch, y);
                }
            }
            return keys;
        }
    }
}
