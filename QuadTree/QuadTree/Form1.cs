using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuadTree
{
    public partial class Form1 : Form
    {
        protected QuadTree quadTree;

        public Form1()
        {
            InitializeComponent();

            int length = this.Width - 30 * 2;
            this.Height = this.Width;
            //DrawQuadTree(127, 6);
            DrawQuadTree(length, 32);

            DetectCollision();
        }

        public void DrawQuadTree(int length, int totalPoints)
        {
            Display.Width = length;
            Display.Height = length;

            //List<NamedPoint> points = new List<NamedPoint>();
            //points.Add(new NamedPoint("A", 40, 45));
            //points.Add(new NamedPoint("B", 15, 70));
            //points.Add(new NamedPoint("C", 70, 10));
            //points.Add(new NamedPoint("D", 69, 50));
            //points.Add(new NamedPoint("E", 55, 80));
            //points.Add(new NamedPoint("F", 80, 90));

            quadTree = new QuadTree(0, 0, length);
            //string initial = quadTree.ToString();

            int seed = 100;
            Random random = new Random(seed);
            //Random random = new Random();

            char nextChar = 'A';
            for (int i = 0; i < totalPoints; i++)
            {
                int x = random.Next(0, length);
                int y = random.Next(0, length);
                NamedPoint point = new NamedPoint(nextChar.ToString(), x, y);
                quadTree.Insert(point);

                nextChar = (char)(Convert.ToUInt16(nextChar) + 1);
            }

            //foreach (NamedPoint point in points)
            //{
            //    quadTree.Insert(point);
            //}

            //string final = quadTree.ToString();
            //Console.Write(final);

            if (Display.Image == null) Display.Image = new Bitmap(length, length);
            Graphics graphics = Graphics.FromImage(Display.Image);
            quadTree.Draw(graphics);
        }

        public void DetectCollision()
        {
            Graphics graphics = Graphics.FromImage(Display.Image);

            int gridLength = Display.Width / 4;
            Rectangle area = new Rectangle(gridLength, gridLength * 2, gridLength, gridLength);
            graphics.DrawRectangle(Pens.Blue, area);

            List<NamedPoint> collisions = quadTree.GetPointsInArea(area);
            foreach (NamedPoint point in collisions)
            {
                Rectangle bounds = new Rectangle(point.X - 2, point.Y - 2, 4, 4);
                graphics.DrawEllipse(Pens.Red, bounds);

                bool collisionDetected = Program.PointWithinRectangle(area.X, area.Y, area.Width, area.Height, point);
                if (collisionDetected == false)
                {
                    continue;
                }
                
                graphics.FillEllipse(Brushes.Red, bounds);
            }
        }
    }
}
