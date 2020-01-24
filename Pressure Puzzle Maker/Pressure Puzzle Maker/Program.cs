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

        private static int perfectPathLength = 12;
        private static int maxOffset = 1;
        private static int longPathLength = perfectPathLength + maxOffset;
        private static int shortPathLength = perfectPathLength - maxOffset;
        private static int veryShortPathLength = perfectPathLength - maxOffset * 2;
        private static int veryLongPathLength = perfectPathLength + maxOffset * 2;

        private static Color tooLongColour = Color.DarkViolet;
        private static Color veryLongPathColour = Color.Violet;
        private static Color longPathColour = Color.Blue;
        private static Color perfectPathColour = Color.YellowGreen;
        private static Color shortPathColour = Color.Orange;
        private static Color veryShortPathColour = Color.Red;
        private static Color tooShortColour = Color.DarkRed;

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

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
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
                    int maxX = allTiles.GetLength(0) - 1;
                    int maxY = allTiles.GetLength(1) - 1;

                    List<Point[]> allPaths = GetPaths(graphics, startX, startY, endX, endY, maxX, maxY);
                    allPaths.Sort(CompareByCountClosestToPerfect);

                    foreach (Point[] path in allPaths)
                    {
                        DrawPath(graphics, path);
                    }
                }
            }

            imageOutdated = false;
        }

        private static int CompareByCountLongestToShortest(Point[] a, Point[] b)
        {
            return b.Length - a.Length;
        }

        private static int CompareByCountClosestToPerfect(Point[] a, Point[] b)
        {
            return (int)Math.Abs(perfectPathLength - b.Length) - (int)Math.Abs(perfectPathLength - a.Length);
        }

        private static void DrawPath(Graphics graphics, Point[] path)
        {
            int pathLength = path.Length;
            if (pathLength < 2)
            {
                return;
            }

            if(pathLength > 2 * perfectPathLength)
            {
                return;
            }

            //if (pathLength > veryLongPathLength)
            //{
            //    return;
            //}
            //if(pathLength < veryShortPathLength)
            //{
            //    return;
            //}

            Color penColour = perfectPathColour;

            if (pathLength > perfectPathLength)
            {
                penColour = longPathColour;
            }
            if (pathLength > longPathLength)
            {
                penColour = veryLongPathColour;
            }
            if(pathLength > veryLongPathLength)
            {
                penColour = tooLongColour;
            }

            if (pathLength < perfectPathLength)
            {
                penColour = shortPathColour;
            }
            if (pathLength < shortPathLength)
            {
                penColour = veryShortPathColour;
            }
            if(pathLength < veryShortPathLength)
            {
                penColour = tooShortColour;
            }

            Pen pen = new Pen(penColour, 4f);

            Point currentPoint = path[0];
            for (int j = 1; j < pathLength; j++)
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

        private static List<Point[]> GetPaths(Graphics graphics, int startX, int startY, int endX, int endY, int maxX, int maxY)
        {
            List<List<Point>> allPaths = new List<List<Point>>();
            List<Point[]> allCompletePaths = new List<Point[]>();

            Point[] allEndingPoints = PreparePaths(allPaths, startX, startY, endX, endY, maxX, maxY);

            do
            {
                FollowPaths(ref allPaths, ref allCompletePaths, ref allEndingPoints, graphics, endX, endY);
            } while (allPaths.Count > 0);

            return allCompletePaths;
        }

        private static Point[] PreparePaths(List<List<Point>> allPaths, int startX, int startY, int endX, int endY, int maxX, int maxY)
        {
            int currentX = startX, currentY = startY;

            // Determine which edge we're starting on and move out one space
            // Left edge
            if (startX == 0)
            {
                currentX++;
            }

            // Top edge
            if (startY == 0)
            {
                currentY++;
            }

            // Right edge
            if (startX == maxX)
            {
                currentX--;
            }

            // Bottom edge
            if (startY == maxY)
            {
                currentY--;
            }

            Point startPoint = new Point(currentX, currentY);

            // Prepare starting paths
            allPaths.Add(new List<Point> { startPoint });   // Middle path

            // Branch vertically if ending is on a side edge
            if (endX == 0 || endX == maxX)
            {
                List<Point> upPath = new List<Point> { startPoint };
                bool upPathBlocked = false;

                List<Point> downPath = new List<Point> { startPoint };
                bool downPathBlocked = false;

                int maxOffset = Math.Max(currentY, maxY - currentY);
                for (int i = 1; i <= maxOffset; i++)
                {
                    if (upPathBlocked == false)
                    {
                        int nextUpY = currentY - i;
                        if (nextUpY >= 0)
                        {
                            Tile nextUpTile = allTiles[currentX, nextUpY];
                            if (nextUpTile.isBlocked == false && nextUpTile.isValid == true)
                            {
                                Point nextUpPoint = new Point(currentX, nextUpY);
                                upPath.Add(nextUpPoint);

                                allPaths.Add(new List<Point>(upPath));
                            }
                            else
                            {
                                upPathBlocked = true;
                            }
                        }
                    }

                    if (downPathBlocked == false)
                    {
                        int nextDownY = currentY + i;
                        if (nextDownY <= maxY)
                        {
                            Tile nextDownTile = allTiles[currentX, nextDownY];
                            if (nextDownTile.isBlocked == false && nextDownTile.isValid == true)
                            {
                                Point nextDownPoint = new Point(currentX, nextDownY);
                                downPath.Add(nextDownPoint);

                                allPaths.Add(new List<Point>(downPath));
                            }
                            else
                            {
                                downPathBlocked = true;
                            }
                        }
                    }
                }
            }

            // Branch horizontally if ending is on a top or bottom edge
            if (endY == 0 || endY == maxY)
            {
                List<Point> rightPath = new List<Point> { startPoint };
                List<Point> leftPath = new List<Point> { startPoint };

                int maxOffset = Math.Max(currentX, maxX - currentX);
                for (int i = 1; i <= maxOffset; i++)
                {
                    int nextRightX = currentX + i;
                    if (nextRightX <= maxX)
                    {
                        Tile nextRightTile = allTiles[nextRightX, currentY];
                        if (nextRightTile.isBlocked == false && nextRightTile.isValid == true)
                        {
                            Point nextRightPoint = new Point(nextRightX, currentY);
                            rightPath.Add(nextRightPoint);

                            allPaths.Add(new List<Point>(rightPath));
                        }
                    }

                    int nextLeftX = currentX - i;
                    if (nextLeftX >= 0)
                    {
                        Tile nextLeftTile = allTiles[nextLeftX, currentY];
                        if (nextLeftTile.isBlocked == false && nextLeftTile.isValid == true)
                        {
                            Point nextLeftPoint = new Point(nextLeftX, currentY);
                            leftPath.Add(nextLeftPoint);

                            allPaths.Add(new List<Point>(leftPath));
                        }
                    }
                }
            }

            // Take next steps
            // All paths move right
            int directionX = 0;
            if (startX == 0)
            {
                directionX = 1;
            }

            // All paths move left
            if (startX == maxX)
            {
                directionX = -1;
            }

            int directionY = 0;
            // All paths move down
            if(startY == 0)
            {
                directionY = 1;
            }

            // All paths move up
            if(startY == maxY)
            {
                directionY = -1;
            }

            foreach (List<Point> currentPath in allPaths.ToArray())
            {
                Point currentPoint = currentPath[currentPath.Count - 1];
                int nextX = Clamp(currentPoint.X + directionX, 0, maxX);
                int nextY = Clamp(currentPoint.Y + directionY, 0, maxY);

                // Check if next tile is valid
                Tile nextTile = allTiles[nextX, nextY];
                if (nextTile.isBlocked == true || nextTile.isValid == false)
                {
                    // Dead end
                    allPaths.Remove(currentPath);
                    continue;
                }

                Point nextPoint = new Point(nextX, nextY);
                currentPath.Add(nextPoint);
            }

            // Prepare ending points
            Point[] allEndingPoints = new Point[3];
            int nextIndex = 0;

            // Determine which 3 of 4 points are valid
            // Top point
            if (endY - 1 >= 0)
            {
                allEndingPoints[nextIndex] = new Point(endX, endY - 1);
                nextIndex++;
            }

            // Right point
            if (endX + 1 <= maxX)
            {
                allEndingPoints[nextIndex] = new Point(endX + 1, endY);
                nextIndex++;
            }

            // Bottom point
            if (endY + 1 <= maxY)
            {
                allEndingPoints[nextIndex] = new Point(endX, endY + 1);
                nextIndex++;
            }

            // Left point
            if (endX - 1 >= 0)
            {
                allEndingPoints[nextIndex] = new Point(endX - 1, endY);
            }

            return allEndingPoints;
        }

        private static void FollowPaths(ref List<List<Point>> allPaths, ref List<Point[]> allCompletePaths, ref Point[] allEndingPoints, 
            Graphics graphics, int endX, int endY)
        {
            foreach (List<Point> currentPath in allPaths.ToArray())
            {
                if (currentPath.Count > 2 * perfectPathLength)
                {
                    allPaths.Remove(currentPath);
                    continue;
                }

                //DrawPath(graphics, currentPath);

                Point currentPoint = currentPath[currentPath.Count - 1];
                int currentX = currentPoint.X;
                int currentY = currentPoint.Y;

                // Check if we've reached one of the three end points
                bool pathComplete = false;
                foreach (Point endPoint in allEndingPoints)
                {
                    int lastX = endPoint.X;
                    int lastY = endPoint.Y;

                    if (currentX != lastX || currentY != lastY)
                    {
                        continue;
                    }

                    // Move path to completed paths
                    allCompletePaths.Add(currentPath.ToArray());
                    allPaths.Remove(currentPath);
                    pathComplete = true;

                    if (allPaths.Count > 0)
                    {
                        break;
                    }

                    // All paths complete
                    return;
                }

                if(pathComplete == true)
                {
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
                if (nextTile.isBlocked == false && nextTile.isValid == true && nextX != currentX)
                {
                    allNextPoints.Add(new Point(nextX, currentY));
                }

                // Move vertically toward end point
                int nextY = currentY;
                if (currentY < endY)
                {
                    nextY++;
                }
                if (currentY > endY)
                {
                    nextY--;
                }

                // Check if next tile is valid
                nextTile = allTiles[currentX, nextY];
                if (nextTile.isBlocked == false && nextTile.isValid == true && nextY != currentY)
                {
                    allNextPoints.Add(new Point(currentX, nextY));
                }

                if (allNextPoints.Count == 0)
                {
                    // Dead end
                    allPaths.Remove(currentPath);
                    continue;
                }

                Point nextPoint = allNextPoints[0];

                //continue;   //testing

                // Branch if possible first so next point isn't added
                if (allNextPoints.Count > 1)
                {
                    Point branchPoint = allNextPoints[1];

                    List<Point> branchPath = new List<Point>(currentPath) { branchPoint };
                    allPaths.Add(branchPath);
                }

                // Add next point after branching
                currentPath.Add(nextPoint);
            }
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
