// Footer band tests: the band comes out as wide as asked, its tiles in the order from
// art/footer-band.txt, enlarged into sharp blocks, and that order matches the code's copy.

using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class FooterBandTests
    {
        // Checked against the grids in art/footer-band.txt. Each tile is 20×6 pixels, so 60×18 at
        // 3×, and the order starts 1 3. A row of 870 px is wider than the order's 14 tiles
        // (840 px), so it starts over with tile 1 and cuts that one off partway.
        [TestMethod]
        public void BuildLaysTheTilesInOrderAsWideAsTheRowInSharpBlocks()
        {
            const int blockSize = 3, tileWidth = 60;
            Color blue = Color.FromArgb(0x5A, 0xAE, 0xE8);
            Color yellow = Color.FromArgb(0xF9, 0xD5, 0x40);
            Color darkGrass = Color.FromArgb(0x26, 0x59, 0x2C);
            Color grass = Color.FromArgb(0x3F, 0x91, 0x40);

            using (Bitmap band = FooterBand.Build(870))
            {
                Assert.AreEqual(870, band.Width);
                Assert.AreEqual(18, band.Height);

                // The second tile is tile 3, whose flower's top petal is one blue pixel at column
                // 7, row 0, with nothing either side and the yellow centre below. Smoothing or a
                // half-pixel shift would change its block or the pixels just outside it.
                int left = tileWidth + 7 * blockSize;
                for (int y = 0; y < blockSize; y++)
                    for (int x = left; x < left + blockSize; x++)
                        AssertPixel(blue, band, x, y);
                Assert.AreEqual(0, band.GetPixel(left - 1, 0).A, "Left of the petal should be transparent.");
                Assert.AreEqual(0, band.GetPixel(left + blockSize, 0).A, "Right of the petal should be transparent.");
                AssertPixel(yellow, band, left, blockSize);

                // Past the 14th tile it starts over: tile 1 has a blade tip at column 4, row 1,
                // where no other tile has dark grass.
                AssertPixel(darkGrass, band, 14 * tileWidth + 4 * blockSize, 1 * blockSize);

                // And it reaches the row's last pixel, in tile 1's bottom row of grass.
                AssertPixel(grass, band, 869, 17);
            }
        }

        static void AssertPixel(Color expected, Bitmap bitmap, int x, int y)
        {
            Assert.AreEqual(expected.ToArgb(), bitmap.GetPixel(x, y).ToArgb(), "Pixel at " + x + ", " + y);
        }

        // FooterBand's order is a copy of the "order" line in art/footer-band.txt. This fails
        // when someone changes one side without the other.
        [TestMethod]
        public void TheOrderMatchesTheOrderLineInItsArtFile()
        {
            // Such as "order 1 3 1 2": tile 1, then tile 3, then tile 1 again.
            string testsFolder = Path.GetDirectoryName(typeof(FooterBandTests).Assembly.Location);
            string orderLine = File.ReadLines(Path.Combine(testsFolder, "art", "footer-band.txt"))
                .Single(line => line.StartsWith("order "));
            int[] fileOrder = orderLine.Substring("order ".Length)
                .Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            CollectionAssert.AreEqual(fileOrder, FooterBand.Order.ToArray());
        }
    }
}
