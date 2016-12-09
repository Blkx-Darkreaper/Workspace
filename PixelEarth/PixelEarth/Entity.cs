using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PixelEarth
{
    public class Entity
    {
        public Point location { get; protected set; }
        public int size { get; set; }
        public Shape shape { get; set; }
        public enum Shape { dot, line, circle, square };
        public static double metersPerPixel = 5;
        protected Color mainColour { get; set; }
        public int heading { get; set; }
        public Queue<Point> wayPoints { get; set; }
        public bool visible { get; set; }

        public Entity(Point location, int size, Color mainColour)
        {
            Init(location, size, mainColour);
        }

        public Entity(int x, int y, int size, Color mainColour) : this(new Point(x, y), size, mainColour) { }

        protected virtual void Init(Point location, int size, Color mainColour)
        {
            this.location = location;
            this.size = size;
            this.mainColour = mainColour;
            this.heading = 0;
            this.wayPoints = new Queue<Point>();
            this.visible = true;
        }

        public virtual Point GetDrawLocation()
        {
            return GetDrawLocation(location);
        }

        protected virtual Point GetDrawLocation(Point location)
        {
            int drawX = (int)Math.Round(location.X / metersPerPixel, 0);
            int drawY = (int)Math.Round(location.Y / metersPerPixel, 0);
            Point drawLocation = new Point(drawX, drawY);
            return drawLocation;
        }

        public virtual void DrawDot(Graphics graphics, Color colour) {
            SolidBrush brush = new SolidBrush(colour);

            //Point drawLocation = GetDrawLocation();
            Point drawLocation = location;

            graphics.FillRectangle(brush, drawLocation.X, drawLocation.Y, 1, 1);
        }

        public virtual void DrawLine(Graphics graphics, Color colour, int pixelLength)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            Point lineStart = GetDrawLocation();

            int tailX = lineStart.X + (int)Math.Round(Program.Sin(heading) * pixelLength, 0);
            int tailY = lineStart.Y + (int)Math.Round(Program.Cos(heading) * pixelLength, 0);

            Point lineEnd = new Point(tailX, tailY);

            graphics.DrawLine(pen, lineStart, lineEnd);
        }

        public virtual void DrawCircle(Graphics graphics, Color colour, int radius)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            Rectangle bounds = GetDrawBounds();

            graphics.DrawEllipse(pen, bounds);
        }

        public virtual void DrawSquare(Graphics graphics, Color colour, int pixelLength)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            List<Point> allPoints = new List<Point>();

            Point drawLocation = GetDrawLocation();

            int cornerX, cornerY;
            Point rotatedCorner;

            cornerX = drawLocation.X - pixelLength / 2;
            cornerY = drawLocation.Y - pixelLength / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, drawLocation);
            allPoints.Add(rotatedCorner);

            Point copy = new Point(rotatedCorner.X, rotatedCorner.Y);

            cornerX = drawLocation.X + pixelLength / 2;
            //cornerY = drawLocation.Y - pixelLength / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, drawLocation);
            allPoints.Add(rotatedCorner);

            //cornerX = drawLocation.X + pixelLength / 2;
            cornerY = drawLocation.Y + pixelLength / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, drawLocation);
            allPoints.Add(rotatedCorner);

            cornerX = drawLocation.X - pixelLength / 2;
            //cornerY = drawLocation.Y + pixelLength / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, drawLocation);
            allPoints.Add(rotatedCorner);

            allPoints.Add(copy);

            graphics.DrawPolygon(pen, allPoints.ToArray());
        }

        public virtual void Draw(Graphics graphics)
        {
            //int pixelSize = (int)Math.Round(size / metersPerPixel,0);
            //DrawSquare(graphics, mainColour, pixelSize);

            DrawDot(graphics, mainColour);
        }

        public virtual void Update(float hoursElapsed)
        {
            int initialX = location.X;
            int initialY = location.Y;

            HeadToWayPoint();
        }

        public virtual void AddWayPoint(Point wayPoint)
        {
            wayPoints.Enqueue(wayPoint);
        }

        public virtual void WayPointReached(Point currentLocation, int threshold=5)
        {
            Point nextWayPoint = wayPoints.Peek();
            if (nextWayPoint == null)
            {
                return;
            }

            int distance = (int)Math.Round(Program.GetDistance(currentLocation, nextWayPoint), 0);
            if (distance > threshold)
            {
                return;
            }

            wayPoints.Dequeue();
        }

        protected virtual void HeadToWayPoint()
        {
            if (wayPoints == null)
            {
                return;
            }
            if (wayPoints.Count == 0)
            {
                return;
            }

            WayPointReached(location);

            Point nextWayPoint = wayPoints.Peek();
            if (nextWayPoint == null)
            {
                return;
            }

            int absBearing = (int)Math.Round(Program.GetAbsAngle(location, nextWayPoint), 0);

            int headingChange = absBearing - heading;

            Turn(headingChange);
        }

        public virtual void TurnLeft()
        {
            TurnLeft(1);
        }

        public virtual void TurnLeft(int amount) {
            Turn(-amount);
        }

        public virtual void TurnRight() {
            TurnRight(1);
        }

        public virtual void TurnRight(int amount)
        {
            Turn(amount);
        }

        public virtual void Turn(int amount)
        {
            heading += amount;
            heading %= 360;
        }

        public virtual void TurnTowardsPoint(Point point, float elapsedTime)
        {
            double desiredHeading = Program.GetAbsAngle(location, point);

            double changeOfHeading1 = desiredHeading - heading;
            double changeOfHeading2 = heading - desiredHeading;

            double changeOfHeading;
            if (Math.Abs(changeOfHeading1) <= Math.Abs(changeOfHeading2))
            {
                changeOfHeading = changeOfHeading1;
            }
            else
            {
                changeOfHeading = changeOfHeading2;
            }

            Turn((int)changeOfHeading);
        }

        public virtual Rectangle GetDrawBounds()
        {
            int drawSize = (int)Math.Round(size / metersPerPixel, 0);
            return GetDrawBounds(drawSize, drawSize);
        }

        public virtual Rectangle GetDrawBounds(int width, int height)
        {
            Point drawLocation = GetDrawLocation();
            int drawWidth = (int)Math.Round(width / metersPerPixel, 0);
            int drawHeight = (int)Math.Round(height / metersPerPixel, 0);

            Rectangle bounds = new Rectangle(drawLocation.X - drawWidth / 2, drawLocation.Y - drawHeight / 2, drawWidth, drawHeight);
            return bounds;
        }

        public virtual Rectangle GetBounds()
        {
            return GetBounds(size, size);
        }

        public virtual Rectangle GetBounds(int width, int height)
        {
            Rectangle bounds = new Rectangle(location.X - width / 2, location.Y - height / 2, width, height);
            return bounds;
        }

        public virtual bool DetectCollision(Entity other)
        {
            Rectangle bounds = GetBounds();
            int radius = bounds.Width / 2;

            Rectangle otherBounds = other.GetDrawBounds();
            int otherRadius = bounds.Width / 2;

            double distance = Program.GetDistance(location, other.location);
            int minDistance = radius + otherRadius;

            if (distance > minDistance)
            {
                return false;
            }

            return true;
        }
    }
}
