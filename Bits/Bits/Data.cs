using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Data : Entity
    {
        public Point Center { get; protected set; }
        public PointF CenterF { get; protected set; }
        public int Value { get; protected set; }
        //protected Branches currentPath { get; set; }
        protected Queue<Point> wayPoints { get; set; }
        public float Velocity { get; protected set; }
        public  float CruiseVelocity { get; protected set; }
        public float MaxVelocity { get; protected set; }
        protected int heading { get; set; }
        protected int facing { get; set; }
        protected bool isOrbiting { get; set; }
        protected int orbitRadius { get; set; }
        protected Point orbitCentrum { get; set; }
        protected int orbitsRemaining { get; set; }

        public Data(Point center, float cruiseVelocity, float maxVelocity)
            : base()
        {
            int centerX = center.X;
            int centerY = center.Y;
            this.Center = new Point(centerX, centerY);
            this.CenterF = new PointF(centerX, centerY);
            this.Value = 0;
            this.wayPoints = new Queue<Point>();
            this.Velocity = 0;
            this.CruiseVelocity = cruiseVelocity;
            this.MaxVelocity = maxVelocity;
            this.heading = 0;
            this.facing = 0;
            this.isOrbiting = false;
            this.orbitRadius = 0;
            this.orbitsRemaining = 0;
        }

        public Data(Point center, int value, float cruiseVelocity, float maxVelocity)
            : this(center, cruiseVelocity, maxVelocity)
        {
            this.Value = value;
        }

        public Data(int x, int y, float cruiseVelocity, float maxVelocity) : this(new Point(x, y), cruiseVelocity, maxVelocity) { }

        public Data(int x, int y, int value, float cruiseVelocity, float maxVelocity) : this(new Point(x, y), value, cruiseVelocity, maxVelocity) { }

        //public Content(Branches startPath, int totalEntries, int MAX_VELOCITY)
        //    : this(startPath.StartPoint.Center, totalEntries, MAX_VELOCITY)
        //{
        //    this.currentPath = startPath;
        //    AddWayPoint(startPath.Output.Center);
        //    ChangeVelocity(1);
        //}

        public virtual void SetLocation(Point location)
        {
            int locationX = location.X;
            int locationY = location.Y;
            this.Center = new Point(locationX, locationY);
            this.CenterF = new Point(locationX, locationY);
        }

        public override void Update(float timeElapsed)
        {
            if (isOrbiting == true)
            {
                Orbit(timeElapsed);
            }
            else
            {
                HeadToWayPoint();

                int initialX = Center.X;
                int initialY = Center.Y;

                float deltaX = (float)(Program.Sin(heading) * Velocity * timeElapsed);
                float deltaY = (float)(Program.Cos(heading) * Velocity * timeElapsed);

                float finalFX = initialX + deltaX;
                float finalFY = initialY - deltaY;

                CenterF = new PointF(finalFX, finalFY);

                int finalX = initialX + (int)Math.Round(deltaX, 0);
                int finalY = initialY - (int)Math.Round(deltaY, 0);
                Center = new Point(finalX, finalY);
            }
        }

        public virtual void Orbit(Point centrum, int radius)
        {
            if (isOrbiting == true)
            {
                return;
            }

            isOrbiting = true;
            orbitCentrum = centrum;
            orbitRadius = radius;
            orbitsRemaining = -1;
        }

        public virtual void StopOrbiting()
        {
            if (isOrbiting == false)
            {
                return;
            }

            orbitsRemaining = 1;
        }

        public virtual void Orbit(float timeElapsed)
        {
            if (isOrbiting == false)
            {
                return;
            }

            double arcLength = Velocity * timeElapsed;

            int initialX = Center.X;
            int initialY = Center.Y;

            int centrumX = orbitCentrum.X;
            int centrumY = orbitCentrum.Y;

            double deltaAngle = 180 * arcLength / (Math.PI * orbitRadius);

            float finalFX = (float)(centrumX + (initialX - centrumX) * Program.Cos(deltaAngle) + (centrumY - initialY) * Program.Sin(deltaAngle));
            float finalFY = (float)(centrumY + (initialY - centrumY) * Program.Cos(deltaAngle) + (initialX - centrumX) * Program.Sin(deltaAngle));
            CenterF = new PointF(finalFX, finalFY);

            int finalX = (int)Math.Round(finalFX, 0);
            int finalY = (int)Math.Round(finalFY, 0);
            Center = new Point(finalX, finalY);
        }

        //public virtual void SwitchPath()
        //{
        //    Branches nextPath = currentPath.Output.Branches;
        //    if (nextPath == null)
        //    {
        //        return;
        //    }

        //    int pathType = nextPath.GetPathType();

        //    switch (pathType)
        //    {
        //        case (int)Branches.PathType.BitNode:
        //            BitNode node = (BitNode)nextPath;
        //            Orbit(node.Center, BitNode.nodeRadius);
        //            break;

        //        case (int)Branches.PathType.Straight:
        //        default:
        //            Point endPoint = nextPath.Output.Center;
        //            AddWayPoint(endPoint);
        //            break;
        //    }
        //}

        public virtual void AddWayPoint(Point wayPoint)
        {
            wayPoints.Enqueue(wayPoint);
        }

        public virtual bool WayPointReached(PointF currentLocation, float threshold = 1f)
        {
            if (wayPoints == null)
            {
                return true;
            }
            if (wayPoints.Count == 0)
            {
                return true;
            }

            Point nextWayPoint = wayPoints.Peek();
            if (nextWayPoint == null)
            {
                return true;
            }

            if (isOrbiting == true)
            {
                float arcLength = (float)Program.GetClockwiseArcLength(currentLocation, nextWayPoint, orbitCentrum);
                if (arcLength > threshold)
                {
                    return false;
                }

                if (orbitsRemaining > 0)
                {
                    orbitsRemaining--;
                }

                if (orbitsRemaining == 0)
                {
                    isOrbiting = false;
                }

                if (isOrbiting == true)
                {
                    return false;
                }
            }
            else
            {
                float distance = (float)Program.GetDistance(currentLocation, nextWayPoint);
                if (distance > threshold)
                {
                    return false;
                }
            }

            wayPoints.Dequeue();

            //SwitchPath();
            return true;
        }

        protected virtual void HeadToWayPoint()
        {
            if (wayPoints == null)
            {
                //Stop();
                return;
            }
            if (wayPoints.Count == 0)
            {
                //Stop();
                return;
            }

            Point nextWayPoint = wayPoints.Peek();
            if (nextWayPoint == null)
            {
                return;
            }

            int absBearing = (int)Math.Round(Program.GetAbsAngle(Center, nextWayPoint), 0);

            int headingChange = absBearing - heading;

            ChangeHeading(headingChange);
        }

        public virtual void ChangeHeading(int amount)
        {
            //if (Math.Abs(amount) > turnSpeed)
            //{
            //    amount = Math.Sign(amount) * turnSpeed;
            //}

            heading += amount;
            if (heading < 0)
            {
                heading += 360;
            }

            heading %= 360;
        }

        public virtual void TurnLeft()
        {
            TurnLeft(1);
        }

        public virtual void TurnLeft(int amount)
        {
            Turn(-amount);
        }

        public virtual void TurnRight()
        {
            TurnRight(1);
        }

        public virtual void TurnRight(int amount)
        {
            Turn(amount);
        }

        public virtual void Turn(int amount)
        {
            //if (Math.Abs(amount) > turnSpeed)
            //{
            //    amount = Math.Sign(amount) * turnSpeed;
            //}

            facing += amount;
            if (facing < 0)
            {
                facing += 360;
            }

            facing %= 360;
        }

        public virtual void Accelerate()
        {
            //if (finalVelocity >= MAX_VELOCITY)
            //{
            //    finalVelocity = MAX_VELOCITY;
            //    return;
            //}

            ChangeVelocity(0.5f);
        }

        public virtual void Decelerate()
        {
            //if (Math.Abs(finalVelocity) >= MAX_VELOCITY)
            //{
            //    finalVelocity = -MAX_VELOCITY;
            //    return;
            //}

            ChangeVelocity(-0.5f);
        }

        public virtual void Cruise()
        {
            if (Velocity == CruiseVelocity)
            {
                return;
            }

            SetVelocity(CruiseVelocity);
        }

        public virtual void ChangeVelocity(float delta)
        {
            Velocity += delta;

            if (Velocity < 0)
            {
                Velocity = 0;
                return;
            }

            //if (finalVelocity >= MAX_VELOCITY)
            //{
            //    finalVelocity = MAX_VELOCITY;
            //    return;
            //}

            //if (Math.Abs(finalVelocity) >= MAX_VELOCITY)
            //{
            //    finalVelocity = -MAX_VELOCITY;
            //    return;
            //}
        }

        public virtual void SetVelocity(float finalVelocity)
        {
            if (finalVelocity > MaxVelocity)
            {
                Velocity = MaxVelocity;
                return;
            }

            Velocity = finalVelocity;
        }

        public virtual void Stop()
        {
            Velocity = 0;
        }

        //public virtual void TurnTowardsPoint(Point point, float elapsedTime)
        //{
        //    double desiredHeading = Program.GetAbsAngle(Center, point);

        //    double changeOfHeading1 = desiredHeading - bearing;
        //    double changeOfHeading2 = bearing - desiredHeading;

        //    double changeOfHeading;
        //    if (Math.Abs(changeOfHeading1) <= Math.Abs(changeOfHeading2))
        //    {
        //        changeOfHeading = changeOfHeading1;
        //    }
        //    else
        //    {
        //        changeOfHeading = changeOfHeading2;
        //    }

        //    //if (Math.Abs(changeOfHeading) > elapsedTime * turnSpeed)
        //    //{
        //    //    changeOfHeading = elapsedTime * turnSpeed * changeOfHeading / Math.Abs(changeOfHeading);
        //    //}

        //    Turn((int)changeOfHeading);
        //}

        //public virtual void MoveToPoint(Point point, float elapsedTime)
        //{
        //    TurnTowardsPoint(point, elapsedTime);

        //    int deviation = (int)Program.GetDistance(Center, point);
        //    if (deviation == 0)
        //    {
        //        Stop();
        //        return;
        //    }

        //    Accelerate();
        //}
    }
}