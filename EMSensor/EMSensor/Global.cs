using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EMSensor
{
    class Global
    {
        public static double GetBearingToPoint(Point center, Point destination)
        {
            double centerX = center.X;
            double centerY = center.Y;
            double destinationX = destination.X;
            double destinationY = destination.Y;

            double distanceX = centerX - destinationX;
            double distanceY = destinationY - centerY; // Y Axis increases downwards

            double heading = 360;

            if (distanceY == 0)
            {
                if (distanceX < 0)
                {
                    heading = 270;
                    return heading;
                }

                heading = 90;
                return heading;
            }

            if (distanceY < 0)
            {
                heading = 180;
            }

            heading += ConvertRadiansToDegrees(Math.Atan(distanceX / distanceY));
            heading %= 360;

            return heading;
        }

        public static double GetDistanceToPoint(Point origin, Point destination)
        {
            double originX = origin.X;
            double originY = origin.Y;
            double receiverX = destination.X;
            double receiverY = destination.Y;

            double distanceX = receiverX - originX;
            double distanceY = receiverY - originY;

            double distanceToPoint = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            return distanceToPoint;
        }

        public static double ConvertDegreesToRadians(double degrees)
        {
            double radians = Math.PI * degrees / 180;
            return radians;
        }

        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = radians * 180 / Math.PI;
            return degrees;
        }
    }
}
