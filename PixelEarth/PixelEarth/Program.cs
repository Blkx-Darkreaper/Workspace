using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PixelEarth
{
    public static class Program
    {
        public static List<Grid> atmosphere { get; set; }
        public static DateTime currentDate { get; set; }
        public static double gmt { get; set; }
        public enum View { Daylight, Temperature }
        public static View view { get; set; }
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

        //public static double GetSolarTime(DateTime date)
        //{

        //}

        public static void GenerateWorld(int width, int height)
        {
            atmosphere = new List<Grid>();
            gmt = 0;
            Size worldSize = new Size(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //int positionX = (int)Math.Round(x * Entity.metersPerPixel, 0);
                    //int positionY = (int)Math.Round(y * Entity.metersPerPixel, 0);

                    //Grid nextGrid = new Grid(positionX, positionY, (int)Entity.metersPerPixel, Color.Black);

                    Grid nextGrid = new Grid(x, y, worldSize, 5, 1, Color.Black);
                    atmosphere.Add(nextGrid);
                }
            }
        }

        public static void Update(float hoursElapsed)
        {
            gmt += hoursElapsed;

            foreach (Entity entity in atmosphere)
            {
                entity.Update(hoursElapsed);
            }
        }

        public static void Draw(Graphics graphics, Size screenSize)
        {
            //graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, screenSize.Width, screenSize.Height));  // fill background

            foreach (Grid grid in atmosphere)
            {
                switch (view)
                {
                    case View.Daylight:
                        grid.DrawDaylight(graphics);
                        break;

                    case View.Temperature:
                        grid.DrawTemperature(graphics);
                        break;

                    default:
                        grid.Draw(graphics);
                        break;
                }
            }

            bool drawTimezones = true;
            if (drawTimezones == true)
            {
                Pen pen = new Pen(Brushes.Blue);

                int bottomEdge = screenSize.Height - 1;
                int timezoneWidth = screenSize.Width / 24;

                for (int i = 1; i < 24; i++)
                {
                    int x = timezoneWidth * i;
                    graphics.DrawLine(pen, x, 0, x, bottomEdge);
                }
            }

            bool drawLatitudes = true;
            if (drawLatitudes == true)
            {
                Pen pen = new Pen(Brushes.Red);
                int latitude;

                double modifier = (double)screenSize.Height / 180;

                int rightEdge = screenSize.Width - 1;

                // Arctic circle
                latitude = (int)(23 * modifier);
                graphics.DrawLine(pen, 0, latitude, rightEdge, latitude);

                // Tropic of Cancer
                latitude = (int)(67 * modifier);
                graphics.DrawLine(pen, 0, latitude, rightEdge, latitude);

                // Equator
                latitude = (int)(90 * modifier);
                graphics.DrawLine(pen, 0, latitude, rightEdge, latitude);

                // Tropic of Capricorn
                latitude = (int)(113 * modifier);
                graphics.DrawLine(pen, 0, latitude, rightEdge, latitude);

                // Antarctic circle
                latitude = (int)(157 * modifier);
                graphics.DrawLine(pen, 0, latitude, rightEdge, latitude);
            }
        }

        public static DateTime GetDateTime()
        {
            return GetDateTime(gmt);
        }

        public static DateTime GetDateTime(double absTime)
        {
            absTime += 12;  // Start at noon (beginning of Julian Day)

            int days = (int)(absTime) / 24;
            int hours = (int)(absTime) % 24;
            int mins = (int)(60 * (absTime - 24 * days - hours));

            DateTime dateTime = new DateTime(2016, 6, 20, 0, 0, 0);
            dateTime = dateTime.AddDays(days);
            dateTime = dateTime.AddHours(hours);
            dateTime = dateTime.AddMinutes(mins);

            return dateTime;
        }

        public static DateTime GetDateTimeFromJulianDate(double julianDate)
        {
            int julianDay = (int)Math.Round(julianDate, 0);
            int e = julianDay + 1401 + (((4 * julianDay + 274277) / 146097) * 3) / 4 - 38;
            int f = 4 * e + 3;
            int g = (f % 1461) / 4;
            int h = 5 * g + 2;

            double time = (julianDate - julianDay) * 24 - 12;   // julian day starts at noon
            if (time < 0)
            {
                time += 24;
            }

            int hour = (int)time;
            int minute = (int)(60 * (time - hour));
            int second = (int)(60 * (60 * (time - hour) - minute));

            int day = (h % 153) / 5 + 1;
            int month = (h / 153 + 2) % 12 + 1;
            int year = f / 1461 - 4716 + (12 + 2 - month) / 12;

            DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
            return dateTime;
        }

        public static double GetJulianDate(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;

            double hours = dateTime.Hour;
            double mins = dateTime.Minute;
            double secs = dateTime.Second;
            double time = (hours - 12 + mins / 60 + secs / 3600) / 24;

            int a = (int)Math.Floor((double)(14 - month) / 12);
            double y = year + 4800 - a;
            double m = month + 12 * a - 3;

            double julianDay = (day + Math.Floor((153 * m + 2) / 5) + 365 * y + Math.Floor(y / 4) - Math.Floor(y / 100) + Math.Floor(y / 400) - 32045);
            double julianDate = julianDay + time;
            return julianDate;
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
            if (double.IsInfinity(Math.Abs(tangentSlope)) == true)
            {
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
