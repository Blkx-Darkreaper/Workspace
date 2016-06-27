using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace SpriteRipper
{
    public static class Program
    {
        //private static List<Tile> allTiles { get; set; }
        //private static LinkedList<Tile> allTiles { get; set; }
        private static SortedSet<Tile> allTiles { get; set; }
        private static ConcurrentBag<Tile> tileDepository { get; set; }
        public static bool displayReady { get; private set; }
        private static TileSorting allSortedTiles { get; set; }
        public static bool tilesetReady { get; private set; }
        public static Color backgroundColour { get; set; }
        private static int ACCURACY = 2;
        public static int tasksComplete { get; private set; }
        public static int totalTasks { get; private set; }
        public static int duplicates { get; private set; }
        public static long sortTime { get; private set; }
        private const string IMAGE_FILTER = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif";
        private const int MAX_THREADS = 4;
        private static int MAX_CONCURRENT_TILES = 160;

        static Program()
        {
            allTiles = null;
            tileDepository = null;
            allSortedTiles = null;

            displayReady = false;
            tilesetReady = false;

            // default colour
            backgroundColour = Color.LightSeaGreen;

            tasksComplete = 0;
            totalTasks = 0;
            duplicates = 0;
            sortTime = 0;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Gui());
        }

        public static int GetTileCount()
        {
            if (allTiles == null)
            {
                return 0;
            }

            return allTiles.Count;
        }

        public static int GetSortedTileCount()
        {
            if (allSortedTiles == null)
            {
                return 0;
            }

            return allSortedTiles.TileCount;
        }

        public static int GetGroupCount()
        {
            if (allSortedTiles == null)
            {
                return 0;
            }

            return allSortedTiles.GroupCount;
        }

        public static int GetCountOfGroup(int groupIndex)
        {
            return allSortedTiles.allTileGroups[groupIndex].Count;
        }

        public static OpenFileDialog GetFilenameToOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            // Set filter options
            dialog.Filter = IMAGE_FILTER;

            dialog.Multiselect = false;

            return dialog;
        }

        public static SaveFileDialog GetFilenameToSave()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            // Set filter options
            dialog.Filter = IMAGE_FILTER;

            return dialog;
        }

        public static ImageFormat GetImageFormat(string filename)
        {
            filename = filename.ToLower();
            int start = filename.IndexOf(".") + 1;
            int length = filename.Length - start;
            string extension = filename.Substring(start, length);

            ImageFormat format;
            switch (extension)
            {
                case "bmp":
                    format = ImageFormat.Bmp;
                    break;

                case "gif":
                    format = ImageFormat.Gif;
                    break;

                case "jpg":
                case "jpeg":
                    format = ImageFormat.Jpeg;
                    break;

                case "png":
                    format = ImageFormat.Png;
                    break;

                case "tif":
                    format = ImageFormat.Tiff;
                    break;

                default:
                    throw new Exception("Invalid file format");
            }

            return format;
        }

        public static Bitmap LoadImage(string path)
        {
            Bitmap imageToLoad = new Bitmap(path);
            return imageToLoad;
        }

        public static Bitmap ProcessImage(Bitmap image, int bitsPerColour, int tileSize, float patternThreshold, float colourThreshold)
        {
            // Split the image into tiles
            LoadAllTiles(image, bitsPerColour, tileSize);

            // Sort all tiles into groups based on comparison
            SortTiles(patternThreshold, colourThreshold);

            // Build tileset
            //PixelFormat format = image.PixelFormat;
            PixelFormat format = PixelFormat.Format24bppRgb;
            int zoom = 1;
            Bitmap tileset = GetGroupedTileset(format, tileSize, zoom);
            return tileset;
        }

        public static void ResetAll()
        {
            //allTiles = new List<Tile>();
            //allTiles = new LinkedList<Tile>();
            allTiles = new SortedSet<Tile>();
            displayReady = false;

            ResetTileset();
        }

        public static void ResetTileset()
        {
            allSortedTiles = new TileSorting();
            tilesetReady = false;
        }

        public static Bitmap GetGroupedTileset(PixelFormat format, int tileSize, int zoom)
        {
            if (allSortedTiles == null)
            {
                return null;
            }

            int maxTilesWide = allSortedTiles.GetMaxGroupSize();
            int totalGroups = allSortedTiles.Count;

            int width = maxTilesWide * tileSize * zoom + maxTilesWide - 1;
            int height = totalGroups * tileSize * zoom + 5 * (totalGroups - 1);

            Bitmap tileset = new Bitmap(width, height, format);

            // Fill with background colour
            SetBackgroundColour(width, height, tileset);

            // Draw tile groups in tileset
            for (int i = 0; i < allSortedTiles.Count; i++)
            {
                TileGroup group = allSortedTiles[i];
                int masterIndex = group.masterIndex;
                //Tile master = allTiles[masterIndex];
                Tile master = allTiles.ElementAt(masterIndex);
                // draw tileToDraw
                int x = 0;
                int y = i * (tileSize * zoom + 5);
                DrawTileOntoImage(tileset, master, x, y, zoom);

                List<int> similarTiles = group.GetSortedTiles();
                // draw similar tiles
                for (int j = 0; j < similarTiles.Count; j++)
                {
                    x = tileSize * zoom + 1 + j * (tileSize * zoom + 1);
                    int tileIndex = similarTiles[j];
                    //Tile tileToDraw = allTiles[tileIndex];
                    Tile tileToDraw = allTiles.ElementAt(tileIndex);
                    DrawTileOntoImage(tileset, tileToDraw, x, y, zoom);
                }
            }

            return tileset;
        }

        public static Bitmap GetTileset(PixelFormat format, int tileSize, int tilesWide, int zoom)
        {
            int totalTiles = allSortedTiles.TileCount;

            int width = tilesWide * tileSize * zoom + tilesWide - 1;

            int rows = (int)Math.Ceiling((double)totalTiles / (double)tilesWide);
            int height = rows * tileSize * zoom + rows - 1;

            Bitmap tileset = new Bitmap(width, height, format);

            // Fill with background colour
            SetBackgroundColour(width, height, tileset);

            // Draw tile groups in sequence
            int tileNumber = 0;
            for (int i = 0; i < allSortedTiles.Count; i++)
            {
                // draw tileToDraw
                TileGroup group = allSortedTiles[i];
                int masterIndex = group.masterIndex;
                //Tile master = allTiles[masterIndex];
                Tile master = allTiles.ElementAt(masterIndex);
                DrawNextTileOntoImage(tileset, master, tileSize, tilesWide, zoom, ref tileNumber);

                List<int> similarTiles = group.GetSortedTiles();
                // draw similar tiles
                for (int j = 0; j < similarTiles.Count; j++)
                {
                    int tileIndex = similarTiles[j];
                    //Tile tileToDraw = allTiles[tileIndex];
                    Tile tileToDraw = allTiles.ElementAt(tileIndex);
                    DrawNextTileOntoImage(tileset, tileToDraw, tileSize, tilesWide, zoom, ref tileNumber);
                }
            }

            return tileset;
        }

        private static void SetBackgroundColour(int width, int height, Bitmap tileset)
        {
            Graphics g = Graphics.FromImage(tileset);
            SolidBrush brush = new SolidBrush(backgroundColour);
            g.FillRectangle(brush, 0, 0, width, height);
        }

        private static void DrawNextTileOntoImage(Bitmap tileset, Tile tileToDraw, int tileSize, int tilesWide, int zoom, ref int count)
        {
            int x = (count % tilesWide) * tileSize * zoom + (count % tilesWide);
            int y = (count / tilesWide) * tileSize * zoom + (count / tilesWide);

            DrawTileOntoImage(tileset, tileToDraw, x, y, zoom);
            count++;
        }

        //private static Bitmap DrawGroupedTileset(PixelFormat format, int tileSize, int width, int height, int zoom)
        //{
        //    if (allSortedTiles == null)
        //    {
        //        return null;
        //    }

        //    Bitmap tileset = new Bitmap(width, height, format);

        //    // Draw tile groups in tileset
        //    for (int i = 0; i < allSortedTiles.Count; i++)
        //    {
        //        TileGroup group = allSortedTiles[i];
        //        int masterIndex = group.masterIndex;
        //        Tile tileToDraw = allTiles[masterIndex];
        //        // draw tileToDraw
        //        int x = 0;
        //        int y = 1 + i * (tileSize * zoom + 5);
        //        DrawTileOntoImage(tileset, tileToDraw, x, y, zoom);

        //        List<int> similarTiles = group.GetSortedTiles();
        //        // draw similar tiles
        //        for (int j = 0; j < similarTiles.Count; j++)
        //        {
        //            x = tileSize * zoom + 1 + j * (tileSize * zoom + 1);
        //            int tileIndex = similarTiles[j];
        //            Tile tileToDraw = allTiles[tileIndex];
        //            DrawTileOntoImage(tileset, tileToDraw, x, y, zoom);
        //        }
        //    }

        //    return tileset;
        //}

        public static void DrawAllTilesOntoImage(Bitmap image, int tilesWide, int tileSize, int zoom)
        {
            for (int i = 0; i < allTiles.Count; i++)
            {
                //Tile tile = allTiles[i];
                Tile tile = allTiles.ElementAt(i);
                int x = i % tilesWide * (tileSize * zoom + 1);
                int y = i / tilesWide * (tileSize * zoom + 1);

                DrawTileOntoImage(image, tile, x, y, zoom);
            }
        }

        private static void DrawTileOntoImage(Bitmap image, Tile tile, int x, int y)
        {
            DrawTileOntoImage(image, tile, x, y, 1);
        }

        private static void DrawTileOntoImage(Bitmap image, Tile tile, int x, int y, int zoom)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Image tileImage = tile.image;
                int width = tileImage.Width * zoom;
                int height = tileImage.Height * zoom;

                g.DrawImage(tileImage, x, y, width, height);
            }
        }

        public static void SortTiles(float patternThreshold, float colourThreshold)
        {
            tilesetReady = false;
            tasksComplete = 0;
            totalTasks = allTiles.Count;
            duplicates = 0;
            sortTime = 0;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            allSortedTiles = new TileSorting();
            //while(allTiles.Count > 0)
            for (int i = 0; i < allTiles.Count; i++)
            {
                tasksComplete = i;

                //Tile tile = allTiles[i];
                Tile tile = allTiles.ElementAt(i);
                //Tile tile = allTiles.First.Value;
                //allTiles.RemoveFirst();

                if (i == 0)
                {
                    allSortedTiles.AddGroup(i);
                    continue;
                }

                TileGroup group = FindMatchingGroup(tile, patternThreshold, colourThreshold);
                if (group == null)
                {
                    // Add to new group
                    allSortedTiles.AddGroup(i);
                    continue;
                }

                // Convert pattern match to hashcode
                int masterIndex = group.masterIndex;
                //Tile master = allTiles[masterIndex];
                Tile master = allTiles.ElementAt(masterIndex);
                Tuple<float, float> results = master.GetMatches(tile);
                float patternMatch = results.Item1;
                float colourMatch = results.Item2;

                bool areIdentical = Tile.IdenticalTo(patternMatch, colourMatch);
                if (areIdentical == true)
                {
                    duplicates++;
                    continue;
                }

                // If tilecount is saved for each group, could improve performance by reducing the hashcode size since tilecount gets smaller with time
                int key = Tile.GetHashcode(ACCURACY, patternMatch, colourMatch, totalTasks);
                AddSimilarTileToGroup(i, tile, group, key);
            }

            timer.Stop();
            sortTime = timer.ElapsedMilliseconds;

            tilesetReady = true;
        }

        private static void AddSimilarTileToGroup(int tileIndex, Tile tile, TileGroup group, int key)
        {

            // Check if key already exists
            bool hasKey = group.ContainsKey(key);
            while (hasKey == true)
            {
                int otherTileIndex = group[key];
                //Tile otherTile = allTiles[otherTileIndex];
                Tile otherTile = allTiles.ElementAt(otherTileIndex);
                Tuple<float, float> results = otherTile.GetMatches(tile);
                float patternMatch = results.Item1;
                float colourMatch = results.Item2;

                bool areIdentical = Tile.IdenticalTo(patternMatch, colourMatch);
                if (areIdentical == true)
                {
                    duplicates++;
                    return;
                }

                key++;
                hasKey = group.ContainsKey(key);
            }

            group.AddSimilar(key, tileIndex);
        }

        private static TileGroup FindMatchingGroup(Tile tile, float patternThreshold, float colourThreshold)
        {
            foreach (TileGroup group in allSortedTiles)
            {
                int masterIndex = group.masterIndex;
                //Tile master = allTiles[masterIndex];
                Tile master = allTiles.ElementAt(masterIndex);
                bool areSimilar = master.SimilarTo(tile, patternThreshold, colourThreshold);
                if (areSimilar == false)
                {
                    continue;
                }

                return group;
            }

            return null;
        }

        public static void LoadAllTiles(Bitmap image, int bitsPerColour, int tileSize)
        {
            LoadAllTiles(image, bitsPerColour, tileSize, 0, 0);
        }

        public static void LoadAllTiles(Bitmap image, int bitsPerColour, int tileSize, int offsetX, int offsetY)
        {
            displayReady = false;
            int width = image.Width - offsetX;
            int height = image.Height - offsetY;
            int totalTiles = Program.GetTileCount(width, height, tileSize);

            tasksComplete = 0;
            totalTasks = totalTiles;

            int subWidth = width;
            int subHeight = height;

            while (totalTiles > MAX_CONCURRENT_TILES)
            {
                subWidth /= 2;
                subHeight /= 2;

                totalTiles = Program.GetTileCount(subWidth, subHeight, tileSize);
            }

            int subTilesWide = subWidth / tileSize;
            int totalSubImages = (width / subWidth) * (height / subHeight);

            //allTiles = new List<Tile>();
            //allTiles = new LinkedList<Tile>();
            allTiles = new SortedSet<Tile>();
            //tileDepository = new ConcurrentBag<Tile>();   //threaded

            for (int i = 0; i < totalSubImages; i++)
            {
                int x = (i % subTilesWide) * subWidth;
                int y = (i / subTilesWide) * subHeight;
                Rectangle rect = new Rectangle(x, y, subWidth, subHeight);
                Bitmap subImage = image.Clone(rect, PixelFormat.Format24bppRgb);
                LoadTiles(subImage, bitsPerColour, tileSize, offsetX, offsetY);
            }

            //allTiles = new SortedSet<Tile>(tileDepository); //threaded
            displayReady = true;
        }

        private static void LoadTiles(Bitmap image, int bitsPerColour, int tileSize, int offsetX, int offsetY)
        {
            int boundsX = image.Width - tileSize * (int)Math.Ceiling((double)offsetX / tileSize);
            int boundsY = image.Height - tileSize * (int)Math.Ceiling((double)offsetY / tileSize);

            for (int y = offsetY; y < boundsY; y += tileSize)
            {
                for (int x = offsetX; x < boundsX; x += tileSize)
                {
                    Tile tileToAdd;
                    try
                    {
                        //tileToAdd = GetTile(image, bitsPerColour, x, y, tileSize);
                        tileToAdd = GetTile(image, bitsPerColour, x, y, tileSize, tasksComplete);
                    }
                    catch (OutOfMemoryException ex)
                    {
                        MessageBox.Show(string.Format("Error loading tiles. {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    allTiles.Add(tileToAdd);
                    //allTiles.AddLast(tileToAdd);

                    tasksComplete++;
                }
            }
        }

        public static void LoadTilesThreaded(Bitmap image, int bitsPerColour, int tileSize, int offsetX, int offsetY)
        {
            int rowCount = GetRowCount(image.Height, tileSize, offsetY);
            int threads = 0;
            //ManualResetEvent[] completedRows = new ManualResetEvent[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                if (threads == MAX_THREADS)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                // Assign a row to a thread
                //completedRows[i] = new ManualResetEvent(false);
                //ThreadInfo info = new ThreadInfo(image, bitsPerColour, tileSize, offsetX, offsetY, i, completedRows[i]);
                //ThreadPool.QueueUserWorkItem(LoadTileRow, info);
                Thread thread = new Thread(() => LoadTileRow(image, bitsPerColour, tileSize, offsetX, offsetY, i));
                thread.Start();
                thread.Join();
                threads++;
            }

            // Wait until all rows have been processed
            //WaitHandle.WaitAll(completedRows);
        }

        private class ThreadInfo
        {
            public Bitmap image { get; set; }
            public int bitsPerColour { get; set; }
            public int tileSize { get; set; }
            public int offsetX { get; set; }
            public int offsetY { get; set; }
            public int rowNumber { get; set; }
            public ManualResetEvent completionStatus { get; set; }

            public ThreadInfo(Bitmap image, int bitsPerColour, int tileSize, int offsetX, int offsetY, int rowNumber, ManualResetEvent completionStatus)
            {
                this.image = image;
                this.bitsPerColour = bitsPerColour;
                this.tileSize = tileSize;
                this.offsetX = offsetX;
                this.offsetY = offsetY;
                this.rowNumber = rowNumber;
                this.completionStatus = completionStatus;
            }
        }

        private static void LoadTileRow(Bitmap image, int bitsPerColour, int tileSize, int offsetX, int offsetY, int rowNumber)
        {
            //private static void LoadTileRow(object parameters)
            //{
            //ThreadInfo info = parameters as ThreadInfo;

            //Bitmap image = info.image;
            //int bitsPerColour = info.bitsPerColour;
            //int tileSize = info.tileSize;
            //int offsetX = info.offsetX;
            //int offsetY = info.offsetY;
            //int rowNumber = info.rowNumber;
            //ManualResetEvent resetEvent = info.completionStatus;

            int tilesWide = GetColumnsCount(image.Width, tileSize, offsetX);
            int y = tileSize * rowNumber + offsetY;

            for (int i = 0; i < tilesWide; i++)
            {
                int x = i * tileSize + offsetX;
                int tileNumber = tilesWide * rowNumber + i;

                Tile tileToAdd;
                try
                {
                    //tileToAdd = GetTile(image, bitsPerColour, x, y, tileSize);
                    tileToAdd = GetTile(image, bitsPerColour, x, y, tileSize, tileNumber);
                }
                catch (OutOfMemoryException ex)
                {
                    MessageBox.Show(string.Format("Error loading tiles. {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                tileDepository.Add(tileToAdd);
            }

            //resetEvent.Set();
            tasksComplete += tilesWide;
        }

        public static int GetTileCount(int width, int height, int tileSize)
        {
            int tilesWide = GetColumnsCount(width, tileSize);
            int tilesHigh = GetRowCount(height, tileSize);

            int totalTiles = tilesWide * tilesHigh;
            return totalTiles;
        }

        private static int GetRowCount(int height, int tileSize, int offset)
        {
            int tilesHigh = (int)Math.Floor((double)(height - offset) / (double)tileSize);
            return tilesHigh;
        }

        private static int GetRowCount(int height, int tileSize)
        {
            return GetRowCount(height, tileSize, 0);
        }

        private static int GetColumnsCount(int width, int tileSize, int offset)
        {
            int tilesWide = (int)Math.Floor((double)(width - offset) / (double)tileSize);
            return tilesWide;
        }

        private static int GetColumnsCount(int width, int tileSize)
        {
            return GetColumnsCount(width, tileSize, 0);
        }

        private static Tile GetTile(Bitmap image, int bitsPerColour, int x, int y, int size)
        {
            Rectangle rect = new Rectangle(x, y, size, size);
            Bitmap subImage = image.Clone(rect, image.PixelFormat);
            Tile tile = new Tile(subImage, bitsPerColour);
            return tile;
        }

        private static Tile GetTile(Bitmap image, int bitsPerColour, int x, int y, int size, int index)
        {
            Rectangle rect = new Rectangle(x, y, size, size);
            Bitmap subImage = image.Clone(rect, image.PixelFormat);
            Tile tile = new Tile(subImage, bitsPerColour, index);
            return tile;
        }
    }
}
