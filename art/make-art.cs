// Turns the text pixel grids into PNGs (the mascot's frames and the footer band's tiles) and
// the tool's icon, Mocchi.ico.
//
// The grids in this folder are the source of truth: palette.txt maps each character to a
// colour, each animation file (idle.txt, typing.txt, done.txt, problem.txt) holds its frames
// and play order, footer-band.txt holds the band's tiles and their order, and icon-16.txt and
// icon-32.txt hold the hand-placed icon faces. This writes png/<animation>-<frame>.png for
// every frame, png/footer-band-<tile>.png for every tile, and Mocchi.ico with 16, 32, 48 and
// 256 px images. Running it again on unchanged grids gives byte-for-byte the same files.
//
// It's compiled with the C# 5 compiler that ships with .NET Framework, so nothing new is
// installed. From the repo root, in Git Bash:
//
//   mkdir -p art/bin
//   /c/Windows/Microsoft.NET/Framework64/v4.0.30319/csc.exe -nologo -r:System.Drawing.dll -out:art/bin/make-art.exe 'art\make-art.cs'
//   art/bin/make-art.exe art
//
// csc misreads a forward-slash source path, so the source takes a backslash. The one
// argument is the art folder. art/bin/ is git-ignored.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

static class MakeArt
{
    static readonly string[] Animations = { "idle", "typing", "done", "problem" };

