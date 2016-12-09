using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinder
{
    public partial class Form1 : Form
    {
        public int width { get; set; }
        public int height { get; set; }
        LocationMap allLocations { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Build_Click(object sender, EventArgs e)
        {
            int startX = Convert.ToInt32(startBoxX.Text);
            int startY = Convert.ToInt32(startBoxY.Text);
            int endX = Convert.ToInt32(endBoxX.Text);
            int endY = Convert.ToInt32(endBoxY.Text);

            width = Math.Abs(endX - startX);
            height = Math.Abs(endY - startY);

            findPathButton.Enabled = true;

            allLocations = new LocationMap();
            int mapWidth = map.Width;
            int mapHeight = map.Height;
            map.Controls.Clear();

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    int x = 50 + mapWidth / width * w;
                    int y = 50 + mapHeight / height * h;
                    Point nextPoint = new Point(x, y);

                    Location toAdd = new Location(nextPoint);
                    int difficulty = toAdd.difficulty;
                    allLocations.Add(toAdd);

                    Label terrain = new Label();
                    terrain.BringToFront();
                    terrain.Text = difficulty.ToString();
                    terrain.AutoSize = true;

                    int labelWidth = terrain.Width;
                    int labelHeight = terrain.Height;

                    terrain.Location = nextPoint;

                    map.Controls.Add(terrain);
                }
            }

            allLocations.SetStartAndEnd();
        }

        private void FindPath_Click(object sender, EventArgs e)
        {
            Pathfinder pathfinder = new Pathfinder();
            List<Point> bestPath = pathfinder.findBestPath(allLocations);
            Point firstPoint = bestPath[0];
            Graphics canvas = map.CreateGraphics();
            Pen pen = new Pen(Color.Red);

            for (int i = 1; i < bestPath.Count; i++ )
            {
                Point nextPoint = bestPath[i];
                canvas.DrawLine(pen, firstPoint, nextPoint);
                firstPoint = nextPoint;
            }

            findPathButton.Enabled = false;
        }

        private void Box_TextChanged(object sender, EventArgs e)
        {
            findPathButton.Enabled = false;
        }
    }
}
