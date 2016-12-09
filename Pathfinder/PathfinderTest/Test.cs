using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Diagnostics;
using Pathfinder;

namespace PathfinderTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void GetAdjacentGrids()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            Random random = new Random();
            Program.GenerateRandomMap(gridsWide, gridsHigh, random, 1, 9);

            Grid two = Program.allGrids[2];
            Grid seven = Program.allGrids[7];
            Grid eight = Program.allGrids[8];
            Grid twelve = Program.allGrids[12];
            Grid six = Program.allGrids[6];

            Grid gridToCheck = Program.GetGridAbove(seven, gridsWide);
            Assert.AreEqual(two, gridToCheck, string.Format("Expected {0}, Actual {1}", two, gridToCheck));

            gridToCheck = Program.GetGridBelow(seven, gridsWide, gridsHigh);
            Assert.AreEqual(twelve, gridToCheck, string.Format("Expected {0}, Actual {1}", twelve, gridToCheck));

            gridToCheck = Program.GetGridToRight(seven, gridsWide, gridsHigh);
            Assert.AreEqual(eight, gridToCheck, string.Format("Expected {0}, Actual {1}", eight, gridToCheck));

            gridToCheck = Program.GetGridToLeft(seven, gridsWide, gridsHigh);
            Assert.AreEqual(six, gridToCheck, string.Format("Expected {0}, Actual {1}", six, gridToCheck));
        }

        [TestMethod]
        public void GetAdjacentGridsNoLeft()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            Random random = new Random();
            Program.GenerateRandomMap(gridsWide, gridsHigh, random, 1, 9);

            Grid zero = Program.allGrids[0];
            Grid five = Program.allGrids[5];
            Grid six = Program.allGrids[6];
            Grid ten = Program.allGrids[10];

            Grid gridToCheck = Program.GetGridAbove(five, gridsWide);
            Assert.AreEqual(zero, gridToCheck, string.Format("Expected {0}, Actual {1}", zero, gridToCheck));

            gridToCheck = Program.GetGridBelow(five, gridsWide, gridsHigh);
            Assert.AreEqual(ten, gridToCheck, string.Format("Expected {0}, Actual {1}", ten, gridToCheck));

            gridToCheck = Program.GetGridToRight(five, gridsWide, gridsHigh);
            Assert.AreEqual(six, gridToCheck, string.Format("Expected {0}, Actual {1}", six, gridToCheck));

            gridToCheck = Program.GetGridToLeft(five, gridsWide, gridsHigh);
            Assert.AreEqual(null, gridToCheck, string.Format("Expected {0}, Actual {1}", null, gridToCheck));
        }

        [TestMethod]
        public void GetAdjacentGridsNoRightOrBelow()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            Random random = new Random();
            Program.GenerateRandomMap(gridsWide, gridsHigh, random, 1, 9);

            Grid fourteen = Program.allGrids[14];
            Grid nineteen = Program.allGrids[19];
            Grid eighteen = Program.allGrids[18];

            Grid gridToCheck = Program.GetGridAbove(nineteen, gridsWide);
            Assert.AreEqual(fourteen, gridToCheck, string.Format("Expected {0}, Actual {1}", fourteen, gridToCheck));

            gridToCheck = Program.GetGridBelow(nineteen, gridsWide, gridsHigh);
            Assert.AreEqual(null, gridToCheck, string.Format("Expected {0}, Actual {1}", null, gridToCheck));

            gridToCheck = Program.GetGridToRight(nineteen, gridsWide, gridsHigh);
            Assert.AreEqual(null, gridToCheck, string.Format("Expected {0}, Actual {1}", null, gridToCheck));

            gridToCheck = Program.GetGridToLeft(nineteen, gridsWide, gridsHigh);
            Assert.AreEqual(eighteen, gridToCheck, string.Format("Expected {0}, Actual {1}", eighteen, gridToCheck));
        }

        [TestMethod]
        public void FindRandomPath()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            int min = 1;
            int max = 9;

            Random random = new Random();

            Program.GenerateRandomMap(gridsWide, gridsHigh, random, min, max);

            Grid origin = Program.allGrids[0];
            Grid destination = Program.allGrids[Program.allGrids.Count - 1];
            Point[] allPoints = Program.GetPath(origin, destination, gridsWide, gridsHigh);

            Assert.IsTrue(allPoints.Length > 0, String.Format("Actual {0}", allPoints.Length));

            foreach (Point point in allPoints)
            {
                Console.WriteLine(point.ToString());
            }
        }

        [TestMethod]
        public void FindSPath()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            Program.GenerateMap(gridsWide, gridsHigh, 1);

            // Build barriers
            Program.allGrids[1].terrain = 9;
            Program.allGrids[6].terrain = 9;
            Program.allGrids[11].terrain = 9;

            Program.allGrids[8].terrain = 9;
            Program.allGrids[13].terrain = 9;
            Program.allGrids[18].terrain = 9;

            // Find path
            Grid origin = Program.allGrids[0];
            Grid destination = Program.allGrids[Program.allGrids.Count - 1];
            Point[] allPoints = Program.GetPath(origin, destination, gridsWide, gridsHigh);

            int expectedPoints = 14;
            Assert.IsTrue(allPoints.Length == expectedPoints, String.Format("Expected {0}, Actual {1}", expectedPoints, allPoints.Length));

            Point pointToCheck;
            int expectedX = 16;
            int expectedY = 16;

            pointToCheck = allPoints[0];
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[1];
            expectedY = 48;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[2];
            expectedY = 80;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[3];
            expectedY = 112;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[4];
            expectedX = 48;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[5];
            expectedX = 80;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[6];
            expectedY = 80;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[7];
            expectedY = 48;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[8];
            expectedY = 16;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[9];
            expectedX = 112;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[10];
            expectedX = 144;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[11];
            expectedY = 48;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[12];
            expectedY = 80;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));

            pointToCheck = allPoints[13];
            expectedY = 112;
            Assert.IsTrue(pointToCheck.X == expectedX, String.Format("Expected {0}, Actual {1}", expectedX, pointToCheck.X));
            Assert.IsTrue(pointToCheck.Y == expectedY, String.Format("Expected {0}, Actual {1}", expectedY, pointToCheck.Y));
        }

        [TestMethod]
        public void FindLargeSPath()
        {
            int gridsWide = 9;
            int gridsHigh = 10;

            Program.GenerateMap(gridsWide, gridsHigh, 1);

            // Build barriers
            for (int i = 1; i < 82; i += gridsWide)
            {
                Program.allGrids[i].terrain = 99;
            }
            for (int i = 12; i <= 84; i += gridsWide)
            {
                Program.allGrids[i].terrain = 99;
            }
            for (int i = 5; i < 86; i += gridsWide)
            {
                Program.allGrids[i].terrain = 99;
            }
            for (int i = 16; i <= 88; i += gridsWide)
            {
                Program.allGrids[i].terrain = 99;
            }

            // Find path
            Grid origin = Program.allGrids[0];
            Grid destination = Program.allGrids[Program.allGrids.Count - 1];
            Point[] allPoints = Program.GetPath(origin, destination, gridsWide, gridsHigh);

            int expectedPoints = 54;
            Assert.IsTrue(allPoints.Length == expectedPoints, String.Format("Expected {0}, Actual {1}", expectedPoints, allPoints.Length));
        }

        [TestMethod]
        public void FindPathTiming()
        {
            int gridsWide = 5;
            int gridsHigh = 4;

            Program.GenerateMap(gridsWide, gridsHigh, 1);

            // Build barriers
            Program.allGrids[1].terrain = 9;
            Program.allGrids[6].terrain = 9;
            Program.allGrids[11].terrain = 9;

            Program.allGrids[8].terrain = 9;
            Program.allGrids[13].terrain = 9;
            Program.allGrids[18].terrain = 9;

            Stopwatch timer = new Stopwatch();
            int expectedPoints = 14;

            // Find path
            Grid origin = Program.allGrids[0];
            Grid destination = Program.allGrids[Program.allGrids.Count - 1];

            // Unthreaded test
            timer.Start();
            Point[] allPoints = Program.GetPath(origin, destination, gridsWide, gridsHigh);
            timer.Stop();

            Assert.IsTrue(allPoints.Length == expectedPoints, String.Format("Expected {0}, Actual {1}", expectedPoints, allPoints.Length));

            TimeSpan unthreadedTime = timer.Elapsed;
            Console.WriteLine(String.Format("{0:00}:{1:00}.{2:00}", unthreadedTime.Minutes, unthreadedTime.Seconds, unthreadedTime.Milliseconds / 10));

            // Threaded test
            timer.Start();
            allPoints = Program.GetPathThreaded(origin, destination, gridsWide, gridsHigh);
            timer.Stop();

            Assert.IsTrue(allPoints.Length == expectedPoints, String.Format("Expected {0}, Actual {1}", expectedPoints, allPoints.Length));

            TimeSpan threadedTime = timer.Elapsed;
            Console.WriteLine(String.Format("{0:00}:{1:00}.{2:00}", threadedTime.Minutes, threadedTime.Seconds, threadedTime.Milliseconds / 10));

            Assert.IsTrue(threadedTime < unthreadedTime);
        }
    }
}
