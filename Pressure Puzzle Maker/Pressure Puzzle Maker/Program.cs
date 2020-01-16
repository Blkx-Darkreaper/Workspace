using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pressure_Puzzle_Maker
{
    public enum Direction { Up, Right, Down, Left };

    static class Program
    {
        public static Image blankImage = Image.FromFile(
            @"D:\Users\Darkreaper\Git\Workspace\Pressure Puzzle Maker\Pressure Puzzle Maker\Images\Blank.png");
        public static Image blockedImage = Image.FromFile(
            @"D:\Users\Darkreaper\Git\Workspace\Pressure Puzzle Maker\Pressure Puzzle Maker\Images\Blocked.png");
        public static Image startImage = Image.FromFile(
            @"D:\Users\Darkreaper\Git\Workspace\Pressure Puzzle Maker\Pressure Puzzle Maker\Images\Start.png");
        public static Image invalidImage = Image.FromFile(
            @"D:\Users\Darkreaper\Git\Workspace\Pressure Puzzle Maker\Pressure Puzzle Maker\Images\Invalid.png");
        public static Image offLimitsImage = Image.FromFile(
            @"D:\Users\Darkreaper\Git\Workspace\Pressure Puzzle Maker\Pressure Puzzle Maker\Images\OffLimits.png");

        private static int perfectPathLength = 14;
        private static int maxPathLength = perfectPathLength + 2;
        private static int minPathLength = perfectPathLength - 2;
        private static int shortPathLength = 6;
        private static int longPathLength = 18;

        private static Color veryLongPathColour = Color.Red;
        private static Color longPathColour = Color.Orange;
        private static Color idealPathColour = Color.YellowGreen;
        private static Color perfectPathColour = Color.Green;
        private static Color shortPathColour = Color.Blue;
        private static Color veryShortPathColour = Color.Indigo;

        private static Color[] allPathColours = {Color.PaleVioletRed, Color.MediumVioletRed, Color.Red,
            Color.OrangeRed, Color.Orange, Color.Yellow, Color.YellowGreen, Color.GreenYellow, Color.Green,
            Color.SkyBlue, Color.LightBlue, Color.Blue, Color.Indigo, Color.Violet};

        public static Tile[,] allTiles;
        public static List<Tile> allStartTiles;

        public static bool imageOutdated { get; private set; }
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

        public static void ImageChanged()
        {
            imageOutdated = true;
        }

        public static void GeneratePuzzle(ref Bitmap bitmap, int width, int height)
        {
            allTiles = new Tile[width, height];
            allStartTiles = new List<Tile>();

            imageOutdated = true;

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Create new tile and draw
                        Tile tile = new Tile(x, y);
                        tile.Draw(graphics);

                        // Add to array
                        allTiles[x, y] = tile;
                    }
                }
            }

            imageOutdated = false;
        }

        public static void RedrawPuzzle(ref Bitmap bitmap)
        {
            //if(imageOutdated == false)
            //{
            //    return;
            //}

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                foreach (Tile tile in allTiles)
                {
                    tile.Draw(graphics);
                    //tile.Invalidate();
                }

                // Draw paths
                if (allStartTiles.Count < 2)
                {
                    return;
                }

                //return; //testing

                Tile startTile = allStartTiles[0];
                int startX = startTile.position.X;
                int startY = startTile.position.Y;

                for (int i = 1; i < allStartTiles.Count; i++)
                {
                    Tile endTile = allStartTiles[i];

                    int endX = endTile.position.X;
                    int endY = endTile.position.Y;

                    //int colourIndex = 0;

                    List<List<Point>> allPaths = GetPaths(startX, startY, endX, endY);
                    allPaths.Sort(CompareByCountClosestToPerfect);

                    foreach (List<Point> path in allPaths)
                    {
                        int pathLength = path.Count;
                        if(pathLength > longPathLength + shortPathLength)
                        {
                            continue;
                        }

                        Color penColour = perfectPathColour;

                        if(pathLength > maxPathLength)
                        {
                            penColour = longPathColour;
                        }
                        if(pathLength > longPathLength)
                        {
                            penColour = veryLongPathColour;
                        }

                        if(pathLength < minPathLength)
                        {
                            penColour = shortPathColour;
                        }
                        if(pathLength < shortPathLength)
                        {
                            penColour = veryShortPathColour;
                        }

                        if(pathLength != perfectPathLength)
                        {
                            penColour = idealPathColour;
                        }

                        //if (path.Count > pathLength)
                        //if (path.Count != maxPathLength)
                        //{
                        //    continue;
                        //}

                        //Color penColour = allPathColours[colourIndex];
                        //Color penColour = allPathColours[path.Count - 1];
                        Pen pen = new Pen(penColour, 4f);

                        //colourIndex = (colourIndex + 1) % allPathColours.Length;

                        Point currentPoint = path[0];
                        for (int j = 1; j < path.Count; j++)
                        {
                            int currentX = currentPoint.X * blankImage.Width + blankImage.Width / 2;
                            int currentY = currentPoint.Y * blankImage.Height + blankImage.Height / 2;

                            Point nextPoint = path[j];
                            int nextX = nextPoint.X * blankImage.Width + blankImage.Width / 2;
                            int nextY = nextPoint.Y * blankImage.Height + blankImage.Height / 2;

                            graphics.DrawLine(pen, currentX, currentY, nextX, nextY);

                            currentPoint = nextPoint;
                        }
                    }
                }
            }

            imageOutdated = false;
        }

        private static int CompareByCountLongestToShortest(List<Point> a, List<Point> b)
        {
            return b.Count - a.Count;
        }

        private static int CompareByCountClosestToPerfect(List<Point> a, List<Point> b)
        {
            return (int)Math.Abs(perfectPathLength - b.Count) - (int)Math.Abs(perfectPathLength - a.Count);
        }

        private static List<List<Point>> GetPaths(int startX, int startY, int endX, int endY)
        {
            List<List<Point>> allPaths = new List<List<Point>>();

            int pathWidth = allTiles.GetLength(0);
            int pathHeight = 2 * allTiles.GetLength(1) - 1;
            bool[,] allTakenPaths = new bool[pathWidth, pathHeight];

            int pathsComplete = 0;
            int currentX = startX, currentY = startY;
            Point startPoint = new Point(currentX, currentY);
            allTakenPaths[currentX, currentY] = true;

            int maxY = allTiles.GetLength(1) - 1;

            if (currentX != endY)
            {
                List<Point> verticalPath = new List<Point> { startPoint };
                allPaths.Add(verticalPath);
            }

            if (currentY != endY)
            {
                List<Point> horizontalPath = new List<Point> { startPoint };
                allPaths.Add(horizontalPath);
            }

            do
            {
                foreach (List<Point> currentPath in allPaths.ToArray())
                {
                    //if (currentPath.Count > maxPathLength)
                    //{
                    //    allPaths.Remove(currentPath);
                    //    continue;
                    //}

                    Point currentPoint = currentPath[currentPath.Count - 1];
                    currentX = currentPoint.X;
                    currentY = currentPoint.Y;

                    if (currentX == endX && currentY == endY)
                    {
                        pathsComplete++;
                        if (pathsComplete == allPaths.Count)
                        {
                            break;
                        }

                        continue;
                    }

                    // Try to move horizontally first
                    int nextX = currentX;
                    if (currentX < endX)
                    {
                        nextX++;
                    }
                    if (currentX > endX)
                    {
                        nextX--;
                    }

                    List<Point> allNextPoints = new List<Point>();

                    // Check if next tile is valid
                    Tile nextTile = allTiles[nextX, currentY];
                    bool pathTaken = IsPathTaken(ref allTakenPaths, currentX, currentY, nextX, currentY);

                    if (nextTile.isBlocked == false && nextTile.isValid == true && nextX != currentX)
                    //if (nextTile.isBlocked == false && nextTile.isValid == true && nextX != currentX
                    //    && pathTaken == false)
                    {
                        allNextPoints.Add(new Point(nextX, currentY));
                    }

                    Point lastPoint = currentPoint;
                    if (currentPath.Count > 1)
                    {
                        lastPoint = currentPath[currentPath.Count - 2];
                    }

                    int deltaX = currentX - lastPoint.X;
                    int deltaY = currentY - lastPoint.Y;

                    int upY = currentY - 1;
                    int downY = currentY + 1;

                    bool success;

                    if (downY - endY <= endY - upY)
                    {
                        // Move Down
                        //if ((deltaX != 0 || deltaY > 0) && downY <= maxY)
                        //{
                        //    nextTile = allTiles[currentX, downY];
                        //    if (nextTile.isBlocked == false)
                        //    {
                        //        allNextPoints.Add(new Point(currentX, downY));
                        //    }
                        //}
                        if (deltaX != 0 || deltaY > 0)
                        {
                            success = AddPointIfPossible(ref allTakenPaths, ref allNextPoints, maxY, currentX, currentY, currentX, downY);
                        }

                        // Move Up
                        //if ((deltaX != 0 || deltaY < 0) && upY >= 0)
                        //{
                        //    nextTile = allTiles[currentX, upY];
                        //    if (nextTile.isBlocked == false)
                        //    {
                        //        allNextPoints.Add(new Point(currentX, upY));
                        //    }
                        //}
                        if (deltaX != 0 || deltaY < 0)
                        {
                            success = AddPointIfPossible(ref allTakenPaths, ref allNextPoints, maxY, currentX, currentY, currentX, upY);
                        }
                    }
                    else
                    {
                        // Move Up
                        if (deltaX != 0 || deltaY < 0)
                        {
                            success = AddPointIfPossible(ref allTakenPaths, ref allNextPoints, maxY, currentX, currentY, currentX, upY);
                        }

                        // Move Down
                        if (deltaX != 0 || deltaY > 0)
                        {
                            success = AddPointIfPossible(ref allTakenPaths, ref allNextPoints, maxY, currentX, currentY, currentX, downY);
                        }
                    }

                    if (allNextPoints.Count == 0)
                    {
                        // Dead end
                        allPaths.Remove(currentPath);
                        continue;
                    }

                    Point nextPoint = allNextPoints[0];
                    TakePath(ref allTakenPaths, currentPoint, nextPoint);

                    //continue;   //testing

                    // Branch if possible
                    if (allNextPoints.Count > 1)
                    {
                        for (int i = 1; i < allNextPoints.Count; i++)
                        {
                            Point branchPoint = allNextPoints[i];

                            List<Point> branchPath = new List<Point>(currentPath) { branchPoint };
                            allPaths.Add(branchPath);
                            TakePath(ref allTakenPaths, currentPoint, branchPoint);
                        }
                    }

                    // Add next point after branching
                    currentPath.Add(nextPoint);
                }
            } while (pathsComplete < allPaths.Count);

            return allPaths;
        }

        private static Point GetPath(Point point1, Point point2)
        {
            return GetPath(point1.X, point1.Y, point2.X, point2.Y);
        }

        private static Point GetPath(int x1, int y1, int x2, int y2)
        {
            int leftmostX = x1;

            // Assumed
            //if(x1 < x2)
            //{
            //    leftmostX = x1;
            //}

            if (x1 > x2)
            {
                leftmostX = x2;
            }

            if (y1 < y2)
            {
                leftmostX = x1;
            }

            if (y1 > y2)
            {
                leftmostX = x2;
            }

            int x = leftmostX;
            int y = y1 + y2;
            return new Point(x, y);
        }

        private static bool IsPathTaken(ref bool[,] allTakenPaths, int x1, int y1, int x2, int y2)
        {
            return IsPathTaken(ref allTakenPaths, new Point(x1, y1), new Point(x2, y2));
        }

        private static bool IsPathTaken(ref bool[,] allTakenPaths, Point point1, Point point2)
        {
            Point path = GetPath(point1, point2);

            bool pathTaken = allTakenPaths[path.X, path.Y];
            return pathTaken;
        }

        private static void TakePath(ref bool[,] allTakenPaths, Point point1, Point point2)
        {
            Point path = GetPath(point1, point2);

            allTakenPaths[path.X, path.Y] = true;
        }

        private static bool AddPointIfPossible(ref bool[,] allTakenPaths, ref List<Point> allNextPoints, int maxY, 
            int currentX, int currentY, int nextX, int nextY)
        {
            if (nextY > maxY)
            {
                return false;
            }

            if (nextY < 0)
            {
                return false;
            }

            Tile nextTile = allTiles[nextX, nextY];
            if (nextTile.isBlocked == true)
            {
                return false;
            }

            if (nextTile.isValid == false)
            {
                return false;
            }

            bool pathTaken = IsPathTaken(ref allTakenPaths, currentX, currentY, nextX, nextY);
            if (pathTaken == true)
            {
                return false;
            }

            allNextPoints.Add(new Point(nextX, nextY));
            return true;
        }

        public static Tile GetTileAtPosition(Point position)
        {
            int pixelX = position.X;
            int pixelY = position.Y;

            //pixelX = 20;    //testing
            //pixelY = 15;    //testing

            int x = pixelX / blankImage.Width;
            int y = pixelY / blankImage.Height;

            Tile tile = allTiles[x, y];
            return tile;
        }
    }
}
