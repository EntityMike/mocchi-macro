// The game's on-screen keyboard (MR2 DX), as measured in a 1600x900 screenshot of the game's
// client area (docs/screenshots/mr2/ingamekeyboard.png). Answers whether a character has a key,
// where each key's center is, and which symbol key a stand-in character stands for.

using System.Collections.Generic;
using System.Drawing;

namespace MocchiMacro
{
    public static class KeyboardLayout
    {
        // The size of the picture the key positions were measured in.
        public const int ReferenceWidth = 1600, ReferenceHeight = 900;

        // Two 5-column blocks with a wider gap between them than between keys, so each block
        // gets its own left edge.
        const float LeftBlockX = 270, RightBlockX = 735, ColumnPitch = 75;
        const float TopRowY = 280, RowPitch = 55.9f;

        // One string per row of each block. Left block row 6 has the middle dot, and right block
        // row 6 is circle, cross, triangle, square.
        static readonly string[] LeftBlock = { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXY", "Z!?()", "'\":\u00B7.", "01234" };
        static readonly string[] RightBlock = { "abcde", "fghij", "klmno", "pqrst", "uvwxy", "z\u25CB\u00D7\u25B3\u25A1", "+-=~%", "56789" };

        // Stand-ins: characters the keyboard has no key for, which the search text box swaps for
        // the symbol keys in row 6 above, so they can be typed on an ordinary keyboard or pasted.
        // Each looks a little like its symbol. The first four are Shift + a number key on a US
        // keyboard; the backtick is the only free key left that looks at all like a raised dot,
        // since a comma sits low. The Japanese middle dot is there so names pasted from Japanese
        // text work. A list, not a dictionary, so the order is fixed for the box's tooltip.
        public static readonly IReadOnlyList<(char StandIn, char Symbol)> StandIns = new[]
        {
            ('@', '\u25CB'), // ○ round
            ('*', '\u00D7'), // × multiplication sign in code
            ('^', '\u25B3'), // △ pointed top
            ('#', '\u25A1'), // □ boxy
            ('`', '\u00B7'), // · small raised mark
            ('\u30FB', '\u00B7'), // · the same dot in Japanese text
        };

        // BS sits right of the grid on the digits row, and Ent. one row below it. Their x is
        // measured; their y is the grid row's, which is within 3 px of the measured centers.
        public static readonly PointF Backspace = new PointF(1197, TopRowY + 7 * RowPitch);
        public static readonly PointF Enter = new PointF(1198, TopRowY + 8 * RowPitch);

        static readonly Dictionary<char, PointF> Keys = BuildKeys();

        public static bool HasKey(char c) => Keys.ContainsKey(c);

        // The symbol a stand-in stands for, or the character itself if it isn't a stand-in.
        public static char SwapStandIn(char c)
        {
            foreach (var (standIn, symbol) in StandIns)
                if (c == standIn) return symbol;
            return c;
        }

        // The key's center in the 1600x900 reference picture.
        public static PointF KeyCenter(char c) => Keys[c];

        static Dictionary<char, PointF> BuildKeys()
        {
            var keys = new Dictionary<char, PointF>();
            for (int row = 0; row < 8; row++)
            {
                float y = TopRowY + row * RowPitch;
                for (int col = 0; col < 5; col++)
                {
                    keys[LeftBlock[row][col]] = new PointF(LeftBlockX + col * ColumnPitch, y);
                    keys[RightBlock[row][col]] = new PointF(RightBlockX + col * ColumnPitch, y);
                }
            }
            return keys;
        }
    }
}
