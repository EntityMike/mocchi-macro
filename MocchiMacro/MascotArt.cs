// The mascot's art as the window shows it. The frames are the PNGs from art/png and the icon is
// art/Mocchi.ico, all embedded in the .exe so it stays a single file; art/make-art.cs says how
// those are made.

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace MocchiMacro
{
    public static class MascotArt
    {
        // Every frame pixel becomes a 3×3 block, so a 32-pixel frame shows at 96 px. A whole
        // multiple keeps it crisp. It's the same at every display scaling, so the mascot looks
        // smaller beside the controls the higher the scaling goes: the window-look spec says why.
        const int Scale = 3;

        // The frame named like its PNG, such as "idle-1". It's enlarged once here, rather than
        // each time it's drawn, so the window can show it at its own size and nothing later gets
        // a chance to smooth it.
        public static Bitmap LoadFrame(string name)
        {
            using (Stream png = typeof(MascotArt).Assembly.GetManifestResourceStream("Mascot." + name + ".png"))
            using (var frame = new Bitmap(png))
            {
                var enlarged = new Bitmap(frame.Width * Scale, frame.Height * Scale);
                using (Graphics graphics = Graphics.FromImage(enlarged))
                {
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    // Without this, GDI+ samples from pixel corners rather than centres, which
                    // shifts the picture by half a source pixel and trims its top and left edges.
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;
                    graphics.DrawImage(frame, 0, 0, enlarged.Width, enlarged.Height);
                }
                return enlarged;
            }
        }

        // The icon with all its sizes, so the window can take the nearest one: at 100% scaling,
        // the hand-placed 16 px face for the title bar rather than a shrunken 32. Icon keeps its
        // own copy of the data, so the stream can close.
        public static Icon LoadIcon()
        {
            using (Stream ico = typeof(MascotArt).Assembly.GetManifestResourceStream("Mocchi.ico"))
                return new Icon(ico);
        }
    }
}
