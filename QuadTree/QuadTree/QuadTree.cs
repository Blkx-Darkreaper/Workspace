using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace QuadTree
{
    public class QuadTree
    {
        public Rectangle Bounds { get; protected set; }
        public NamedPoint Point { get; protected set; }
        public QuadTree NorthWest { get; protected set; }
        public QuadTree NorthEast { get; protected set; }
        public QuadTree SouthWest { get; protected set; }
        public QuadTree SouthEast { get; protected set; }

        public QuadTree(int x, int y, int length) : this(x, y, length, length) { }

        public QuadTree(int x, int y, int width, int height)
        {
            this.Bounds = new Rectangle(x, y, width, height);
            this.NorthWest = null;
            this.NorthEast = null;
            this.SouthWest = null;
            this.SouthEast = null;
            this.Point = null;
        }

        public override string ToString()
        {
            string output = JsonConvert.SerializeObject(this);
            return output;
        }

        public void Draw(Graphics graphics)
        {
            Pen pen = Pens.Black;

            graphics.DrawRectangle(pen, Bounds);

            if (Point != null) Point.Draw(graphics);

            if (NorthWest != null) NorthWest.Draw(graphics);
            if (NorthEast != null) NorthEast.Draw(graphics);
            if (SouthWest != null) SouthWest.Draw(graphics);
            if (SouthEast != null) SouthEast.Draw(graphics);
        }

        public bool Insert(NamedPoint point)
        {
            var pointInBounds = PointWithinSquare(point);
            if (pointInBounds == false)
            {
                return false;
            }

            // If not point has been added yet
            if (this.Point == null && this.NorthWest == null)
            {
                this.Point = point;
                return true;
            }

            Stack<NamedPoint> stack = new Stack<NamedPoint>();
            stack.Push(point);

            // Otherwise subdivide
            if (this.NorthWest == null)
            {
                stack.Push(this.Point);
                this.Point = null;

                SubDivide();
            }

            // Redistribute points
            while (stack.Count > 0)
            {
                NamedPoint nextPoint = stack.Pop();
                if (this.NorthWest.Insert(nextPoint) == true)
                {
                    continue;
                }
                if (this.NorthEast.Insert(nextPoint) == true)
                {
                    continue;
                }
                if (this.SouthWest.Insert(nextPoint) == true)
                {
                    continue;
                }
                if (this.SouthEast.Insert(nextPoint) == true)
                {
                    continue;
                }

                // An error has occured
                throw new InvalidOperationException();
                //return false;
            }

            return true;
        }

        public void SubDivide()
        {
            int x = this.Bounds.X;
            int y = this.Bounds.Y;
            int length = this.Bounds.Width;
            int halfLength = length / 2;
            int remainder = length - halfLength;

            this.NorthWest = new QuadTree(x, y, halfLength);
            this.NorthEast = new QuadTree(x + halfLength, y, remainder, halfLength);
            this.SouthWest = new QuadTree(x, y + halfLength, halfLength, remainder);
            this.SouthEast = new QuadTree(x + halfLength, y + halfLength, remainder);
        }

        public List<NamedPoint> GetPointsInArea(Rectangle area)
        {
            List<NamedPoint> allPoints = new List<NamedPoint>();

            if (this.Bounds.IntersectsWith(area) == false)
            {
                return allPoints;
            }

            if (Point != null)
            {
                bool pointInArea = Program.PointWithinSquare(area, Point);
                if (pointInArea == true) allPoints.Add(Point);
            }

            if (NorthWest == null)
            {
                return allPoints;
            }

            allPoints.AddRange(NorthWest.GetPointsInArea(area));
            allPoints.AddRange(NorthEast.GetPointsInArea(area));
            allPoints.AddRange(SouthWest.GetPointsInArea(area));
            allPoints.AddRange(SouthEast.GetPointsInArea(area));

            return allPoints;
        }

        public bool PointWithinSquare(NamedPoint point)
        {
            return Program.PointWithinRectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height, point);
        }
    }
}
