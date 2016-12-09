using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NachosCrazyTaxiService
{
    class Entity
    {
        public Point center { get; protected set; }
        public Size dimensions { get; protected set; }
        protected Color lineColour { get; set; }
        protected int lineThickness { get; set; }
        protected Wall[] allWalls { get; set; }
        public static string[] DIRECTIONS = new string[] { "North", "East", "South", "West" };

        protected struct Wall
        {
            public enum directions { NORTH, EAST, SOUTH, WEST };
            public int directionIndex { get; set; }
            public bool damaged { get; set; }
        }

        public Entity(Point center, Size dimensions, Color lineColour, int lineThickness)
        {
            this.center = center;
            this.dimensions = dimensions;
            this.lineColour = lineColour;
            this.lineThickness = lineThickness;

            allWalls = new Wall[4];
            for (int i = 0; i < 4; i++)
            {
                InitWall(i, i);
            }
        }

        public Entity(int centerX, int centerY, Size dimensions, Color lineColour, int lineThickness)
            : this(new Point(centerX, centerY), dimensions, lineColour, lineThickness) { }

        public Entity(Point center, int width, int height, Color lineColour, int lineThickness) : this(center, new Size(width, height), lineColour, lineThickness) { }

        public Entity(int centerX, int centerY, int width, int height, Color lineColour, int lineThickness)
            : this(new Point(centerX, centerY), new Size(width, height), lineColour, lineThickness) { }

        protected void InitWall(int index, int directionIndex)
        {
            allWalls[index].directionIndex = directionIndex;
            allWalls[index].damaged = false;
        }

        public void Draw(Graphics graphics)
        {
            Point[] allPoints = GetDrawPoints();

            Pen pen = new Pen(lineColour, lineThickness);
            graphics.DrawLines(pen, allPoints);
        }

        protected Point[] GetDrawPoints()
        {
            int cornerX = center.X - dimensions.Width / 2;
            int cornerY = center.Y - dimensions.Height / 2;
            Point corner = new Point(cornerX, cornerY);

            List<Point> allPoints = new List<Point>();
            allPoints.Add(corner);

            Point nextPoint = corner;
            for (int i = 0; i < 4; i++)
            {
                Wall wall = allWalls[i];
                Point nextEndPoint = GetNextWallPoint(corner, i);

                if (wall.damaged == false)
                {
                    allPoints.Add(nextEndPoint);
                    continue;
                }

                int buffer = dimensions.Width / 10;
                if (i % 2 == 1)
                {
                    buffer = dimensions.Height / 10;
                }

                if (buffer == 0)
                {
                    buffer = 1;
                }

                nextPoint = GetNextWallPoint(corner, i, buffer, buffer);
                allPoints.Add(nextPoint);

                Point endBreakPoint = GetNextWallPoint(corner, i, 9 * buffer, 9 * buffer);
                int modifier = 3;
                while (!nextPoint.Equals(endBreakPoint))
                {
                    switch (i)
                    {
                        case 0:
                            nextPoint = new Point(nextPoint.X + buffer, nextPoint.Y + (modifier * buffer / 2));
                            allPoints.Add(nextPoint);

                            nextPoint = new Point(nextPoint.X + buffer, nextPoint.Y - (modifier * buffer / 2));
                            allPoints.Add(nextPoint);
                            break;

                        case 1:
                            nextPoint = new Point(nextPoint.X + (modifier * buffer / 2), nextPoint.Y + buffer);
                            allPoints.Add(nextPoint);

                            nextPoint = new Point(nextPoint.X - (modifier * buffer / 2), nextPoint.Y + buffer);
                            allPoints.Add(nextPoint);
                            break;
                    }

                    modifier = Math.Abs(modifier - 1);
                }

                allPoints.Add(nextEndPoint);
            }

            return allPoints.ToArray();
        }

        protected Point GetNextWallPoint(Point corner, int wallIndex)
        {
            int distanceX = 0;
            int distanceY = 0;
            switch (wallIndex)
            {
                case 0:
                    distanceX = dimensions.Width;
                    break;

                case 1:
                    distanceX = dimensions.Width;
                    distanceY = dimensions.Height;
                    break;

                case 2:
                    distanceY = dimensions.Height;
                    break;
            }

            return GetNextWallPoint(corner, wallIndex, distanceX, distanceY);
        }

        protected Point GetNextWallPoint(Point corner, int wallIndex, int distanceX, int distanceY)
        {
            Point endPoint;
            switch (wallIndex)
            {
                case 0:
                    endPoint = new Point(corner.X + distanceX, corner.Y);
                    break;

                case 1:
                    endPoint = new Point(corner.X + distanceX, corner.Y + distanceY);
                    break;

                case 2:
                    endPoint = new Point(corner.X, corner.Y + distanceY);
                    break;

                case 3:
                default:
                    endPoint = corner;
                    break;
            }

            return endPoint;
        }

        public void TakeDamage(int damage, int directionIndex)
        {
            allWalls[directionIndex].damaged = true;
        }
    }
}