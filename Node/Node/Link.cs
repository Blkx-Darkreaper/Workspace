using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Node
{
    public class Link : Entity
    {
        public NetworkNode start { get; set; }
        public NetworkNode end { get; set; }
        public List<Packet> incomingPackets { get; set; }
        public List<Packet> outgoingPackets { get; set; }

        public Link(int width, NetworkNode start, NetworkNode end, Color mainColour, Color backgroundColour)
            : base(new Point(), width, mainColour, backgroundColour)
        {
            this.start = start;
            this.end = end;
            incomingPackets = new List<Packet>();
            outgoingPackets = new List<Packet>();
        }

        public Link(int width, NetworkNode start, NetworkNode end, Color mainColour) : base(new Point(), width, mainColour) {
            this.start = start;
            this.end = end;
            incomingPackets = new List<Packet>();
            outgoingPackets = new List<Packet>();
        }

        public override void DrawLayer0(Graphics graphics)
        {
            if (drawBackground == false)
            {
                return;
            }

            SolidBrush brush = new SolidBrush(backgroundColour);

            Point[] points = GetVertices();

            graphics.FillPolygon(brush, points);
        }

        public override void DrawLayer2(Graphics graphics)
        {
            Pen pen = new Pen(new SolidBrush(mainColour));

            Point[] points = GetVertices();

            graphics.DrawLine(pen, points[0], points[1]);
            graphics.DrawLine(pen, points[2], points[3]);
        }

        public Point GetExitPoint(NetworkNode node)
        {
            Point center = node.GetDrawLocation();
            int centerX = center.X;
            int centerY = center.Y;

            Point otherCenter;
            if (node.Equals(start))
            {
                otherCenter = end.GetDrawLocation();
            }
            else
            {
                otherCenter = start.GetDrawLocation();
            }

            double absAngle = Program.GetAbsAngle(center, otherCenter);

            int radius = (int)Math.Round(node.size / metersPerPixel, 0);
            double distanceFromCenterToExitPoint = Math.Sqrt(Math.Pow(radius, 2) - 0.25 * Math.Pow(size / metersPerPixel, 2));

            double deltaXFromCenterToExitPoint = distanceFromCenterToExitPoint * Program.Sin(absAngle);
            double deltaYFromCenterToExitPoint = distanceFromCenterToExitPoint * Program.Cos(absAngle);

            int exitPointX = centerX + (int)Math.Round(deltaXFromCenterToExitPoint, 0);
            int exitPointY = centerY + (int)Math.Round(deltaYFromCenterToExitPoint, 0);

            Point exitPoint = new Point(exitPointX, exitPointY);
            return exitPoint;
        }

        public Point[] GetVertices()
        {
            List<Point> allPoints = new List<Point>();

            Point center1 = start.GetDrawLocation();
            int center1X = center1.X;
            int center1Y = center1.Y;

            Point center2 = end.GetDrawLocation();
            int center2X = center2.X;
            int center2Y = center2.Y;

            double absAngle1 = Program.GetAbsAngle(center1, center2);

            int radius1 = (int)Math.Round(start.size / 2 / metersPerPixel, 0);
            double distanceFromCenter1ToLinePoint1Point3 = Math.Sqrt(Math.Pow(radius1, 2) - 0.25 * Math.Pow(size / metersPerPixel, 2));

            double deltaXFromCenter1ToPoint1 = distanceFromCenter1ToLinePoint1Point3 * Program.Sin(absAngle1) - 0.5 * size / metersPerPixel * Program.Cos(absAngle1);
            double deltaYFromCenter1ToPoint1 = distanceFromCenter1ToLinePoint1Point3 * Program.Cos(absAngle1) + 0.5 * size / metersPerPixel * Program.Sin(absAngle1);

            int point1X = center1X + (int)Math.Round(deltaXFromCenter1ToPoint1, 0);
            int point1Y = center1Y + (int)Math.Round(deltaYFromCenter1ToPoint1, 0);

            Point point1 = new Point(point1X, point1Y);
            allPoints.Add(point1);

            double absAngle2 = (absAngle1 + 180) % 360;

            int radius2 = (int)Math.Round(end.size / 2 / metersPerPixel, 0);
            double distanceFromCenter2ToLinePoint2Point3 = Math.Sqrt(Math.Pow(radius2, 2) - 0.25 * Math.Pow(size / metersPerPixel, 2));

            double deltaXFromCenter2ToPoint2 = distanceFromCenter2ToLinePoint2Point3 * Program.Sin(absAngle2) + 0.5 * size / metersPerPixel * Program.Cos(absAngle2);
            double deltaYFromCenter2ToPoint2 = distanceFromCenter2ToLinePoint2Point3 * Program.Cos(absAngle2) - 0.5 * size / metersPerPixel * Program.Sin(absAngle2);

            int point2X = center2X + (int)Math.Round(deltaXFromCenter2ToPoint2, 0);
            int point2Y = center2Y + (int)Math.Round(deltaYFromCenter2ToPoint2, 0);

            Point point2 = new Point(point2X, point2Y);
            allPoints.Add(point2);

            double deltaXFromCenter2ToPoint3 = distanceFromCenter2ToLinePoint2Point3 * Program.Sin(absAngle2) - 0.5 * size / metersPerPixel * Program.Cos(absAngle2);
            double deltaYFromCenter2ToPoint3 = distanceFromCenter2ToLinePoint2Point3 * Program.Cos(absAngle2) + 0.5 * size / metersPerPixel * Program.Sin(absAngle2);

            int point3X = center2X + (int)Math.Round(deltaXFromCenter2ToPoint3, 0);
            int point3Y = center2Y + (int)Math.Round(deltaYFromCenter2ToPoint3, 0);

            Point point3 = new Point(point3X, point3Y);
            allPoints.Add(point3);

            double deltaXFromCenter1ToPoint4 = distanceFromCenter1ToLinePoint1Point3 * Program.Sin(absAngle1) + 0.5 * size / metersPerPixel * Program.Cos(absAngle1);
            double deltaYFromCenter1ToPoint4 = distanceFromCenter1ToLinePoint1Point3 * Program.Cos(absAngle1) - 0.5 * size / metersPerPixel * Program.Sin(absAngle1);

            int point4X = center1X + (int)Math.Round(deltaXFromCenter1ToPoint4, 0);
            int point4Y = center1Y + (int)Math.Round(deltaYFromCenter1ToPoint4, 0);

            Point point4 = new Point(point4X, point4Y);
            allPoints.Add(point4);

            return allPoints.ToArray();
        }

        public override bool DetectCollision(Entity other)
        {
            Point[] vertices = GetVertices();

            Rectangle otherBounds = other.GetDrawBounds();
            int radius = otherBounds.Width;

            for (int i = 0; i < vertices.Length; i++)
            {
                Point point1 = vertices[i];
                int nextIndex = (i + 1) % vertices.Length;
                Point point2 = vertices[nextIndex];

                double distanceToLine = Program.GetMinDistanceToLineSegment(point1, point2, other.GetDrawLocation());

                if (distanceToLine > radius)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
