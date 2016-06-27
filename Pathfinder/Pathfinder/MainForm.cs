using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinder
{
    public partial class MainForm : Form
    {
        protected int gridsWide { get; set; }
        protected int gridsHigh { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void RandomMap_Click(object sender, EventArgs e)
        {
            //gridsWide = 5;
            //gridsHigh = 5;
            gridsWide = 25;
            gridsHigh = 20;

            int width = gridsWide * Grid.SIZE;
            int height = gridsHigh * Grid.SIZE;

            int min = 1;
            int max = 9;

            Random random = new Random();

            Program.GenerateRandomMap(gridsWide, gridsHigh, random, min, max);
            Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            Program.DrawMap(image, Color.Black, 1);

            MapDisplay.Image = image;

            FindPath.Enabled = true;
        }

        private void SMap_Click(object sender, EventArgs e)
        {
            gridsWide = 9;
            gridsHigh = 10;

            int width = gridsWide * Grid.SIZE;
            int height = gridsHigh * Grid.SIZE;

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

            Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            Program.DrawMap(image, Color.Black, 1);

            MapDisplay.Image = image;

            FindPath.Enabled = true;
        }

        private void FindPath_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap)MapDisplay.Image;

            Grid origin = Program.allGrids[0];
            Grid destination = Program.allGrids[Program.allGrids.Count - 1];
            Point[] allPoints = Program.GetPath(origin, destination, gridsWide, gridsHigh);

            Program.DrawPath(image, Color.Red, 2, allPoints);

            MapDisplay.Refresh();

            FindPath.Enabled = false;
        }
    }
}
