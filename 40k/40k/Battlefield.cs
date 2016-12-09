using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Battlefield
    {
        public static CubicMatrix<Model> allModels { get; set; }
        public static CubicMatrix<Grid> allGrids { get; set; }

        public Battlefield(decimal width, decimal height, decimal depth, decimal gridSize = 0.25m, decimal gridHeight = 0.25m)
        {
            int gridsWide = Convert.ToInt32(width / gridSize);
            int gridsHigh = Convert.ToInt32(height / gridSize);
            int gridsDeep = Convert.ToInt32(depth / gridHeight);

            allModels = new CubicMatrix<Model>(gridsWide, gridsHigh, gridsDeep);
            allGrids = new CubicMatrix<Grid>(gridsWide, gridsWide, gridsDeep);

            generateWorld(gridsWide, gridsHigh, gridSize);
        }

        public class CubicMatrix<T> where T : class
        {
            private int width;
            private int height;
            private int depth;
            private T[, ,] matrix;

            public CubicMatrix(int inWidth, int inHeight, int inDepth)
            {
                width = inWidth;
                height = inHeight;
                depth = inDepth;
                matrix = new T[inWidth, inHeight, inDepth];
            }

            public void Add(T toAdd, int x, int y, int z)
            {
                matrix[x, y, z] = toAdd;
            }

            public void Remove(int x, int y, int z)
            {
                matrix[x, y, z] = null;
            }

            public void Remove(T toRemove)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            T value = matrix[x, y, z];
                            bool match = value.Equals(toRemove);
                            if (match == false)
                            {
                                continue;
                            }

                            matrix[x, y, z] = null;
                        }
                    }
                }
            }

            public IEnumerable<T> Values
            {
                get
                {
                    LinkedList<T> allValues = new LinkedList<T>();
                    foreach (T entry in matrix)
                    {
                        allValues.AddLast(entry);
                    }
                    return allValues.AsEnumerable<T>();
                }
            }

            public T this[int x, int y, int z]
            {
                get
                {
                    return matrix[x, y, z];
                }
            }

            public IEnumerable<T> RangeInclusive(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                LinkedList<T> list = new LinkedList<T>();
                for (int a = x1; a <= x2; a++)
                {
                    for (int b = y1; b <= y2; b++)
                    {
                        for (int c = z1; c <= z2; c++)
                        {
                            T toAdd = matrix[a, b, c];
                            list.AddLast(toAdd);
                        }
                    }
                }

                return list.AsEnumerable<T>();
            }

            public bool Available(int x, int y, int z)
            {
                T toCheck = matrix[x, y, z];
                if (toCheck != null)
                {
                    return false;
                }

                return true;
            }
        }

        protected void generateWorld(int gridsWide, int gridsHigh, decimal gridSize)
        {
        }

        public void addArmy(Army toAdd)
        {
            foreach (Unit unit in toAdd.allUnits)
            {
                foreach (Model member in unit.allUnitMembers)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void removeArmy(Army toRemove)
        {
            foreach (Unit unit in toRemove.allUnits)
            {
                foreach (Model member in unit.allUnitMembers)
                {
                    removeModel(member);
                }
            }
        }

        public bool addModel(Model toAdd, Point spawn, int elevation)
        {
            toAdd.currentLocation = spawn;
            toAdd.currentElevation = elevation;
            int x = spawn.X;
            int y = spawn.Y;

            bool isAvailable = allModels.Available(x, y, elevation);
            if (isAvailable == true)
            {
                allModels.Add(toAdd, x, y, elevation);
            }

            return isAvailable;
        }

        private void removeModel(Model toRemove)
        {
            Point location = toRemove.currentLocation;
            int x = location.X;
            int y = location.Y;
            int z = toRemove.currentElevation;

            allModels.Remove(x, y, z);
        }


        public bool moveModel(Model toMove, Point destination, int z)
        {
            int x = destination.X;
            int y = destination.Y;
            bool isAvailable = allModels.Available(x, y, z);
            if (isAvailable == true)
            {
                Point currentLocation = toMove.currentLocation;
                int xi = currentLocation.X;
                int yi = currentLocation.Y;
                int zi = toMove.currentElevation;
                allModels.Remove(xi, yi, zi);

                toMove.currentLocation = destination;
                toMove.currentElevation = z;
                allModels.Add(toMove, x, y, z);
            }

            return isAvailable;
        }

        public static List<Unit> getEnemySquadsInSight(Point origin, Faction faction)
        {
            throw new NotImplementedException();
        }

        //public static List<Unit> getEnemySquadsInSight(Model origin, Faction faction)
        //{
        //    List<Unit> enemySquadsInSight = new List<Unit>();

        //    foreach (Faction factionToCheck in allUnitsByFaction.Keys)
        //    {
        //        bool match = faction.Equals(factionToCheck);
        //        if (match == true)
        //        {
        //            continue;
        //        }
        //        foreach (Unit unitToCheck in allUnitsByFaction[factionToCheck])
        //        {
        //            bool inSight = false;

        //            foreach (Model modelToCheck in unitToCheck.allUnitMembers)
        //            {
        //                inSight = targetIsInLineOfSight(origin, modelToCheck);
        //                if (inSight == false)
        //                {
        //                    continue;
        //                }

        //                break;
        //            }
        //            if (inSight == false)
        //            {
        //                continue;
        //            }

        //            enemySquadsInSight.Add(unitToCheck);
        //        }
        //    }

        //    return enemySquadsInSight;
        //}

        public static List<Unit> getAllEnemySquadsInRange(Point origin, decimal range, Faction faction)
        {
            throw new NotImplementedException();
        }

        //public static List<Unit> getAllEnemySquadsInRange(Point origin, decimal range, Faction faction)
        //{
        //    List<Unit> enemySquadsInRange = new List<Unit>();

        //    foreach (Faction key in allUnitsByFaction.Keys)
        //    {
        //        bool match = faction.Equals(key);
        //        if (match == true)
        //        {
        //            continue;
        //        }
        //        foreach (Unit toCheck in allUnitsByFaction[key])
        //        {
        //            Point toCheckLocation = toCheck.getLocation();
        //            decimal distance = getDistance(origin, toCheckLocation);
        //            if (distance > range)
        //            {
        //                continue;
        //            }

        //            enemySquadsInRange.Add(toCheck);
        //        }
        //    }

        //    return enemySquadsInRange;
        //}

        public static Unit getNearestEnemySquad(Point origin, Faction faction)
        {
            throw new NotImplementedException();
        }

        //public static Unit getNearestEnemySquad(Point origin, Faction faction)
        //{
        //    Unit nearestEnemySquad = null;
        //    decimal smallestDistance = decimal.MaxValue;

        //    foreach (Faction key in allUnitsByFaction.Keys)
        //    {
        //        bool match = faction.Equals(key);
        //        if (match == true)
        //        {
        //            continue;
        //        }
        //        foreach (Unit toCheck in allUnitsByFaction[key])
        //        {
        //            Point toCheckLocation = toCheck.getLocation();
        //            decimal distance = getDistance(origin, toCheckLocation);
        //            if (distance >= smallestDistance)
        //            {
        //                continue;
        //            }

        //            nearestEnemySquad = toCheck;
        //            smallestDistance = distance;
        //        }
        //    }

        //    return nearestEnemySquad;
        //}

        public static bool targetIsInLineOfSight(Model viewer, Model target)
        {
            Point origin = viewer.currentLocation;
            Point targetLocation = target.currentLocation;
            int viewerElevation = viewer.currentElevation;
            int targetElevation = target.currentElevation;

            int absBearing = Global.getDirection(origin, targetLocation);
            int theta;
            if (absBearing >= 270)
            {
                theta = absBearing - 270;
            }
            else if (absBearing >= 180)
            {
                theta = 270 - absBearing;
            }
            else if (absBearing >= 90)
            {
                theta = absBearing - 90;
            }
            else
            {
                theta = 90 - absBearing;
            }

            decimal distance = Global.getDistance(origin, targetLocation);
            int absDistance = Global.getAbsDistance(distance);

            int deltaElevation = targetElevation - viewerElevation;

            int previousX = origin.X;
            int previousY = origin.Y;
            int previousZ = viewerElevation;
            bool pointClear = true;
            for (int i = 1; i < absDistance; i++)
            {
                int nextX = (int)(Math.Cos(Global.degreesToRadians(theta)) * i);
                int nextY = (int)(Math.Sin(Global.degreesToRadians(theta)) * i);
                int nextZ = i * deltaElevation / absDistance;

                if (previousX == nextX && previousY == nextY && previousZ == nextZ)
                {
                    previousX = nextX;
                    previousY = nextY;
                    previousZ = nextZ;
                    continue;
                }

                pointClear = allModels.Available(nextX, nextY, nextZ);
                pointClear = allGrids.Available(nextX, nextY, nextZ);
                throw new NotImplementedException();

                if (pointClear == false)
                {
                    return false;
                }

                previousX = nextX;
                previousY = nextY;
                previousZ = nextZ;
            }

            return true;
        }
    }
}
