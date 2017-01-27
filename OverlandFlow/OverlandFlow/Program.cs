using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverlandFlow
{
    static class Program
    {
        public static Timer Timer { get; set; }
        private static List<Grid> allGrids { get; set; }
        private static int gridLengthPixels { get; set; }
        private static float pixelsPerMeter { get; set; }
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

        public static void GenerateTerrain(int displayWidth, int[] gridHeights)
        {
            int totalGrids = gridHeights.Length;
            gridLengthPixels = displayWidth / totalGrids;

            pixelsPerMeter = gridLengthPixels / (float)Grid.Length;

            allGrids = new List<Grid>(totalGrids);
            Grid previous = null;

            foreach (int height in gridHeights)
            {
                Grid gridToAdd = new Grid(height);
                allGrids.Add(gridToAdd);

                if (previous == null)
                {
                    previous = gridToAdd;
                    continue;
                }

                previous.Next = gridToAdd;
                previous = gridToAdd;
            }

            // Testing
            //allGrids[0].AddWater(400, 1);
            //allGrids[1].AddWater(200, 1);
            //allGrids[2].AddWater(325, 1);
            // End Testing
        }

        public static void AddWater(float water, float flowSpeed)
        {
            Grid firstGrid = allGrids[0];

            firstGrid.AddWater(water, flowSpeed);
        }

        public static void WaterFlow(float timeElapsed, bool isDraining)
        {
            // Update grids
            foreach (Grid grid in allGrids)
            {
                grid.RemoveWater(timeElapsed, isDraining);
            }
        }

        public static void DrawTerrain(Graphics graphics, int bottom)
        {
            Brush grey = Brushes.Gray;
            Brush blue = Brushes.Blue;

            Font font = new Font("Arial", 10f, FontStyle.Regular);
            int x = 0;
            int bedrock = bottom - 40;

            foreach (Grid grid in allGrids)
            {
                // Display Water depth and flow speed
                float depth = grid.GetDepth();
                string waterDepthText = string.Format("{0:0.0} m", depth);
                graphics.DrawString(waterDepthText, font, blue, new PointF(x, bottom - 15));

                string flowSpeedText = string.Format("{0:0.0} m/s", grid.FlowSpeed);
                graphics.DrawString(flowSpeedText, font, grey, new PointF(x, bottom - 30));

                int height = grid.Height;
                int pixelHeight = (int)pixelsPerMeter * height;

                int y = bedrock - pixelHeight;

                Rectangle bounds = new Rectangle(x, y, gridLengthPixels, pixelHeight);
                graphics.FillRectangle(grey, bounds);

                // Draw water
                int pixelDepth = (int)(pixelsPerMeter * depth);

                if (pixelDepth <= 0)
                {
                    x += gridLengthPixels;
                    continue;
                }

                y -= pixelDepth;

                bounds = new Rectangle(x, y, gridLengthPixels, pixelDepth);
                graphics.FillRectangle(blue, bounds);

                // Increment for next grid
                x += gridLengthPixels;
            }
        }
    }
}