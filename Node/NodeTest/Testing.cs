using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Node;

namespace NodeTest
{
    [TestClass]
    public class Testing
    {
        public bool PointsAreEqual(Point point, Point otherPoint)
        {
            if (point.X != otherPoint.X)
            {
                return false;
            }

            return point.Y == otherPoint.Y;
        }

        [TestMethod]
        public void GetLinkPoints90Degrees()
        {
            Point centerA = new Point(10, 10);
            Point centerB = new Point(30, 10);

            int radius = 5;

            NetworkNode nodeA = new NetworkNode("A", radius, centerA, Color.Red, Color.Navy);
            NetworkNode nodeB = new NetworkNode("B", radius, centerB, Color.Blue, Color.Navy);

            int width = 6;
            Link link = new Link(width, nodeA, nodeB, Color.Green, Color.Navy);

            Point[] allPoints = link.GetVertices();
            Point[] expectedPoints = new Point[] { new Point(14, 13), new Point(26, 13), new Point(14, 7), new Point(26, 7) };

            for (int i = 0; i < 4; i++)
            {
                Point point = allPoints[i];
                Point expectedPoint = expectedPoints[i];

                Assert.IsTrue(PointsAreEqual(point, expectedPoint));
            }
        }

        [TestMethod]
        public void GetLinkPoints45Degrees()
        {
            Point centerA = new Point(10, 10);
            Point centerB = new Point(20, 20);

            int radius = 5;

            NetworkNode nodeA = new NetworkNode("A", radius, centerA, Color.Red, Color.Navy);
            NetworkNode nodeB = new NetworkNode("B", radius, centerB, Color.Blue, Color.Navy);

            int width = 6;
            Link link = new Link(width, nodeA, nodeB, Color.Green, Color.Navy);

            Point[] allPoints = link.GetVertices();
            Point[] expectedPoints = new Point[] { new Point(11, 15), new Point(15, 19), new Point(15, 11), new Point(19, 15) };

            for (int i = 0; i < 4; i++)
            {
                Point point = allPoints[i];
                Point expectedPoint = expectedPoints[i];

                Assert.IsTrue(PointsAreEqual(point, expectedPoint));
            }
        }

        [TestMethod]
        public void GetLinkPoints135Degrees()
        {
            Point centerA = new Point(10, 20);
            Point centerB = new Point(20, 10);

            int radius = 5;

            NetworkNode nodeA = new NetworkNode("A", radius, centerA, Color.Red, Color.Navy);
            NetworkNode nodeB = new NetworkNode("B", radius, centerB, Color.Blue, Color.Navy);

            int width = 6;
            Link link = new Link(width, nodeA, nodeB, Color.Green, Color.Navy);

            Point[] allPoints = link.GetVertices();
            Point[] expectedPoints = new Point[] { new Point(15, 19), new Point(19, 15), new Point(11, 15), new Point(15, 11) };

            for (int i = 0; i < 4; i++)
            {
                Point point = allPoints[i];
                Point expectedPoint = expectedPoints[i];

                Assert.IsTrue(PointsAreEqual(point, expectedPoint));
            }
        }

        [TestMethod]
        public void GetLinkPoints180Degrees()
        {
            Point centerA = new Point(10, 30);
            Point centerB = new Point(10, 10);

            int radius = 5;

            NetworkNode nodeA = new NetworkNode("A", radius, centerA, Color.Red, Color.Navy);
            NetworkNode nodeB = new NetworkNode("B", radius, centerB, Color.Blue, Color.Navy);

            int width = 6;
            Link link = new Link(width, nodeA, nodeB, Color.Green, Color.Navy);

            Point[] allPoints = link.GetVertices();
            Point[] expectedPoints = new Point[] { new Point(13, 26), new Point(13, 14), new Point(7, 26), new Point(7, 14) };

            for (int i = 0; i < 4; i++)
            {
                Point point = allPoints[i];
                Point expectedPoint = expectedPoints[i];

                Assert.IsTrue(PointsAreEqual(point, expectedPoint));
            }
        }

        [TestMethod]
        public void GetLinkPoints270Degrees()
        {
            Point centerA = new Point(30, 10);
            Point centerB = new Point(10, 10);

            int radius = 5;

            NetworkNode nodeA = new NetworkNode("A", radius, centerA, Color.Red, Color.Navy);
            NetworkNode nodeB = new NetworkNode("B", radius, centerB, Color.Blue, Color.Navy);

            int width = 6;
            Link link = new Link(width, nodeA, nodeB, Color.Green, Color.Navy);

            Point[] allPoints = link.GetVertices();
            Point[] expectedPoints = new Point[] { new Point(26, 7), new Point(14, 7), new Point(26, 13), new Point(14, 13) };

            for (int i = 0; i < 4; i++)
            {
                Point point = allPoints[i];
                Point expectedPoint = expectedPoints[i];

                Assert.IsTrue(PointsAreEqual(point, expectedPoint));
            }
        }

        [TestMethod]
        public void GetAbsAngles()
        {
            Point point1 = new Point(20, 20);
            double angle;
            Point point2;

            point2 = new Point(25, 25);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 45);

            point2 = new Point(25, 20);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 90);

