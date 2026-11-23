// Mascot art tests: a frame comes out of the .exe at 96 px, with its pixels enlarged into sharp,
// square blocks, and the icon is embedded for the window and set on the .exe itself.

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
            var white = Color.FromArgb(0xFF, 0xFF, 0xFF);
            var outline = Color.FromArgb(0x3A, 0x24, 0x30);
            var skin = Color.FromArgb(0xF5, 0xC9, 0xC6);

            using (Bitmap frame = MascotArt.LoadFrame("idle-1"))
            {
                Assert.AreEqual(96, frame.Width);
                Assert.AreEqual(96, frame.Height);
                Assert.AreEqual(0, frame.GetPixel(0, 0).A, "The corner should be transparent.");

                int left = 10 * blockSize, top = 9 * blockSize;
                for (int y = top; y < top + blockSize; y++)
                {
                    for (int x = left; x < left + blockSize; x++)
                    {
                        AssertPixel(white, frame, x, y);
                    }
                }

                AssertPixel(outline, frame, left - 1, top);
                AssertPixel(outline, frame, left + blockSize, top);
                AssertPixel(skin, frame, left, top - 1);
                AssertPixel(outline, frame, left, top + blockSize);
            }
        }

        // Checked against the grid in art/icon-16.txt. Its left eye has a white pixel at column 3,
        // row 8, with outline to its right and skin to its left, so a face shrunk from a larger
        // size would blur them.
        [TestMethod]
        public void LoadIconGivesTheHandPlacedFaceAtSmallSize()
        {
            var white = Color.FromArgb(0xFF, 0xFF, 0xFF);
            var outline = Color.FromArgb(0x3A, 0x24, 0x30);
            var skin = Color.FromArgb(0xF5, 0xC9, 0xC6);

            using (Icon icon = MascotArt.LoadIcon())
            using (var small = new Icon(icon, 16, 16))
            using (var face = small.ToBitmap())
            {
                Assert.AreEqual(16, face.Width);
                Assert.AreEqual(16, face.Height);
                Assert.AreEqual(0, face.GetPixel(0, 0).A, "The corner should be transparent.");
                AssertPixel(white, face, 3, 8);
                AssertPixel(outline, face, 4, 8);
                AssertPixel(skin, face, 2, 8);
            }
        }

        // An .exe with no icon of its own gets Windows' generic one, which wouldn't match. The
        // embedded icon is asked for at the size Windows returned, which can vary with scaling.
        [TestMethod]
        public void TheExeCarriesTheEmbeddedIcon()
        {
            using (var exeIcon = Icon.ExtractAssociatedIcon(typeof(MascotArt).Assembly.Location))
            using (var exeFace = exeIcon.ToBitmap())
            using (Icon embeddedIcon = MascotArt.LoadIcon())
            using (var embeddedAtExeSize = new Icon(embeddedIcon, exeFace.Size))
            using (var embeddedFace = embeddedAtExeSize.ToBitmap())
            {
                for (int y = 0; y < exeFace.Height; y++)
                {
                    for (int x = 0; x < exeFace.Width; x++)
                    {
                        // A fully transparent pixel's color can't be seen, and Windows doesn't keep it.
                        if (exeFace.GetPixel(x, y).A == 0 && embeddedFace.GetPixel(x, y).A == 0)
                        {
                            continue;
                        }

                        AssertPixel(embeddedFace.GetPixel(x, y), exeFace, x, y);
                    }
                }
            }
        }

        static void AssertPixel(Color expected, Bitmap bitmap, int x, int y)
        {
            Assert.AreEqual(expected.ToArgb(), bitmap.GetPixel(x, y).ToArgb(), "Pixel at " + x + ", " + y);
        }
    }
}
