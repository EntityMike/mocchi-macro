// The footer band: a still strip of pixel-art grass along the bottom of the window. It's built
// from the tiles in art/footer-band.txt, laid side by side in that file's order; art/make-art.cs
// says how the tiles become the embedded PNGs.

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MocchiMacro
{
    public static class FooterBand
    {
        // The tiles' numbers in the order they're laid, copied from the "order" line in
        // art/footer-band.txt. Change them together: a test fails if they differ. It fills a row
        // 840 px wide, the window at 200% scaling, and starts over if the row is wider.
        public static readonly IReadOnlyList<int> Order = new[] { 1, 3, 1, 2, 1, 4, 1, 1, 2, 3, 1, 4, 2, 1 };

        // The band as one image, exactly the row's width, so it fills the row at any display
        // scaling. The tiles are enlarged like the mascot's frames.
        public static Bitmap Build(int width)
        {
            var tiles = Enumerable.Range(1, Order.Max())
                .Select(number => MascotArt.LoadEnlarged("footer-band-" + number + ".png"))
                .ToList();
            try
            {
                int tileWidth = tiles[0].Width;
                var band = new Bitmap(width, tiles[0].Height);
                using (var graphics = Graphics.FromImage(band))
                {
                    // Drawn at their own pixel size, given outright: DrawImageUnscaled would scale
                    // by each bitmap's DPI. The last tile is cut off at the row's edge.
                    for (int i = 0; i * tileWidth < width; i++)
                    {
                        graphics.DrawImage(tiles[Order[i % Order.Count] - 1], i * tileWidth, 0, tileWidth, band.Height);
                    }
                }
                return band;
            }
            finally
            {
                foreach (Bitmap tile in tiles)
                {
                    tile.Dispose();
                }
            }
        }
    }
}
