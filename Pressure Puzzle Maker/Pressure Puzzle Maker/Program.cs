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

        public static bool perfectPathOnly = false;
        private static int avgPathLength = 12;
        private static int longPathLength = avgPathLength + offset;
        private static int shortPathLength = avgPathLength - offset;
        private static int offset = 1;

        private static int maxPathLength = longPathLength + allPathColours.Length - 1 - longColourIndex;

        private static Color tooLongColour = Color.Purple;
        private static Color veryLongColour = Color.Indigo;
        private static Color longerColour = Color.Blue;

        private static Color longColour = Color.Turquoise;
        private static int longColourIndex = 5;

        private static Color avgColour = Color.Green;
        //private static int avgColourIndex = 4;

        private static Color shortColour = Color.YellowGreen;
        private static int shortColourIndex = 3;

        private static Color shorterColour = Color.Orange;
        private static Color veryShortColour = Color.Red;
        private static Color tooShortColour = Color.DarkRed;

        private static Color[] allPathColours = { tooShortColour, veryShortColour, shorterColour, shortColour, avgColour,
            longColour, longerColour, veryLongColour, tooLongColour};

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

        public static void SetPerfectPathLength(int maxPolyominoes)
        {
            shortPathLength = 2 * maxPolyominoes;
            longPathLength = 3 * maxPolyominoes;
            avgPathLength = (shortPathLength + longPathLength) / 2;

            maxPathLength = longPathLength + allPathColours.Length - 1 - longColourIndex;
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

                // Sort start tiles from left to right
                allStartTiles.Sort(CompareByLeftmostX);

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

        private static int CompareByLeftmostX(Tile a, Tile b)
        {
            return a.position.X - b.position.Y;
        }

        private static int CompareByCountLongestToShortest(Point[] a, Point[] b)
        {
            return b.Length - a.Length;
        }

        private static int CompareByCountClosestToPerfect(Point[] a, Point[] b)
        {
            return (int)Math.Abs(avgPathLength - b.Length) - (int)Math.Abs(avgPathLength - a.Length);
        }

        private static void DrawPath(Graphics graphics, Point[] path)
        {
            int pathLength = path.Length;
            if (pathLength < 2)
            {
                return;
            }

            if(perfectPathOnly == true)
            {
                if(pathLength < shortPathLength)
                {
                    return;
                }

                if(pathLength > longPathLength)
                {
                    return;
                }
            }

            if(pathLength < shortPathLength - shortColourIndex)
            {
                return;
            }

            if (pathLength > maxPathLength)
            {
                return;
            }

            Color penColour = avgColour;

            if (pathLength == longPathLength)
            {
                penColour = longColour;
            }
            if (pathLength > longPathLength)
            {
                int diff = pathLength - longPathLength;
                int index = Clamp(longColourIndex + diff, 0, allPathColours.Length - 1);

                penColour = allPathColours[index];
            }

            if (pathLength == shortPathLength)
            {
                penColour = shorterColour;
            }
            if(pathLength < shortPathLength)
            {
                int diff = shortPathLength - pathLength;
                int index = Clamp(shortColourIndex - diff, 0, allPathColours.Length - 1);

                penColour = allPathColours[index];
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
                FollowPaths(ref allPaths, ref allCompletePaths, ref allEndingPoints, graphics, endX, endY, maxX, maxY);
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
                        else
                        {
                            upPathBlocked = true;
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
                        else
                        {
                            downPathBlocked = true;
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
            if (endX == maxX)
            {
                directionX = 1;
            }

            // All paths move left
            if (endX == 0)
            {
                directionX = -1;
            }

            int directionY = 0;
            // All paths move down
            if (endY == maxY)
            {
                directionY = 1;
            }

            // All paths move up
            if (endY == 0)
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
            List<Point> allEndingPoints = new List<Point>();

            // Determine which 3 of 4 points are valid
            // Top point
            int topY = endY - 1;
            if (topY >= 0)
            {
                Tile endTile = allTiles[endX, topY];
                if (endTile.isBlocked == false && endTile.isValid == true)
                {
                    allEndingPoints.Add(new Point(endX, topY));
                }
            }

            // Right point
            int rightX = endX + 1;
            if (rightX <= maxX)
            {
                Tile endTile = allTiles[rightX, endY];
                if (endTile.isBlocked == false && endTile.isValid == true)
                {
                    allEndingPoints.Add(new Point(rightX, endY));
                }
            }

            // Bottom point
            int bottomY = endY + 1;
            if (bottomY <= maxY)
            {
                Tile endTile = allTiles[endX, bottomY];
                if (endTile.isBlocked == false && endTile.isValid == true)
                {
                    allEndingPoints.Add(new Point(endX, bottomY));
                }
            }

            // Left point
            int leftX = endX - 1;
            if (leftX >= 0)
            {
                Tile endTile = allTiles[leftX, endY];
                if (endTile.isBlocked == false && endTile.isValid == true)
                {
                    allEndingPoints.Add(new Point(leftX, endY));
                }
            }

            return allEndingPoints.ToArray();
        }

        private static void FollowPaths(ref List<List<Point>> allPaths, ref List<Point[]> allCompletePaths, ref Point[] allEndingPoints,
            Graphics graphics, int endX, int endY, int maxX, int maxY)
        {
            foreach (List<Point> currentPath in allPaths.ToArray())
            {
                if (currentPath.Count > maxPathLength)
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
                    int endPointX = endPoint.X;
                    int endPointY = endPoint.Y;

                    if (currentX != endPointX || currentY != endPointY)
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

                if (pathComplete == true)
                {
                    continue;
                }

                Point lastPoint = currentPoint;
                if (currentPath.Count > 1)
                {
                    lastPoint = currentPath[currentPath.Count - 2];
                }

                // Determine previously moved direction
                int lastX = lastPoint.X;
                int lastY = lastPoint.Y;

                //int lastDirectionX = 0;
                int lastDirectionY = 0;

                //// Moved right
                //if(currentX > lastX)
                //{
                //    lastDirectionX = 1;
                //}

                //// Moved left
                //if(currentX < lastX)
                //{
                //    lastDirectionX = -1;
                //}

                // Moved up
                if(currentY < lastY)
                {
                    lastDirectionY = -1;
                }

                // Moved down
                if(currentY > lastY)
                {
                    lastDirectionY = 1;
                }

                // Determine direction of end point
                int endDirectionX = 0;
                //int endDirectionY = 0;

                // Move right
                if (endX == maxX)
                {
                    endDirectionX = 1;
                }

                // Move left
                if (endX == 0)
                {
                    endDirectionX = -1;
                }

                //// Move down
                //if (endY == maxY)
                //{
                //    endDirectionY = 1;
                //}

                //// Move up
                //if (endY == 0)
                //{
                //    endDirectionY = -1;
                //}

                // Determine which 3 of 4 points are valid
                Tile nextTile;
                List<Point> allNextPoints = new List<Point>();

                // Move horizontally first
                if (endDirectionX > 0)
                {
                    // Right point
                    int rightX = currentX + 1;
                    if (rightX <= maxX)
                    {
                        nextTile = allTiles[rightX, currentY];
                        if (nextTile.isBlocked == false && nextTile.isValid == true)
                        {
                            allNextPoints.Add(new Point(rightX, currentY));
                        }
                    }
                }
                else
                {
                    // Left point
                    int leftX = currentX - 1;
                    if (leftX >= 0)
                    {
                        nextTile = allTiles[leftX, currentY];
                        if (nextTile.isBlocked == false && nextTile.isValid == true)
                        {
                            allNextPoints.Add(new Point(leftX, currentY));
                        }
                    }
                }

                // Top point
                int topY = currentY - 1;
                if (topY >= 0 && lastDirectionY < 1)   // Keep from backtracking
                {
                    nextTile = allTiles[currentX, topY];
                    if (nextTile.isBlocked == false && nextTile.isValid == true)
                    {
                        allNextPoints.Add(new Point(currentX, topY));
                    }
                }

                // Bottom point
                int bottomY = currentY + 1;
                if (bottomY <= maxY && lastDirectionY > -1)   // Keep from backtracking
                {
                    nextTile = allTiles[currentX, bottomY];
                    if (nextTile.isBlocked == false && nextTile.isValid == true)
                    {
                        allNextPoints.Add(new Point(currentX, bottomY));
                    }
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
                    for (int i = 1; i < allNextPoints.Count; i++)
                    {
                        Point branchPoint = allNextPoints[i];

                        List<Point> branchPath = new List<Point>(currentPath) { branchPoint };
                        allPaths.Add(branchPath);
                    }
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
