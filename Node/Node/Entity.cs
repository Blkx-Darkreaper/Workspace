using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Node
{
    public class Entity
    {
        public Point location { get; protected set; }
        public int size { get; set; }
        public Shape shape { get; set; }
        public enum Shape { dot, line, circle, square };
        protected double metersPerPixel = 5;
        protected Color mainColour { get; set; }
        protected Color backgroundColour { get; set; }
        protected bool drawBackground { get; set; }
        public int velocity { get; set; }
        public int maxVelocity { get; set; }
        public int heading { get; set; }
        public int turnSpeed { get; set; }
        public Queue<Point> wayPoints { get; set; }
        public bool visible { get; set; }
        public bool orbiting { get; set; }
        public bool inOrbit { get; set; }
        public bool orbitingClockwise { get; set; }
        public bool dying { get; set; }
        public float lifetime { get; set; }

        public Entity(Point location, int size, Color mainColour, Color backgroundColour)
        {
            Init(location, size, mainColour);
            this.drawBackground = true;
            this.backgroundColour = backgroundColour;
        }

        public Entity(int x, int y, int size, Color mainColour, Color backgroundColour) : this(new Point(x, y), size, mainColour, backgroundColour) { }

        public Entity(Point location, int size, Color mainColour)
        {
            Init(location, size, mainColour);
            this.drawBackground = false;
        }

        public Entity(int x, int y, int size, Color mainColour) : this(new Point(x, y), size, mainColour) { }

        protected virtual void Init(Point location, int size, Color mainColour)
        {
            this.location = location;
            this.size = size;
            this.mainColour = mainColour;
            this.velocity = 0;
            this.maxVelocity = 100;
            this.heading = 0;
            this.turnSpeed = 1;
            this.wayPoints = new Queue<Point>();
            this.visible = true;
            this.orbiting = false;
            this.inOrbit = false;
            this.orbitingClockwise = true;
            dying = false;
            lifetime = 0f;
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

            Point drawLocation = GetDrawLocation();

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

        public virtual void DrawLayer0(Graphics graphics)
        {
            if (drawBackground == false)
            {
                return;
            }

            DrawDot(graphics, backgroundColour);
        }

        public virtual void DrawLayer1(Graphics graphics)
        {
            
        }

        public virtual void DrawLayer2(Graphics graphics)
        {
            DrawDot(graphics, mainColour);
        }

        public virtual void Update(float timeElapsed)
        {
            int initialX = location.X;
            int initialY = location.Y;

            HeadToWayPoint();

            //int finalX = initialX + (int)Math.Round(Program.Sin(heading) * velocity * timeElapsed, 0);
            //int finalY = initialY - (int)Math.Round(Program.Cos(heading) * velocity * timeElapsed, 0);
            int finalX = initialX + (int)Math.Round(Program.Sin(heading) * velocity, 0);
            int finalY = initialY - (int)Math.Round(Program.Cos(heading) * velocity, 0);

            location = new Point(finalX, finalY);
        }

        public virtual Entity[] Explode()
        {
            List<Entity> fragments = new List<Entity>();
            Random random = new Random();

            // Create a new dying entity for each pixel with random heading and lifetime and add to list

            return fragments.ToArray();
        }

        public virtual void Orbit(Point center, int maxRadius, float timeElapsed)
        {
            EnterOrbit(ref center, maxRadius);
            if (inOrbit == false)
            {
                return;
            }

            double arcLength = velocity * timeElapsed;

            int initialX = location.X;
            int initialY = location.Y;

            int centerX = center.X;
            int centerY = center.Y;

            double orbitRadius = maxRadius - (velocity - 1) / 2;

            double deltaAngle = 180 * arcLength / (Math.PI * orbitRadius);

            int finalX = (int)Math.Round(centerX + (initialX - centerX) * Program.Cos(deltaAngle) + (centerY - initialY) * Program.Sin(deltaAngle), 0);
            int finalY = (int)Math.Round(centerY + (initialY - centerY) * Program.Cos(deltaAngle) + (initialX - centerX) * Program.Sin(deltaAngle), 0);

            location = new Point(finalX, finalY);
        }

        private void EnterOrbit(ref Point center, int maxRadius)
        {
            if (inOrbit == true)
            {
                return;
            }

            //if (velocity == 0)
            //{
            //    return;
            //}

            //int constant = maxRadius / 2;
            //double orbitRadius = (2 * constant) / Math.Pow(velocity, 2);

            //double orbitRadius = maxRadius - (velocity - 1) / 2;

            //if (orbitRadius > maxRadius)
            //{
            //    orbitRadius = maxRadius;
            //}

            //double distanceFromPoint = Program.GetDistance(location, center);
            //if (distanceFromPoint != orbitRadius)
            //{
            //    // Move into orbiting position
            //}

            // Determine orbit entry point
            double tangentDeltaX = Program.Sin(heading);
            double tangentDeltaY = Program.Cos(heading);
            double tangentC = tangentDeltaY * location.X + tangentDeltaX * location.Y;

            double normalDeltaX = Program.Sin(heading + 90);
            double normalDeltaY = Program.Cos(heading + 90);
            double normalC = normalDeltaY * center.X + normalDeltaX * center.Y;

            double determinant = tangentDeltaY * normalDeltaX - normalDeltaY * tangentDeltaX;

            // course isn't tangential to orbit
            if (determinant == 0)
            {
                return;
            }

            int orbitPositionX = (int)Math.Round((normalDeltaX * tangentC - tangentDeltaX * normalC) / determinant, 0);
            int orbitPositionY = (int)Math.Round((tangentDeltaY * normalC - normalDeltaX * tangentC) / determinant, 0);
            Point orbitPosition = new Point(orbitPositionX, orbitPositionY);

            double orbitRadius = Program.GetDistance(orbitPosition, center);

            if (orbitRadius > maxRadius)
            {
                orbiting = false;
                return;
            }

            AddWayPoint(orbitPosition);

            // accelerate to orbital velocity
            double orbitVelocity = maxRadius + 1 - 2 * orbitRadius;
            int deltaVelocity = (int)Math.Round(orbitVelocity, 0) - velocity;
            if (deltaVelocity != 0)
            {
                ChangeVelocity(deltaVelocity);
                return;
            }

            int distance = (int)Math.Round(Program.GetDistance(location, center), 0);
            if (distance > orbitRadius)
            {
                return;
            }

            inOrbit = true;
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

        public virtual void Bounce(int normalAngle)
        {
            int normalAdjustment = (360 - normalAngle) % 360;
            int relativeHeading = (heading + normalAdjustment) % 360;

            int bounceHeading = (180 - relativeHeading - normalAdjustment) % 360;

            if (bounceHeading < 0)
            {
                bounceHeading += 360;
            }

            heading = bounceHeading;
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
            if (Math.Abs(amount) > turnSpeed)
            {
                amount = Math.Sign(amount) * turnSpeed;
            }

            heading += amount;
            heading %= 360;
        }

        public virtual void Accelerate()
        {
            if (velocity >= maxVelocity)
            {
                velocity = maxVelocity;
                return;
            }

            ChangeVelocity(5);
        }

        public virtual void Decelerate()
        {
            if (Math.Abs(velocity) >= maxVelocity)
            {
                velocity = -maxVelocity;
                return;
            }

            ChangeVelocity(-5);
        }

        public virtual void ChangeVelocity(int delta)
        {
            velocity += delta;

            if (velocity >= maxVelocity)
            {
                velocity = maxVelocity;
                return;
            }

            if (Math.Abs(velocity) >= maxVelocity)
            {
                velocity = -maxVelocity;
                return;
            }
        }

        public virtual void Stop()
        {
            velocity = 0;
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

            if (Math.Abs(changeOfHeading) > elapsedTime * turnSpeed)
            {
                changeOfHeading = elapsedTime * turnSpeed * changeOfHeading / Math.Abs(changeOfHeading);
            }

            Turn((int)changeOfHeading);
        }

        public virtual void MoveToPoint(Point point, float elapsedTime)
        {
            TurnTowardsPoint(point, elapsedTime);

            int distance = (int)Program.GetDistance(location, point);
            if (distance == 0)
            {
                Stop();
                return;
            }

            Accelerate();
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
