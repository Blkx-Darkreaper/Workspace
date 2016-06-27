using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Pathfinder
{
    public static class Program
    {
        public static List<Grid> allGrids { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static Point[] GetPath(Grid origin, Grid destination, int gridsWide, int gridsHigh)
        {
            List<Point> path = new List<Point>();
            List<Grid> gridPath = new List<Grid>();

            List<PriorityNode<Grid>> allNodes = new List<PriorityNode<Grid>>();
            PriorityQueue<Grid> queue = new PriorityQueue<Grid>();
            int distance = 0;
            PriorityNode<Grid> nodeToAdd = new PriorityNode<Grid>(distance, origin);

            allNodes.Add(nodeToAdd);
            queue.Add(nodeToAdd, nodeToAdd);

            PriorityNode<Grid> nextNode = null;
            while (!queue.Empty)
            {
                nextNode = queue.PopMin();
                Grid nextGrid = nextNode.Value;
                if (nextGrid.Equals(destination))
                {
                    break;
                }

                distance = nextNode.Priority;

                List<Grid> adjacentGrids = GetAllAdjacentGrids(nextGrid, gridsWide, gridsHigh);
                foreach (Grid neighbour in adjacentGrids)
                {
                    PriorityNode<Grid> dummyNode = new PriorityNode<Grid>(neighbour);
                    bool hasNode = allNodes.Contains(dummyNode);
                    if (hasNode == false)
                    {
                        nodeToAdd = dummyNode;
                        allNodes.Add(nodeToAdd);
                    }
                    else
                    {
                        int index = allNodes.IndexOf(dummyNode);
                        nodeToAdd = allNodes[index];
                    }

                    int alternative = distance + neighbour.terrain;
                    int neighbourDistance = nodeToAdd.Priority;
                    if (neighbourDistance != -1)
                    {
                        if (alternative >= neighbourDistance)
                        {
                            continue;
                        }
                    }

                    if (queue.Contains(nodeToAdd))
                    {
                        queue.SetPriority(alternative, nodeToAdd);
                    }
                    else
                    {
                        nodeToAdd.Priority = alternative;
                        queue.Add(nodeToAdd);
                    }

                    nodeToAdd.Previous = nextNode;
                }
            }

            while (nextNode != null)
            {
                Grid nextGrid = nextNode.Value;
                Point pointToAdd = nextGrid.center;
                path.Add(pointToAdd);

                nextNode = nextNode.Previous;
            }

            path.Reverse();

            return path.ToArray();
        }

        public static Point[] GetPathThreaded(Grid origin, Grid destination, int gridsWide, int gridsHigh)
        {
            List<Point> path = new List<Point>();
            List<Grid> gridPath = new List<Grid>();

            ConcurrentBag<PriorityNode<Grid>> allNodes = new ConcurrentBag<PriorityNode<Grid>>();
            ConcurrentPriorityQueue<Grid> queue = new ConcurrentPriorityQueue<Grid>();
            int distance = 0;
            PriorityNode<Grid> nodeToAdd = new PriorityNode<Grid>(distance, origin);

            allNodes.Add(nodeToAdd);
            queue.Add(nodeToAdd, nodeToAdd);

            PriorityNode<Grid> nextNode = null;
            while (!queue.Empty)
            {
                nextNode = queue.PopMin();
                Grid nextGrid = nextNode.Value;
                if (nextGrid.Equals(destination))
                {
                    break;
                }

                distance = nextNode.Priority;

                List<Grid> adjacentGrids = GetAllAdjacentGrids(nextGrid, gridsWide, gridsHigh);
                Thread[] threads = new Thread[adjacentGrids.Count];
                for (int i = 0; i < adjacentGrids.Count; i++)
                {
                    Grid neighbour = adjacentGrids[i];
                    threads[i] = new Thread(() => ProcessNode(ref allNodes, ref queue, distance, nextNode, neighbour));
                    threads[i].Start();
                }

                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Join();
                }
            }

            while (nextNode != null)
            {
                Grid nextGrid = nextNode.Value;
                Point pointToAdd = nextGrid.center;
                path.Add(pointToAdd);

                nextNode = nextNode.Previous;
            }

            path.Reverse();

            return path.ToArray();
        }

        private static void ProcessNode(ref ConcurrentBag<PriorityNode<Grid>> allNodes, ref ConcurrentPriorityQueue<Grid> queue, int distance, PriorityNode<Grid> nextNode, Grid neighbour)
        {
            PriorityNode<Grid> nodeToAdd;
            PriorityNode<Grid> dummyNode = new PriorityNode<Grid>(neighbour);
            bool hasNode = allNodes.Contains(dummyNode);
            if (hasNode == false)
            {
                //nodeToAdd = new PriorityNode<Grid>(neighbour);
                nodeToAdd = dummyNode;
                allNodes.Add(nodeToAdd);
            }
            else
            {
                int index = allNodes.ToList().IndexOf(dummyNode);
                nodeToAdd = allNodes.ElementAt(index);
            }

            int alternative = distance + neighbour.terrain;
            int neighbourDistance = nodeToAdd.Priority;
            if (neighbourDistance != -1)
            {
                if (alternative >= neighbourDistance)
                {
                    return;
                }
            }

            if (queue.Contains(nodeToAdd))
            {
                queue.SetPriority(alternative, nodeToAdd);
            }
            else
            {
                nodeToAdd.Priority = alternative;
                queue.Add(nodeToAdd);
            }

            nodeToAdd.Previous = nextNode;
        }

        private static int GetPriority<T>(List<PriorityNode<T>> allNodes, T valueToFind)
        {
            foreach (PriorityNode<T> node in allNodes)
            {
                T valueToCompare = node.Value;

                if (!valueToCompare.Equals(valueToFind))
                {
                    continue;
                }

                int priority = node.Priority;
                return priority;
            }

            return -1;
        }

        private static List<Grid> GetAllAdjacentGrids(Grid grid, int gridsWide, int gridsHigh)
        {
            List<Grid> adjacentGrids = new List<Grid>();

            Grid gridToAdd;

            gridToAdd = GetGridAbove(grid, gridsWide);
            if (gridToAdd != null)
            {
                adjacentGrids.Add(gridToAdd);
            }

            gridToAdd = GetGridBelow(grid, gridsWide, gridsHigh);
            if (gridToAdd != null)
            {
                adjacentGrids.Add(gridToAdd);
            }

            gridToAdd = GetGridToLeft(grid, gridsWide, gridsHigh);
            if (gridToAdd != null)
            {
                adjacentGrids.Add(gridToAdd);
            }

            gridToAdd = GetGridToRight(grid, gridsWide, gridsHigh);
            if (gridToAdd != null)
            {
                adjacentGrids.Add(gridToAdd);
            }

            return adjacentGrids;
        }

        private static int GetGridIndex(Grid grid, int gridsWide)
        {
            Point center = grid.center;

            int column = (center.X - Grid.SIZE / 2) / Grid.SIZE;
            int row = (center.Y - Grid.SIZE / 2) / Grid.SIZE;

            int index = row * gridsWide + column;
            return index;
        }

        public static Grid GetGridAbove(Grid grid, int gridsWide)
        {
            int index = GetGridIndex(grid, gridsWide);

            int aboveIndex = index - gridsWide;

            if (aboveIndex < 0)
            {
                return null;
            }

            Grid gridAbove = allGrids[aboveIndex];
            return gridAbove;
        }

        public static Grid GetGridBelow(Grid grid, int gridsWide, int gridsHigh)
        {
            int index = GetGridIndex(grid, gridsWide);

            int belowIndex = index + gridsWide;

            if (belowIndex > gridsWide * gridsHigh - 1)
            {
                return null;
            }

            Grid belowGrid = allGrids[belowIndex];
            return belowGrid;
        }

        public static Grid GetGridToLeft(Grid grid, int gridsWide, int gridsHigh)
        {
            int index = GetGridIndex(grid, gridsWide);

            int leftIndex = index - 1;

            if (leftIndex < 0)
            {
                return null;
            }

            int indexRow = index / gridsWide;
            int leftIndexRow = leftIndex / gridsWide;

            if (indexRow != leftIndexRow)
            {
                return null;
            }

            Grid leftGrid = allGrids[leftIndex];
            return leftGrid;
        }

        public static Grid GetGridToRight(Grid grid, int gridsWide, int gridsHigh)
        {
            int index = GetGridIndex(grid, gridsWide);

            int rightIndex = index + 1;

            if (rightIndex > gridsWide * gridsHigh - 1)
            {
                return null;
            }

            int indexRow = index / gridsWide;
            int rightIndexRow = rightIndex / gridsWide;

            if (indexRow != rightIndexRow)
            {
                return null;
            }

            Grid rightGrid = allGrids[rightIndex];
            return rightGrid;
        }

        public static void DrawPath(Bitmap image, Color drawColour, int lineThickness, Point[] allPoints)
        {
            Pen pen = new Pen(drawColour, lineThickness);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Draw lines from each of the points
                graphics.DrawLines(pen, allPoints);
            }
        }

        public static void GenerateRandomMap(int gridsWide, int gridsHigh, Random random, int min, int max)
        {
            allGrids = new List<Grid>();

            int totalGrids = gridsWide * gridsHigh;
            for (int i = 0; i < totalGrids; i++)
            {
                int cornerX = (i % gridsWide) * Grid.SIZE;
                int cornerY = (i / gridsWide) * Grid.SIZE;

                Grid gridToAdd = new Grid(cornerX, cornerY, random, min, max);
                allGrids.Add(gridToAdd);
            }
        }

        public static void GenerateMap(int gridsWide, int gridsHigh, int terrain)
        {
            allGrids = new List<Grid>();

            int totalGrids = gridsWide * gridsHigh;
            for (int i = 0; i < totalGrids; i++)
            {
                int cornerX = (i % gridsWide) * Grid.SIZE;
                int cornerY = (i / gridsWide) * Grid.SIZE;

                Grid gridToAdd = new Grid(cornerX, cornerY, terrain);
                allGrids.Add(gridToAdd);
            }
        }

        public static void DrawMap(Bitmap image, Color lineColour, int lineThickness)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Fill image with white
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                graphics.FillRectangle(Brushes.White, rect);

                // Draw all grids
                foreach (Grid grid in allGrids)
                {
                    grid.Draw(graphics, lineColour, lineThickness);
                }
            }
        }
    }
}
