using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Polyominoes
{
    static class Program
    {
        public static Image tileImage;

        //public static Tile[,] allTiles;
        public static EndTile[,] allTiles;
        public enum PuzzleType { Elec, Mech, Plm};
        public static PuzzleType puzzleType = PuzzleType.Elec;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void LoadImages()
        {
            string imagesPath = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Images");
            tileImage = Image.FromFile(imagesPath + @"\Square.png");
        }

        public static void GeneratePolys(ref Bitmap bitmap)
        {
            Tile.size = tileImage.Width;

            int width = Tile.size * (3 * 2 + 1);
            int height = Tile.size * (3 * 15 + 14);

            //allTiles = new Tile[width, height];
            allTiles = new EndTile[width, height];

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                int x = 0, y = 0;

                // A
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, false, graphics);
                AddDrawTile(x + 2, y, false, graphics);
                AddDrawTile(x + 2, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 2, true, graphics);

                y += 4;

                // Block
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x, y + 1, true, graphics);
                AddDrawTile(x + 1, y + 1, true, graphics);

                y += 3;

                // C
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x, y, false, graphics);
                AddDrawTile(x, y + 1, false, graphics);
                AddDrawTile(x, y + 2, false, graphics);
                AddDrawTile(x + 1, y + 2, true, graphics);

                y += 4;

                // Hyphen
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, true, graphics);

                y += 2;

                // I
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x, y + 1, false, graphics);
                AddDrawTile(x, y + 2, true, graphics);

                y += 4;

                // J
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 1, true, graphics);
                AddDrawTile(x + 1, y + 2, false, graphics);
                AddDrawTile(x, y + 2, true, graphics);

                y += 4;

                // L
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x, y + 1, false, graphics);
                AddDrawTile(x, y + 2, false, graphics);
                AddDrawTile(x + 1, y + 2, true, graphics);

                y += 4;

                // M
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, false, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 2, true, graphics);

                y += 4;

                // P
                AddDrawTile(x, y, false, graphics);
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x, y + 1, false, graphics);
                AddDrawTile(x + 1, y + 1, true, graphics);
                AddDrawTile(x, y + 2, true, graphics);

                y += 4;

                // S
                AddDrawTile(x + 1, y, false, graphics);
                AddDrawTile(x + 2, y, true, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 1, y + 2, false, graphics);
                AddDrawTile(x, y + 2, true, graphics);

                y += 4;

                // T
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, false, graphics);
                AddDrawTile(x + 2, y, true, graphics);
                AddDrawTile(x + 1, y + 1, true, graphics);

                y += 3;

                // V
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x, y + 1, true, graphics);

                y += 3;

                // X
                AddDrawTile(x + 1, y, true, graphics);
                AddDrawTile(x, y + 1, true, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 1, true, graphics);
                AddDrawTile(x + 1, y + 2, true, graphics);

                y += 4;

                // Y
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 2, y, true, graphics);
                AddDrawTile(x, y + 1, false, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 1, false, graphics);
                AddDrawTile(x+ 1, y + 2, true, graphics);

                y += 4;

                // Z
                AddDrawTile(x, y, true, graphics);
                AddDrawTile(x + 1, y, false, graphics);
                AddDrawTile(x + 1, y + 1, false, graphics);
                AddDrawTile(x + 2, y + 1, true, graphics);
            }
        }

        public static void RedrawPolys(ref Bitmap bitmap)
        {
            using(Graphics graphics = Graphics.FromImage(bitmap))
            {
                foreach(Tile tile in allTiles)
                {
                    if(tile == null)
                    {
                        continue;
                    }

                    tile.Draw(graphics);
                }
            }
        }

        //private static void AddTile(int x, int y, bool isEndTile)
        //{
        //    Tile tile;

        //    if (isEndTile == true)
        //    {
        //        tile = new EndTile(x, y);
        //    }
        //    else
        //    {
        //        tile = new Tile(x, y);
        //    }

        //    allTiles[x, y] = tile;
        //}

        private static void AddDrawTile(int x, int y, bool isEndTile, Graphics graphics)
        {
            //Tile tile;
            EndTile tile;

            if (isEndTile == true)
            {
                //tile = new EndTile(x, y);
                tile = new EndTile(x, y, EndTile.Elec.positive, EndTile.Mech.jagged, EndTile.Plm.open);
            }
            else
            {
                //tile = new Tile(x, y);
                tile = new EndTile(x, y);
            }

            tile.Draw(graphics);

            allTiles[x, y] = tile;

            // Create mirrored version
            if (isEndTile == true)
            {
                //tile = new EndTile(6-x, y);
                tile = new EndTile(6 - x, y, EndTile.Elec.positive, EndTile.Mech.jagged, EndTile.Plm.open);
            }
            else
            {
                //tile = new Tile(6 - x, y);
                tile = new EndTile(6 - x, y);
            }

            tile.Draw(graphics);

            allTiles[6 - x, y] = tile;
        }

        public static EndTile GetEndTileAtPosition(Point position)
        {
            int pixelX = position.X;
            int pixelY = position.Y;

            //pixelX = 20;    //testing
            //pixelY = 15;    //testing

            int x = pixelX / tileImage.Width;
            int y = pixelY / tileImage.Height;

            //Tile tile = allTiles[x, y];

            //EndTile endTile = (EndTile)tile;
            EndTile endTile = allTiles[x, y];
            if(endTile.IsEndTile != true)
            {
                return null;
            }

            return endTile;
        }
    }
}