    const int TileWidth = 20, TileHeight = 6;

    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: make-art.exe <art folder>");
            return 1;
        }

        string artFolder = args[0];
        try
        {
            Dictionary<char, Color> palette = ReadPalette(Path.Combine(artFolder, "palette.txt"));

            // Clear out the old frames first, so a frame taken out of a grid doesn't leave
            // its PNG behind.
            string pngFolder = Path.Combine(artFolder, "png");
            Directory.CreateDirectory(pngFolder);
            foreach (string oldPng in Directory.GetFiles(pngFolder, "*.png"))
                File.Delete(oldPng);
            foreach (string animation in Animations)
            {
                List<string[]> frames = ReadGrids(Path.Combine(artFolder, animation + ".txt"), "frame", 32, 32);
                for (int i = 0; i < frames.Count; i++)
                {
                    string pngPath = Path.Combine(pngFolder, animation + "-" + (i + 1) + ".png");
                    File.WriteAllBytes(pngPath, ToPng(ToBitmap(frames[i], palette, 1)));
                }
            }

            string bandPath = Path.Combine(artFolder, "footer-band.txt");
            List<string[]> tiles = ReadGrids(bandPath, "tile", TileWidth, TileHeight);
            CheckEdgesMatch(bandPath, tiles);
            for (int i = 0; i < tiles.Count; i++)
            {
                string pngPath = Path.Combine(pngFolder, "footer-band-" + (i + 1) + ".png");
                File.WriteAllBytes(pngPath, ToPng(ToBitmap(tiles[i], palette, 1)));
            }

            string[] face16 = ReadSingleGrid(Path.Combine(artFolder, "icon-16.txt"), 16);
            string[] face32 = ReadSingleGrid(Path.Combine(artFolder, "icon-32.txt"), 32);
            WriteIcon(Path.Combine(artFolder, "Mocchi.ico"), new[]
            {
                ToBitmap(face16, palette, 1),
                ToBitmap(face32, palette, 1),
                // 48 isn't a whole multiple of 32, so it's the 32 face at 1×, centred.
                ToBitmap(Centred(face32, 48), palette, 1),
                ToBitmap(face32, palette, 8),
            });
        }
        catch (ArtException e)
        {
            Console.Error.WriteLine(e.Message);
            return 1;
        }
        return 0;
    }

    // Each line is a character, then the colour as #RRGGBB, or "transparent". Lines starting
    // with # are comments.
    static Dictionary<char, Color> ReadPalette(string path)
    {
        var palette = new Dictionary<char, Color>();
        foreach (string line in ReadLines(path))
        {
            string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || parts[0].Length != 1)
                throw new ArtException(path + ": can't read palette line \"" + line + "\"");

            char key = parts[0][0];
            if (parts[1] == "transparent")
                palette[key] = Color.Transparent;
            else if (parts[1].Length == 7 && parts[1][0] == '#')
                palette[key] = Color.FromArgb(255, Color.FromArgb(Convert.ToInt32(parts[1].Substring(1), 16)));
            else
                throw new ArtException(path + ": can't read colour \"" + parts[1] + "\"");
        }
        return palette;
    }

    // A grid file holds one or more blocks, each a heading such as "frame 1" or "tile 1",
    // numbered from 1, then its rows. An animation file also has a "play" line: the frame order
    // and each frame's time in ms, which MocchiMacro/MascotAnimation.cs copies. The footer
    // band's file has an "order" line, which MocchiMacro/FooterBand.cs copies. Here both are
    // only skipped.
    static List<string[]> ReadGrids(string path, string blockName, int width, int height)
    {
        string heading = blockName + " ";
        var grids = new List<string[]>();
        List<string> rows = null;
        foreach (string line in ReadLines(path))
        {
            if (line.StartsWith(heading))
            {
                int number;
                if (!int.TryParse(line.Substring(heading.Length).Trim(), out number) || number != grids.Count + 1)
                    throw new ArtException(path + ": expected \"" + heading + (grids.Count + 1) + "\", got \"" + line + "\"");
                rows = new List<string>();
                grids.Add(null);
            }
            else if (line.StartsWith("play ") || line.StartsWith("order "))
            {
                rows = null;
            }
            else if (rows != null)
            {
                rows.Add(line);
                if (rows.Count == height)
                    grids[grids.Count - 1] = rows.ToArray();
                else if (rows.Count > height)
                    throw new ArtException(path + ": " + heading + grids.Count + " has more than " + height + " rows");
            }
            else
            {
                throw new ArtException(path + ": unexpected line \"" + line + "\"");
            }
        }

        for (int i = 0; i < grids.Count; i++)
        {
            if (grids[i] == null)
                throw new ArtException(path + ": " + heading + (i + 1) + " has fewer than " + height + " rows");
            foreach (string row in grids[i])
                if (row.Length != width)
                    throw new ArtException(path + ": " + heading + (i + 1) + " has a row " + row.Length + " wide, not " + width + ": \"" + row + "\"");
        }
        if (grids.Count == 0)
            throw new ArtException(path + ": no " + blockName + "s");
        return grids;
    }

    static string[] ReadSingleGrid(string path, int size)
    {
        List<string[]> frames = ReadGrids(path, "frame", size, size);
        if (frames.Count != 1)
            throw new ArtException(path + ": expected one frame, got " + frames.Count);
        return frames[0];
    }

    // Blank lines and # comments are skipped. Grid rows never start with #, as no palette
    // character is #.
    static IEnumerable<string> ReadLines(string path)
    {
        if (!File.Exists(path))
            throw new ArtException(path + ": not found");
        return File.ReadAllLines(path)
            .Select(line => line.TrimEnd())
            .Where(line => line.Length > 0 && !line.StartsWith("#"));
    }

    // The tiles sit side by side in any order, so a tile whose edge column differed from the
    // others' would leave a seam beside some of them. Tile 1's edges are the ones to match.
    static void CheckEdgesMatch(string path, List<string[]> tiles)
    {
        for (int i = 1; i < tiles.Count; i++)
            for (int y = 0; y < TileHeight; y++)
                if (tiles[i][y][0] != tiles[0][y][0] || tiles[i][y][TileWidth - 1] != tiles[0][y][TileWidth - 1])
                    throw new ArtException(path + ": tile " + (i + 1) + "'s edge columns differ from tile 1's in row " + (y + 1));
    }

    // The grid in the middle of a larger, transparent one.
    static string[] Centred(string[] grid, int size)
    {
        int margin = (size - grid.Length) / 2;
        string blankRow = new string('.', size);
        string side = new string('.', margin);
        var rows = new string[size];
        for (int y = 0; y < size; y++)
        {
            int gridY = y - margin;
            rows[y] = gridY >= 0 && gridY < grid.Length ? side + grid[gridY] + side : blankRow;
        }
        return rows;
    }

    // Each grid pixel becomes a scale × scale block, with no smoothing.
    static Bitmap ToBitmap(string[] grid, Dictionary<char, Color> palette, int scale)
    {
        int width = grid[0].Length, height = grid.Length;
        var bitmap = new Bitmap(width * scale, height * scale, PixelFormat.Format32bppArgb);
        // A new Bitmap takes the screen's DPI, which the PNG records, so the files would differ
        // between machines at different display scaling. Fix it at 96.
        bitmap.SetResolution(96, 96);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color colour;
                if (!palette.TryGetValue(grid[y][x], out colour))
                    throw new ArtException("No palette colour for '" + grid[y][x] + "' in row " + (y + 1) + ": \"" + grid[y] + "\"");
                for (int dy = 0; dy < scale; dy++)
                    for (int dx = 0; dx < scale; dx++)
                        bitmap.SetPixel(x * scale + dx, y * scale + dy, colour);
            }
        }
        return bitmap;
    }

    static byte[] ToPng(Bitmap bitmap)
    {
        using (var stream = new MemoryStream())
        {
            bitmap.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }

    // An .ico is a directory of images. The 256 px image is stored as a PNG, as Windows
    // expects; the smaller ones as 32-bit bitmaps, which every Windows icon API reads.
    static void WriteIcon(string path, Bitmap[] images)
    {
        var encodedImages = images.Select(image => image.Width == 256 ? ToPng(image) : ToIconBitmap(image)).ToList();

        using (var stream = new MemoryStream())
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write((short)0);             // reserved
            writer.Write((short)1);             // type: icon
            writer.Write((short)images.Length);

            int offset = 6 + 16 * images.Length;
            for (int i = 0; i < images.Length; i++)
            {
                writer.Write((byte)(images[i].Width % 256));    // 0 means 256
                writer.Write((byte)(images[i].Height % 256));
                writer.Write((byte)0);          // colour count, 0 for 32-bit
                writer.Write((byte)0);          // reserved
                writer.Write((short)1);         // colour planes
                writer.Write((short)32);        // bits per pixel
                writer.Write(encodedImages[i].Length);
                writer.Write(offset);
                offset += encodedImages[i].Length;
            }
            foreach (byte[] bytes in encodedImages)
                writer.Write(bytes);

            writer.Flush();
            File.WriteAllBytes(path, stream.ToArray());
        }
    }

    // A BITMAPINFOHEADER, the BGRA pixels bottom row first, then a 1-bit AND mask that's set
    // where the pixel is transparent. The header's height counts both, so it's doubled.
    static byte[] ToIconBitmap(Bitmap image)
    {
        int size = image.Width;
        int maskRowBytes = (size + 31) / 32 * 4;

        using (var stream = new MemoryStream())
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(40);                   // header size
            writer.Write(size);
            writer.Write(size * 2);
            writer.Write((short)1);             // planes
            writer.Write((short)32);            // bits per pixel
            writer.Write(0);                    // no compression
            writer.Write(size * size * 4 + maskRowBytes * size);
            writer.Write(0);                    // horizontal resolution, unused
            writer.Write(0);                    // vertical resolution, unused
            writer.Write(0);                    // palette colours
            writer.Write(0);                    // important colours

            for (int y = size - 1; y >= 0; y--)
            {
                for (int x = 0; x < size; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    writer.Write(pixel.B);
                    writer.Write(pixel.G);
                    writer.Write(pixel.R);
                    writer.Write(pixel.A);
                }
            }

            for (int y = size - 1; y >= 0; y--)
            {
                var mask = new byte[maskRowBytes];
                for (int x = 0; x < size; x++)
                    if (image.GetPixel(x, y).A == 0)
                        mask[x / 8] |= (byte)(0x80 >> (x % 8));
                writer.Write(mask);
            }

            writer.Flush();
            return stream.ToArray();
        }
    }

    class ArtException : Exception
    {
        public ArtException(string message) : base(message) { }
    }
}
