using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Node
{
    public static class Program
    {
        public static List<Entity> allEntities { get; set; }
        /// <summary>
        /// The main entry center for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        public static void Update(float timeElapsed)
        {
            foreach (Entity entity in allEntities)
            {
                entity.Update(timeElapsed);
            }
        }

        public static void Draw(Graphics graphics, Size screenSize)
        {
            //graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, screenSize.Width, screenSize.Height));

            // Layer 0
            foreach (Entity entity in allEntities)
            {
                entity.DrawLayer0(graphics);
            }

            // Layer 1
            foreach (Entity entity in allEntities)
            {
                entity.DrawLayer1(graphics);
            }

            // Layer 2
            foreach (Entity entity in allEntities)
            {
                entity.DrawLayer2(graphics);
            }
        }

        public static double DegreesToRadians(double degrees)
        {
            double radians = degrees * Math.PI / 180;
            return radians;
        }

        public static double RadiansToDegrees(double radians)
        {
            double degrees = radians * 180 / Math.PI;
            return degrees;
        }

        public static double Sin(double degrees)
        {
            double radians = DegreesToRadians(degrees);
            double sine = Math.Sin(radians);
            return sine;
        }

        public static double Cos(double degrees)
        {
            double radians = DegreesToRadians(degrees);
            double cosine = Math.Cos(radians);
            return cosine;
        }

        public static double Tan(double degrees)
        {
            double radians = DegreesToRadians(degrees);
            double tangent = Math.Tan(radians);
            return tangent;
        }

        public static double Asin(double arcSine)
        {
            double radians = Math.Asin(arcSine);
            double degrees = RadiansToDegrees(radians);
            return degrees;
        }

        public static double Acos(double arcCosine)
        {
            double radians = Math.Acos(arcCosine);
            double degrees = RadiansToDegrees(radians);
            return degrees;
        }

        public static double Atan(double arcTan)
        {
            double radians = Math.Atan(arcTan);
            double degrees = RadiansToDegrees(radians);
            return degrees;
        }

        public static double GetAbsAngle(Point point1, Point point2)
        {
            int point1X = point1.X;
            int point1Y = point1.Y;

            int point2X = point2.X;
            int point2Y = point2.Y;

            double absAngle = 360;

            if (point2Y - point1Y == 0)
            {
                if (point2X - point1X == 0)
                {
                    absAngle = 0;
                    return absAngle;
                }

                if (point2X - point1X < 0)
                {
                    absAngle = 270;
                    return absAngle;
                }

                absAngle = 90;
                return absAngle;
            }

            if (point2Y - point1Y < 0)
            {
                absAngle = 180;
            }

            absAngle += Program.Atan((point2X - point1X) / (point2Y - point1Y));
            absAngle %= 360;

            return absAngle;
        }

        public static double GetDistance(int point1X, int point1Y, int point2X, int point2Y)
        {
            return GetDistance(new Point(point1X, point1Y), new Point(point2X, point2Y));
        }

        public static double GetDistance(Point point1, Point point2)
        {
            int point1X = point1.X;
            int point1Y = point1.Y;

            int point2X = point2.X;
            int point2Y = point2.Y;

            int deltaX = point2X - point1X;
            int deltaY = point2Y - point1Y;

            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            return distance;
        }

        public static Point RotatePointAroundAxis(int heading, Point point, Point axis)
        {
            return RotatePointAroundAxis(heading, point.X, point.Y, axis.X, axis.Y);
        }

        public static Point RotatePointAroundAxis(int heading, Point point, int axisX, int axisY)
        {
            return RotatePointAroundAxis(heading, point.X, point.Y, axisX, axisY);
        }

        public static Point RotatePointAroundAxis(int heading, int pointX, int pointY, Point axis)
        {
            return RotatePointAroundAxis(heading, pointX, pointY, axis.X, axis.Y);
        }

        public static Point RotatePointAroundAxis(int heading, int pointX, int pointY, int axisX, int axisY)
        {
            int relativePointX = pointX - axisX;
            int relativePointY = pointY - axisY;

            int rotatedPointX = (int)Math.Round(relativePointX * Program.Cos(heading) - relativePointY * Program.Sin(heading), 0);
            int rotatedpointY = (int)Math.Round(relativePointX * Program.Sin(heading) + relativePointY * Program.Cos(heading), 0);

            rotatedPointX += axisX;
            rotatedpointY += axisY;

            Point rotatedPoint = new Point(rotatedPointX, rotatedpointY);
            return rotatedPoint;
        }

        public static double GetMinDistanceToLineSegment(Point linePoint1, Point linePoint2, Point point)
        {
            int point1X = linePoint1.X;
            int point1Y = linePoint1.Y;

            int point2X = linePoint2.X;
            int point2Y = linePoint2.Y;

            int pointX = point.X;
            int pointY = point.Y;

            double lineDistance = Math.Sqrt(Math.Pow(point2Y - point1Y, 2) + Math.Pow(point2X - point1X, 2));
            double distance = Math.Abs((point2Y - point1Y) * pointX - (point2X - point1X) * pointY + point2X * point1Y - point2Y * point1X) / lineDistance;

            return distance;
        }

        public static double GetTangentSlope(Point point, Point center, int radius)
        {
            double angle = GetArcAngle(point, center, radius);

            double tangentSlope = -Program.Cos(angle) / Program.Sin(angle);
            if(double.IsInfinity(Math.Abs(tangentSlope)) == true) {
                tangentSlope = 0;
            }

            return tangentSlope;
        }

        public static double GetArcAngle(Point point, Point center, int radius)
        {
            int centerX = center.X;
            int centerY = center.Y;

            //int point0X = centerX;
            //int point0Y = centerY - radius;

            int pointX = point.X;
            int pointY = point.Y;

            //int distanceX = pointX - point0X;
            //int distanceY = pointY - point0Y;

            double distanceX = pointX - centerX;
            double distanceY = pointY - centerY;

            double angle = Program.Atan(distanceX / distanceY);
            return angle;
        }

        public static Point GetNearestSecantLinePoint(Point secantLineOrigin, int heading, Point circleCenter, int radius)
        {
            double pointAx = secantLineOrigin.X;
            double pointAy = secantLineOrigin.Y;

            double centerX = circleCenter.X;
            double centerY = circleCenter.Y;

            double distanceABx = Program.Sin(heading);
            double distanceABy = -Program.Cos(heading);

            double pointBx = pointAx + distanceABx;
            double pointBy = pointAy + distanceABy;

            //double distanceSquared = Math.Pow(distanceABx, 2) + Math.Pow(distanceABy, 2);

            //double dotProduct = pointAx * pointBy - pointBx * pointAy;
            //double discriminant = Math.Pow(radius, 2) * (Math.Pow(distanceABx, 2) + Math.Pow(distanceABy, 2)) - Math.Pow(dotProduct, 2);

            double t = distanceABx * (centerX - pointAx) + distanceABy * (centerY - pointAy);

            double pointEx = t * distanceABx + pointAx;
            double pointEy = t * distanceABy + pointAy;

            double distanceEC = Math.Sqrt(Math.Pow(pointEx - centerX, 2) + Math.Pow(pointEy - centerY, 2));
            if (distanceEC >= radius)
            {
                return new Point(int.MaxValue, int.MaxValue);
            }

            double distanceT = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(distanceEC, 2));

            //double sign = 1;
            //if (heading > 90)
            //{
            //    if (heading <= 270)
            //    {
            //        sign = -1;
            //    }
            //}

            //int intersectX = (int)Math.Round((dotProduct * distanceABy + sign * GetSign(distanceABy) * distanceABx * Math.Sqrt(discriminant)) / distanceSquared + centerX, 0);
            //int intersectY = (int)Math.Round((-dotProduct * distanceABx + sign * Math.Abs(distanceABy) * Math.Sqrt(discriminant)) / distanceSquared + centerY, 0);
            //int intersectX = (int)Math.Round((t + sign * distanceT) * distanceABx + pointAx, 0);
            //int intersectY = (int)Math.Round((t + sign * distanceT) * distanceABy + pointAy, 0);
            int intersectX = (int)Math.Round((t + distanceT) * distanceABx + pointAx, 0);
            int intersectY = (int)Math.Round((t + distanceT) * distanceABy + pointAy, 0);

            Point intersectPoint = new Point(intersectX, intersectY);
            return intersectPoint;
        }

        public static int GetSign(double number)
        {
            int sign = (int)(number / Math.Abs(number));
            return sign;
        }
    }
}
