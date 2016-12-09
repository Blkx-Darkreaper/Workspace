using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Pathfinder
{
    class Pathfinder
    {
        public Matrix activePaths { get; set; }
        public List<Path> allPaths { get; set; }
        public Point endPoint { get; set; }

        public List<Point> findBestPath(LocationMap allLocations) {
            Point startPoint = allLocations.start.position;
            endPoint = allLocations.end.position;

            int vertical = Path.GetVerticalDirection(startPoint, endPoint);
            int horizontal = Path.GetHorizontalDirection(startPoint, endPoint);

            Path pathA = new Path(this, null, vertical);
            Path pathB = new Path(this, null, horizontal);
            Thread threadA = new Thread(new ThreadStart(pathA.findPath));
            threadA.Join();
            Thread threadB = new Thread(new ThreadStart(pathB.findPath));
            threadB.Join();

            List<Point> bestPath = new List<Point>();
            return bestPath;
        }

        public void addPath(Path toAdd)
        {
            allPaths.Add(toAdd);
        }

        public void takePointIfAvailable(Point toTake, Path takingPoint)
        {
            bool pointAvailable = checkPointAvailable(toTake);

            if (pointAvailable == false)
            {
                return;
            }

            takeAvailablePoint(toTake, takingPoint);
        }

        public bool checkPointAvailable(Point toCheck)
        {
            return activePaths.checkPointAvailable(toCheck);
        }

        public void takeAvailablePoint(Point toTake, Path takingPoint)
        {
            activePaths.takeAvailablePoint(toTake, takingPoint);
        }

        public class Matrix
        {
            public Path[,] allPoints { get; set; }

            public Matrix(int width, int height)
            {
                allPoints = new Path[width, height];
            }

            public bool checkPointAvailable(Point toCheck)
            {
                int x = toCheck.X;
                int y = toCheck.Y;

                Path foundPath = allPoints[x, y];
                if (foundPath != null)
                {
                    return false;
                }

                return true;
            }

            public void takeAvailablePoint(Point toTake, Path takingPoint)
            {
                int x = toTake.X;
                int y = toTake.Y;

                allPoints[x, y] = takingPoint;
            }
        }

        public class Path
        {
            public Pathfinder parent { get; set; }
            public string name { get; set; }
            public Path start { get; set; }
            public Path end { get; set; }

            public const int UP = 0;
            public const int RIGHT = 1;
            public const int DOWN = 2;
            public const int LEFT = 3;
            public const int NO_DIRECTION = 4;
            public int direction { get; set; }
            public int distanceTravelled { get; set; }

            public List<Point> allUnvisitedPoints { get; set; }
            public List<Point> allVisitedPoints { get; set; }
            public bool endReached { get; set; }

            public Path(Pathfinder inParent, Path inPrevious, int inDirection)
            {
                parent = inParent;
                direction = inDirection;
                distanceTravelled = 0;
                allVisitedPoints = new List<Point>();
                endReached = false;
            }

            public Path(Pathfinder inParent, List<Point> inAllUnvisitedPoints, Path inStart, int inDirection)
            {
                parent = inParent;
                start = inStart;
                direction = inDirection;
                distanceTravelled = 0;
                allUnvisitedPoints = inAllUnvisitedPoints;
                allVisitedPoints = new List<Point>();
                endReached = false;
            }

            public void findPath() {
                switch (direction)
                {
                    case UP:
                        break;

                    case DOWN:
                        break;

                    case RIGHT:
                        break;

                    case LEFT:
                        break;

                    default:
                        break;
                }
            }

            private void move() { }

            public static int GetVerticalDirection(Point start, Point end)
            {
                int direction = start.Y - end.Y;
                if (direction == 0)
                {
                    return NO_DIRECTION;
                }

                if (direction < 0)
                {
                    return DOWN;
                }

                return UP;
            }

            public static int GetHorizontalDirection(Point start, Point end)
            {
                int direction = start.X - end.X;
                if (direction == 0)
                {
                    return NO_DIRECTION;
                }

                if (direction < 0)
                {
                    return RIGHT;
                }

                return LEFT;
            }

            private int getBranchDirection(int currentDirection, Point currentPoint, Point endPoint)
            {
                int branchDirection = 0;

                return branchDirection;
            }

            private void spawn() {
                Point currentPoint = allVisitedPoints.Last();
                Point endPoint = parent.endPoint;
                int branchDirection = getBranchDirection(this.direction, currentPoint, endPoint);
                Path branch = new Path(parent, this, branchDirection);
                parent.addPath(branch);
            }

            private void link(Path toLinkTo) {
                end = toLinkTo;
            }

            public int getDistanceFromStart() {
                int totalDistance = distanceTravelled;

                Path previousPath = start;

                while (previousPath != null)
                {
                    int previousDistance = previousPath.distanceTravelled;
                    totalDistance += previousDistance;

                    previousPath = previousPath.start;
                }

                return totalDistance;
            }
        }
    }
}
