// Mascot art tests: a frame comes out of the .exe at 96 px, with its pixels enlarged into sharp,
// square blocks.

using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class MascotArtTests
    {
        // Checked against the grid in art/idle.txt. Frame 1's eye highlight is one white pixel at
        // column 10, row 9, with skin above it and outline on the other three sides, so smoothing
        // or a half-pixel shift would change its block or the pixels just outside it.
        [TestMethod]
        public void LoadFrameEnlargesEachPixelIntoASharpSquareBlock()
        {
            const int blockSize = 3;
            Color white = Color.FromArgb(0xFF, 0xFF, 0xFF);
            Color outline = Color.FromArgb(0x3A, 0x24, 0x30);
            Color skin = Color.FromArgb(0xF5, 0xC9, 0xC6);

            using (Bitmap frame = MascotArt.LoadFrame("idle-1"))
            {
                Assert.AreEqual(96, frame.Width);
                Assert.AreEqual(96, frame.Height);
                Assert.AreEqual(0, frame.GetPixel(0, 0).A, "The corner should be transparent.");

                int left = 10 * blockSize, top = 9 * blockSize;
                for (int y = top; y < top + blockSize; y++)
                    for (int x = left; x < left + blockSize; x++)
                        AssertPixel(white, frame, x, y);

                AssertPixel(outline, frame, left - 1, top);
                AssertPixel(outline, frame, left + blockSize, top);
                AssertPixel(skin, frame, left, top - 1);
                AssertPixel(outline, frame, left, top + blockSize);
            }
        }

        static void AssertPixel(Color expected, Bitmap frame, int x, int y)
        {
            Assert.AreEqual(expected.ToArgb(), frame.GetPixel(x, y).ToArgb(), "Pixel at " + x + ", " + y);
        }
    }
}
