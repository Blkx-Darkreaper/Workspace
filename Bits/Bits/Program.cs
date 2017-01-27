using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Bits
{
    public static class Program
    {
        public static float CRUISE_VELOCITY = 2;
        public static float MAX_VELOCITY = 5;
        public static float OUTPUT_DELAY = 16;
        public static int DataFormat { get; set; }
        public static bool IsDisplayingVarNames { get; set; }
        public enum DataFormats
        {
            Binary, Decimal, Hexadecimal, String
        }
        public struct Text
        {
            public enum Justified
            {
                Left, Center, Right
            }
            public enum Alignment
            {
                Top, Middle, Bottom
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
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

        public static double GetAbsAngle(PointF origin, PointF point)
        {
            float originX = origin.X;
            float originY = origin.Y;

            float pointX = point.X;
            float pointY = point.Y;

            double absAngle = 360;

            if (pointY == originY)
            {
                if (pointX == originX)
                {
                    absAngle = 0;
                    return absAngle;
                }

                if (originX > pointX)
                {
                    absAngle = 270;
                    return absAngle;
                }

                absAngle = 90;
                return absAngle;
            }

            if (pointY > originY)
            {
                absAngle = 180;
            }

            double angle = -Program.Atan((pointX - originX) / (pointY - originY));
            absAngle += angle;
            absAngle %= 360;

            return absAngle;
        }

        public static double GetDistance(float point1X, float point1Y, float point2X, float point2Y)
        {
            return GetDistance(new PointF(point1X, point1Y), new PointF(point2X, point2Y));
        }

        //public static double GetDistance(Point origin, Point point)
        //{
        //    return GetDistance(origin, point);
        //}

        public static double GetDistance(PointF point1, PointF point2)
        {
            float point1X = point1.X;
            float point1Y = point1.Y;

            float point2X = point2.X;
            float point2Y = point2.Y;

            float deltaX = point2X - point1X;
            float deltaY = point2Y - point1Y;

            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            return distance;
        }

        public static double GetClockwiseArcLength(float point1X, float point1Y, float point2X, float point2Y, float centerX, float centerY)
        {
            return GetClockwiseArcLength(new PointF(point1X, point1Y), new PointF(point2X, point2Y), new PointF(centerX, centerY));
        }

        //public static double GetClockwiseArcLength(PointF point1, PointF point2, PointF centrum)
        //{
        //    float point1X = point1.X;
        //    float point1Y = point1.Y;

        //    float point2X = point2.X;
        //    float point2Y = point2.Y;

        //    float centerX = centrum.X;
        //    float centerY = centrum.Y;

        //    double radius1 = GetDistance(point1, centrum);
        //    double radius2 = GetDistance(point2, centrum);

        //    double radius = Math.Min(radius1, radius2);
        //    double deltaAngle = Program.Atan(Math.Abs(point1Y - centerY) / radius) + Program.Atan(Math.Abs(point2Y - centerY) / radius);

        //    double arcLength = radius * deltaAngle * Math.PI / 180.0;
        //    return arcLength;
        //}

        public static double GetClockwiseArcLength(PointF point1, PointF point2, PointF centrum)
        {
            double radius1 = GetDistance(point1, centrum);
            double radius2 = GetDistance(point2, centrum);

            double radius = Math.Min(radius1, radius2);
            double clockwiseArcAngle = GetClockwiseArcAngle(point1, point2, centrum);

            double arcLength = radius * clockwiseArcAngle * Math.PI / 180.0;
            return arcLength;
        }

        public static double GetClockwiseArcLength(double arcAngle, PointF point, PointF center)
        {
            double radius = GetDistance(point, center);
            return GetClockwiseArcLength(arcAngle, radius);
        }

        public static double GetClockwiseArcLength(double arcAngle, double radius)
        {
            double arcLength = radius * arcAngle * Math.PI / 180;
            return arcLength;
        }

        public static double GetArcAngle(double arcLength, double radius)
        {
            double arcAngle = arcLength * 180 / (radius * Math.PI);
            return arcAngle;
        }

        public static double GetClockwiseArcAngle(PointF point1, PointF point2, PointF centrum)
        {
            double bearing1 = Program.GetAbsAngle(centrum, point1);
            double bearing2 = Program.GetAbsAngle(centrum, point2);

            double clockwiseArcAngle = (bearing2 - bearing1) % 360;
            if (clockwiseArcAngle < 0)
            {
                clockwiseArcAngle += 360;
            }

            return clockwiseArcAngle;
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

        public static void DrawDot(Graphics graphics, Color colour, Point center)
        {
            SolidBrush brush = new SolidBrush(colour);

            graphics.FillRectangle(brush, center.X, center.Y, 1, 1);
        }

        public static void DrawLine(Graphics graphics, Color colour, Point lineStart, int heading, int length)
        {
            int tailX = lineStart.X + (int)Math.Round(Program.Sin(heading) * length, 0);
            int tailY = lineStart.Y - (int)Math.Round(Program.Cos(heading) * length, 0);    // inverted Y

            Point lineEnd = new Point(tailX, tailY);

            Program.DrawLine(graphics, colour, lineStart, lineEnd);
        }

        public static void DrawLine(Graphics graphics, Color colour, Point lineStart, Point lineEnd)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            graphics.DrawLine(pen, lineStart, lineEnd);
        }

        public static void DrawDottedLine(Graphics graphics, Color colour, Point lineStart, Point lineEnd, int dashLength, int spaceLength)
        {
            int heading = (int)Program.GetAbsAngle(lineStart, lineEnd);
            int length = (int)Program.GetDistance(lineStart, lineEnd);

            Program.DrawDottedLine(graphics, colour, lineStart, heading, length, dashLength, spaceLength);
        }

        public static void DrawDottedLine(Graphics graphics, Color colour, Point lineStart, int heading, int length, int dashLength, int spaceLength)
        {
            Point drawCursor = new Point(lineStart.X, lineStart.Y);

            int drawDistance = (int)Program.GetDistance(lineStart, drawCursor);
            while (drawDistance < length)
            {
                // Draw dash
                Program.DrawLine(graphics, colour, drawCursor, heading, dashLength);

                // Draw space
                int cursorX = drawCursor.X + (int)Math.Round(Program.Sin(heading) * (dashLength + spaceLength), 0);
                int cursorY = drawCursor.Y - (int)Math.Round(Program.Cos(heading) * (dashLength + spaceLength), 0); // inverted Y

                drawCursor = new Point(cursorX, cursorY);
                drawDistance = (int)Program.GetDistance(lineStart, drawCursor);
            }
        }

        public static Rectangle GetCircularBounds(Point center, int radius)
        {
            int cornerX = center.X - radius;
            int cornerY = center.Y - radius;

            int diameter = radius * 2;

            Rectangle bounds = new Rectangle(cornerX, cornerY, diameter, diameter);
            return bounds;
        }

        public static void DrawCircle(Graphics graphics, Color colour, Point center, int radius)
        {
            Rectangle bounds = GetCircularBounds(center, radius);
            Program.DrawCircle(graphics, colour, bounds);
        }

        public static void DrawCircle(Graphics graphics, Color colour, Rectangle bounds)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            graphics.DrawEllipse(pen, bounds);
        }

        public static void FillCircle(Graphics graphics, Color colour, Point center, int radius)
        {
            Rectangle bounds = GetCircularBounds(center, radius);

            Program.FillCircle(graphics, colour, bounds);
        }

        public static void FillCircle(Graphics graphics, Color colour, Rectangle bounds)
        {
            Brush brush = new SolidBrush(colour);

            graphics.FillEllipse(brush, bounds);
        }

        public static void DrawArc(Graphics graphics, Color colour, Point center, int radius, int heading, int arcLength)
        {
            Rectangle bounds = GetCircularBounds(center, radius);
            Program.DrawArc(graphics, colour, bounds, heading, arcLength);
        }

        public static void DrawArc(Graphics graphics, Color colour, Rectangle bounds, int heading, int arcLength)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            int adjustedAngle = heading - 90;
            if (adjustedAngle < 0)
            {
                adjustedAngle += 360;
            }

            graphics.DrawArc(pen, bounds, adjustedAngle, arcLength);
        }

        public static void DrawSquare(Graphics graphics, Color colour, Point center, int heading, int width)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            Point[] allVertices = GetSquareVertices(center, heading, width);

            graphics.DrawPolygon(pen, allVertices);
        }

        private static Point[] GetSquareVertices(Point center, int heading, int width)
        {
            List<Point> allVertices = new List<Point>();

            int cornerX, cornerY;
            Point rotatedCorner;

            cornerX = center.X - width / 2;
            cornerY = center.Y - width / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, center);
            allVertices.Add(rotatedCorner);

            Point copy = new Point(rotatedCorner.X, rotatedCorner.Y);

            cornerX = center.X + width / 2;
            //vertexY = drawLocation.Y - length / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, center);
            allVertices.Add(rotatedCorner);

            //vertexX = drawLocation.X + length / 2;
            cornerY = center.Y + width / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, center);
            allVertices.Add(rotatedCorner);

            cornerX = center.X - width / 2;
            //vertexY = drawLocation.Y + length / 2;
            rotatedCorner = Program.RotatePointAroundAxis(heading, cornerX, cornerY, center);
            allVertices.Add(rotatedCorner);

            allVertices.Add(copy);
            return allVertices.ToArray();
        }

        public static void FillSquare(Graphics graphics, Color colour, Point center, int heading, int width)
        {
            Brush brush = new SolidBrush(colour);

            Point[] allVertices = GetSquareVertices(center, heading, width);

            graphics.FillPolygon(brush, allVertices);
        }

        public static void DrawRectangle(Graphics graphics, Color colour, Point center, Size size)
        {
            Rectangle bounds = GetRectangularBounds(center, size);

            Program.DrawRectangle(graphics, colour, bounds);
        }

        private static Rectangle GetRectangularBounds(Point center, Size size)
        {
            int width = size.Width;
            int height = size.Height;
            int cornerX = center.X - width / 2;
            int cornerY = center.Y - height / 2;
            Rectangle bounds = new Rectangle(cornerX, cornerY, width, height);
            return bounds;
        }

        public static void DrawRectangle(Graphics graphics, Color colour, Rectangle bounds)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            graphics.DrawRectangle(pen, bounds);
        }

        public static void DrawDottedRectangle(Graphics graphics, Color colour, Point center, Size size, int dashLength, int spaceLength)
        {
            int width = size.Width;
            int height = size.Height;
            int cornerX = center.X - width / 2;
            int cornerY = center.Y - height / 2;

            List<Point> allPoints = new List<Point>();
            Point vertex = new Point(cornerX, cornerY);
            Point copy = new Point(cornerX, cornerY);
            allPoints.Add(vertex);

            cornerX = center.X + width / 2;
            vertex = new Point(cornerX, cornerY);
            allPoints.Add(vertex);

            cornerY = center.Y + height / 2;
            vertex = new Point(cornerX, cornerY);
            allPoints.Add(vertex);

            cornerX = center.X - width / 2;
            vertex = new Point(cornerX, cornerY);
            allPoints.Add(vertex);

            allPoints.Add(copy);

            Program.DrawDottedPolygon(graphics, colour, allPoints.ToArray(), dashLength, spaceLength);
        }

        public static void FillRectangle(Graphics graphics, Color colour, Point center, Size size)
        {
            Rectangle bounds = GetRectangularBounds(center, size);

            Program.FillRectangle(graphics, colour, bounds);
        }

        public static void FillRectangle(Graphics graphics, Color colour, Rectangle bounds)
        {
            Brush brush = new SolidBrush(colour);

            graphics.FillRectangle(brush, bounds);
        }

        public static void DrawHexagon(Graphics graphics, Color colour, Point center, int heading, int majorAxis)
        {
            Pen pen = new Pen(new SolidBrush(colour));

            Point[] allVertices = GetHexagonVertices(center, heading, majorAxis);

            graphics.DrawPolygon(pen, allVertices);
        }

        public static void FillHexagon(Graphics graphics, Color colour, Point center, int heading, int majorAxis)
        {
            Brush brush = new SolidBrush(colour);

            Point[] allVertices = GetHexagonVertices(center, heading, majorAxis);

            graphics.FillPolygon(brush, allVertices);
        }

        private static Point[] GetHexagonVertices(Point center, int heading, int majorAxis)
        {
            List<Point> allPoints = new List<Point>();

            int vertexX, vertexY;
            Point rotatedVertex;

            vertexX = center.X;
            vertexY = center.Y - majorAxis / 2;
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            Point copy = new Point(rotatedVertex.X, rotatedVertex.Y);

            vertexX = center.X + majorAxis / 2;
            vertexY += (int)Math.Ceiling(majorAxis / 4f);
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            vertexY += majorAxis / 2;
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            vertexX = center.X;
            vertexY += (int)Math.Ceiling(majorAxis / 4f);
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            vertexX = center.X - majorAxis / 2;
            vertexY -= (int)Math.Ceiling(majorAxis / 4f);
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            vertexY -= majorAxis / 2;
            rotatedVertex = Program.RotatePointAroundAxis(heading, vertexX, vertexY, center);
            allPoints.Add(rotatedVertex);

            allPoints.Add(copy);
            return allPoints.ToArray();
        }

        public static void DrawPolygon(Graphics graphics, Color colour, Point[] allVertices)
        {
            bool firstVertex = true;
            Point previous = allVertices[0];

            foreach (Point next in allVertices)
            {
                if (firstVertex == true)
                {
                    previous = next;
                    firstVertex = false;
                    continue;
                }

                Program.DrawLine(graphics, colour, previous, next);
                previous = next;
            }
        }

        public static void DrawDottedPolygon(Graphics graphics, Color colour, Point[] allVertices, int dashLength, int spaceLength)
        {
            bool firstVertex = true;
            Point previous = allVertices[0];

            foreach (Point next in allVertices)
            {
                if (firstVertex == true)
                {
                    previous = next;
                    firstVertex = false;
                    continue;
                }

                Program.DrawDottedLine(graphics, colour, previous, next, dashLength, spaceLength);
                previous = next;
            }
        }

        //public static void DrawTextRightToLeft(Graphics graphics, Color colour, string text, Rectangle bounds)
        //{
        //    Font font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        //    SizeF textSize = graphics.MeasureString(text, font);

        //    int shiftedCornerX = topRightCorner.X - (int)Math.Ceiling(textSize.Width);
        //    Point topLeftCorner = new Point(shiftedCornerX, topRightCorner.Y);

        //    DrawText(graphics, colour, text, topLeftCorner);
        //}

        //public static void DrawTextCentered(Graphics graphics, Color colour, string text, Rectangle bounds)
        //{
        //    Font font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        //    SizeF textSize = graphics.MeasureString(text, font);

        //    int shiftedCornerX = center.X - (int)Math.Ceiling(textSize.Width);
        //    int shiftedCornerY = center.Y - (int)Math.Ceiling(textSize.Height);
        //    Point topLeftCorner = new Point(shiftedCornerX, shiftedCornerY);

        //    DrawText(graphics, colour, text, topLeftCorner);
        //}

        public static SizeF GetTextSize(Graphics graphics, string text)
        {
            Font font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF textSize = graphics.MeasureString(text, font);
            return textSize;
        }

        public static void DrawText(Graphics graphics, Color colour, string text, Rectangle bounds)
        {
            DrawText(graphics, colour, text, bounds, (int)Text.Justified.Left, (int)Text.Alignment.Top);
        }

        public static void DrawTextJustified(Graphics graphics, Color colour, string text, Rectangle bounds, int justified)
        {
            DrawText(graphics, colour, text, bounds, justified, (int)Text.Alignment.Top);
        }

        public static void DrawTextCentered(Graphics graphics, Color colour, string text, Rectangle bounds)
        {
            DrawTextJustified(graphics, colour, text, bounds, (int)Program.Text.Justified.Center);
        }

        public static void DrawTextAligned(Graphics graphics, Color colour, string text, Rectangle bounds, int alignment)
        {
            DrawText(graphics, colour, text, bounds, (int)Text.Justified.Left, alignment);
        }

        public static void DrawText(Graphics graphics, Color colour, string text, Rectangle bounds, int justified, int alignment)
        {
            Font font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(colour);

            SizeF textSize = graphics.MeasureString(text, font);

            float x, y;

            const int leftJustify = (int)Text.Justified.Left;
            const int rightJustify = (int)Text.Justified.Right;
            const int centerJustify = (int)Text.Justified.Center;
            switch (justified)
            {
                case rightJustify:
                    x = bounds.X + bounds.Width - textSize.Width;
                    break;

                case centerJustify:
                    x = bounds.X + bounds.Width / 2 - textSize.Width / 2;
                    break;

                case leftJustify:
                default:
                    x = bounds.X;
                    break;
            }

            const int alignedTop = (int)Text.Alignment.Top;
            const int alignedMiddle = (int)Text.Alignment.Middle;
            const int alignedBottom = (int)Text.Alignment.Bottom;
            switch (alignment)
            {
                case alignedBottom:
                    y = bounds.Y + bounds.Height - textSize.Height;
                    break;

                case alignedMiddle:
                    y = bounds.Y + bounds.Height / 2 - textSize.Height / 2;
                    break;

                case alignedTop:
                default:
                    y = bounds.Y;
                    break;
            }

            graphics.DrawString(text, font, brush, x, y);
        }

        public static void DrawChevrons(Graphics graphics, Color colour, Size display, Point center, int heading, int width, int lineThickness, int copies)
        {
            int tipX = center.X;

            //int headWidth = totalBytes.Width;
            int halfWidth = width / 2;
            //int length = totalBytes.Height;

            int actualWidth = (width % 2 == 0 ? width + 1 : width);
            //int copies = 1 + (length - ((int)Math.Ceiling((double)actualWidth / 2) + 1)) / 4;

            int spacing = 2 * lineThickness;
            int actualHeight = (int)Math.Ceiling((double)actualWidth / 2) + 1 + spacing * (copies - 1);
            int offsetY = -actualHeight / 2;

            for (int i = 0; i < copies; i++)
            {
                for (int j = 0; j < lineThickness; j++)
                {
                    int tipY = center.Y + offsetY + i * spacing;
                    int wingY = tipY + halfWidth;

                    tipY += j;
                    Point tip = Program.RotatePointAroundAxis(heading, new Point(tipX, tipY), center);

                    wingY += j;

                    Point leftWing = Program.RotatePointAroundAxis(heading, new Point(tipX - halfWidth, wingY), center);
                    Program.DrawLine(graphics, colour, tip, leftWing);

                    Point rightWing = Program.RotatePointAroundAxis(heading, new Point(tipX + halfWidth, wingY), center);
                    Program.DrawLine(graphics, colour, tip, rightWing);
                }
            }
        }

        public static bool ComparePoints(Point pointA, Point pointB)
        {
            int pointAx = pointA.X;
            int pointBx = pointB.X;
            if (pointAx != pointBx)
            {
                return false;
            }

            int pointAy = pointA.Y;
            int pointBy = pointB.Y;
            if (pointAy != pointBy)
            {
                return false;
            }

            return true;
        }

        public static string ConvertToBinary(string value)
        {
            int dec = ConvertToDecimal(value);
            string binary = ConvertDecimalToBinary(dec);

            return binary;
        }

        public static string ConvertDecimalToBinary(int dec)
        {
            string binary = string.Empty;
            int remaining = dec;

            int firstBitPosition = (int)Math.Log(remaining, 2);
            if (firstBitPosition <= 0)
            {
                return "0";
            }

            for (int bitPosition = firstBitPosition; bitPosition >= 0; bitPosition--)
            {
                int bitValue = (int)Math.Pow(2, bitPosition);
                if (bitValue <= remaining)
                {
                    binary += '1';
                    remaining -= bitValue;
                }
                else
                {
                    binary += '0';
                }
            }

            return binary;
        }

        public static int ConvertToDecimal(string value)
        {
            int dec;
            bool isNumeral = int.TryParse(value, out dec);
            if (isNumeral == false)
            {
                dec = ConvertAsciiToDecimal(value);
            }

            return dec;
        }

        public static int ConvertAsciiToDecimal(string ascii)
        {
            char character = ascii.ToCharArray()[0];
            int dec = (int)character;

            return dec;
        }

        public static string ConvertToHexadecimal(string value)
        {
            int dec = ConvertToDecimal(value);

            string hex = ConvertDecimalToHexadecimal(dec);
            return hex;
        }

        public static string ConvertDecimalToHexadecimal(int dec)
        {
            string hexPrefix = "0x";
            string hex = string.Empty;

            int dividend = dec;

            do
            {
                int quotient = dividend / 16;
                int remainder = dividend - 16 * quotient;
                int value = remainder;

                char hexValue;
                if (value > 9)
                {
                    hexValue = (char)(Convert.ToUInt16('A') + value - 10);
                }
                else
                {
                    hexValue = (char)(Convert.ToUInt16('0') + value);
                }

                hex = hexValue + hex;

                dividend = quotient;
            } while (dividend > 0);

            hex = hexPrefix + hex;

            return hex;
        }
    }
}