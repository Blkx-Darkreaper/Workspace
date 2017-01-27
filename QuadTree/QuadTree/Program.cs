using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace QuadTree
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static bool PointWithinSquare(Rectangle bounds, NamedPoint point)
        {
            return Program.PointWithinRectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height, point);
        }

        public static bool PointWithinRectangle(int x, int y, int width, int height, NamedPoint point)
        {
            if (point == null)
            {
                return false;
            }

            var pointX = point.X;
            var pointY = point.Y;

            if (pointX < x)
            {
                return false;
            }
            if (pointY < y)
            {
                return false;
            }

            var outerX = x + width;
            if (pointX > outerX)
            {
                return false;
            }

            var outerY = y + height;
            if (pointY > outerY)
            {
                return false;
            }

            return true;
        }
    }
}