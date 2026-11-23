// Watches the tool's mascot and names the frame showing, by comparing every captured pixel with
// the frames the tool has embedded. So a frame only gets a name if it's on screen exactly as drawn:
// a blurred one matches nothing and counts as unknown.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;

namespace MocchiMacro.LiveCheck
{
    sealed class MascotWatcher
    {
        const string ResourcePrefix = "Mascot.", ResourceSuffix = ".png";

        // The window-look spec's fixed 3× at every display scaling.
        const int BlockSize = 3;

        readonly ToolWindow tool;
        readonly Dictionary<string, string> frameNames = new Dictionary<string, string>();

        public int UnknownFrames { get; private set; }
        public long SlowestReplyMs { get; private set; }

        public MascotWatcher(ToolWindow tool)
        {
            this.tool = tool;
            LoadFrames();
        }

        // Each frame as it should look on screen: every art pixel a square block, over the window's
        // background, which shows at the transparent corner of whichever frame is up now. It's built
        // from the raw PNGs rather than with MascotArt.LoadFrame, or a LoadFrame that blurred would
        // match its own blur here. Sorted, and only the first name kept for a picture, so typing-1,
        // the same picture as idle-1, is reported as idle-1.
        void LoadFrames()
        {
            Color background;
            using (Bitmap now = tool.CaptureMascot())
            {
                background = now.GetPixel(0, 0);
            }

            IOrderedEnumerable<string> names = typeof(MascotArt).Assembly.GetManifestResourceNames()
                .Where(resource => resource.StartsWith(ResourcePrefix) && resource.EndsWith(ResourceSuffix))
                .Select(resource => resource.Substring(ResourcePrefix.Length, resource.Length - ResourcePrefix.Length - ResourceSuffix.Length))
                .OrderBy(name => name, StringComparer.Ordinal);

            foreach (string name in names)
            {
                using (Stream png = typeof(MascotArt).Assembly.GetManifestResourceStream(ResourcePrefix + name + ResourceSuffix))
                using (var art = new Bitmap(png))
                using (var onWindow = new Bitmap(art.Width * BlockSize, art.Height * BlockSize, PixelFormat.Format32bppArgb))
                {
                    for (int y = 0; y < onWindow.Height; y++)
                    {
                        for (int x = 0; x < onWindow.Width; x++)
                        {
                            Color pixel = art.GetPixel(x / BlockSize, y / BlockSize);
                            onWindow.SetPixel(x, y, pixel.A == 0 ? background : pixel);
                        }
                    }
                    string fingerprint = Fingerprint(onWindow);
                    if (!frameNames.ContainsKey(fingerprint))
                    {
                        frameNames[fingerprint] = name;
                    }
                }
            }
        }

        public List<(long Ms, string Frame)> Watch(int forMs) => Watch(forMs, elapsedMs => false);

        // Each change of frame, with its time in ms from the start of the watch. Stops after forMs,
        // or sooner once stopWhen, given the elapsed time, says so.
        public List<(long Ms, string Frame)> Watch(int forMs, Func<long, bool> stopWhen)
        {
            var timeline = new List<(long Ms, string Frame)>();
            var clock = Stopwatch.StartNew();
            while (clock.ElapsedMilliseconds < forMs && !stopWhen(clock.ElapsedMilliseconds))
            {
                string frame = CurrentFrame();
                if (timeline.Count == 0 || timeline.Last().Frame != frame)
                {
                    timeline.Add((clock.ElapsedMilliseconds, frame));
                }

                SlowestReplyMs = Math.Max(SlowestReplyMs, tool.MeasureReplyMs());
                Thread.Sleep(5);
            }
            return timeline;
        }

        string CurrentFrame()
        {
            using (Bitmap captured = tool.CaptureMascot())
            {
                if (frameNames.TryGetValue(Fingerprint(captured), out string name))
                {
                    return name;
                }

                UnknownFrames++;
                // Only the first few, since a blurred mascot would be every capture.
                if (UnknownFrames <= 3)
                {
                    captured.Save(Path.Combine(Program.OutputFolder, "unknown-frame-" + UnknownFrames + ".png"), ImageFormat.Png);
                }

                return "unknown";
            }
        }

        // A short summary of all a picture's pixels, to look it up by. Alpha is left out: PrintWindow doesn't fill it
        // in reliably, and everything on the window is opaque by now.
        static string Fingerprint(Bitmap picture)
        {
            BitmapData data = picture.LockBits(new Rectangle(Point.Empty, picture.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var pixels = new byte[picture.Width * picture.Height * 4];
            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            picture.UnlockBits(data);
            for (int alpha = 3; alpha < pixels.Length; alpha += 4)
            {
                pixels[alpha] = 255;
            }

            using (var md5 = MD5.Create())
            {
                return Convert.ToBase64String(md5.ComputeHash(pixels));
            }
        }
    }
}