            point2 = new Point(25, 15);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 135);

            point2 = new Point(20, 15);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 180);

            point2 = new Point(15, 15);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 225);

            point2 = new Point(15, 20);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 270);

            point2 = new Point(15, 25);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 315);

            point2 = new Point(20, 25);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 0);

            point2 = new Point(20, 20);
            angle = Program.GetAbsAngle(point1, point2);
            Assert.IsTrue(angle == 0);
        }

        [TestMethod]
        public void Orbit()
        {
            Point startPoint = new Point(15,10);
            Entity entity = new Entity(startPoint, Color.Yellow);

            Point centerPoint = new Point(10,10);

            entity.ChangeVelocity(10);

            float timeElapsed = 0.463647609000806f;
            int maxOrbit = 20;
            entity.Orbit(centerPoint, maxOrbit, timeElapsed);

            Point endPoint = entity.location;
            Point expectedPoint = new Point(13, 14);

            Assert.IsTrue(PointsAreEqual(endPoint, expectedPoint));
        }

        [TestMethod]
        public void Bounce()
        {
            Point startPoint = new Point(10, 10);
            Entity entity = new Entity(startPoint, Color.Yellow);

            int angle;
            int bounceAngle;
            int expectedAngle;

            entity.heading = 120;
            angle = 0;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 60;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 225;
            angle = 0;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 315;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 15;
            angle = 0;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 165;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 280;
            angle = 0;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 260;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 90;
            angle = 30;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 30;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 90;
            angle = 45;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 0;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 180;
            angle = 0;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 0;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 90;
            angle = 90;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 270;
            Assert.IsTrue(bounceAngle == expectedAngle);

            entity.heading = 45;
            angle = 45;
            entity.Bounce(angle);
            bounceAngle = entity.heading;
            expectedAngle = 225;
            Assert.IsTrue(bounceAngle == expectedAngle);
        }

        [TestMethod]
        public void CircleInsideRectPolygon()
        {
            Point pointA = new Point(10, 10);
            NetworkNode nodeA = new NetworkNode("A", 5, pointA, Color.Red);

            Point pointB = new Point(40, 10);
            NetworkNode nodeB = new NetworkNode("B", 5, pointB, Color.Red);

            Link link = new Link(6, nodeA, nodeB, Color.Red);

            bool collision;
            Point point;
            Entity entity;

            point = new Point(20, 10);
            entity = new Entity(point, Color.Blue);
            collision = link.DetectCollision(entity);
            Assert.IsTrue(collision == false);

            point = new Point(20, 11);
            entity = new Entity(point, Color.Blue);
            collision = link.DetectCollision(entity);
            Assert.IsTrue(collision == true);

            point = new Point(16, 10);
            entity = new Entity(point, Color.Blue);
            collision = link.DetectCollision(entity);
            Assert.IsTrue(collision == true);
        }

        [TestMethod]
        public void NearestSecantLinePoint()
        {
            Point origin;
            int heading;
            Point circleCenter;
            int radius;
            Point intersect;
            Point expected;

            origin = new Point(0, 0);
            heading = 0;
            circleCenter = new Point(0, 0);
            radius = 5;
            intersect = Program.GetNearestSecantLinePoint(origin, heading, circleCenter, radius);
            expected = new Point(0, -5);
            Assert.IsTrue(PointsAreEqual(intersect, expected));

            origin = new Point(80, 56);
            heading = 0;
            circleCenter = new Point(80, 80);
            radius = 25;
            intersect = Program.GetNearestSecantLinePoint(origin, heading, circleCenter, radius);
            expected = new Point(80, 55);
            Assert.IsTrue(PointsAreEqual(intersect, expected));

            origin = new Point(80, 80);
            heading = 45;
            intersect = Program.GetNearestSecantLinePoint(origin, heading, circleCenter, radius);
            expected = new Point(98, 62);
            Assert.IsTrue(PointsAreEqual(intersect, expected));

            heading = 225;
            intersect = Program.GetNearestSecantLinePoint(origin, heading, circleCenter, radius);
            expected = new Point(62, 98);
            Assert.IsTrue(PointsAreEqual(intersect, expected));
        }

        [TestMethod]
        public void RotatePoint()
        {
            Point center = new Point(80, 80);

            int length = 10;
            int cornerX = center.X - length / 2;
            int cornerY = center.Y - length / 2;
            Point corner = new Point(cornerX, cornerY);
            int heading = 0;

            Point rotatedCorner, expectedCorner;

            rotatedCorner = Program.RotatePointAroundAxis(heading, corner, center);
            expectedCorner = new Point(75, 75);
            Assert.IsTrue(PointsAreEqual(rotatedCorner, expectedCorner));

            heading = 90;
            rotatedCorner = Program.RotatePointAroundAxis(heading, corner, center);
            expectedCorner = new Point(85, 75);

            heading = 180;
            rotatedCorner = Program.RotatePointAroundAxis(heading, corner, center);
            expectedCorner = new Point(85, 85);

            heading = 270;
            rotatedCorner = Program.RotatePointAroundAxis(heading, corner, center);
            expectedCorner = new Point(75, 85);

            heading = 45;
            rotatedCorner = Program.RotatePointAroundAxis(heading, corner, center);
            expectedCorner = new Point(80, 73);
        }
    }
}
