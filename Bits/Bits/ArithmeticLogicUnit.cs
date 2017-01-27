using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class ArithmeticLogicUnit : Component
    {
        public Point AltInputPoint { get; protected set; }
        protected Point[] allVertices { get; set; }

        public ArithmeticLogicUnit(Point center) : this(center.X, center.Y) { }

        public ArithmeticLogicUnit(int x, int y) : base(x, y) {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            allVertices = new Point[8];
            allVertices[0] = new Point(x, y);

            int sixthWidth = width / 6;
            x += sixthWidth;
            y -= sixthWidth;
            allVertices[1] = new Point(x, y);

            x += sixthWidth;
            AltInputPoint = new Point(x, y);

            x += sixthWidth;
            allVertices[2] = new Point(x, y);

            x -= sixthWidth;
            y += height;
            allVertices[3] = new Point(x, y);

            int thirdWidth = width / 3;
            x -= thirdWidth;
            Output = new OutputConnection(new Point(x, y));

            x -= thirdWidth;
            allVertices[4] = new Point(x, y);

            x -= sixthWidth;
            y -= height;
            allVertices[5] = new Point(x, y);

            x += sixthWidth;
            InputPoint = new Point(x, y);

            x += sixthWidth;
            allVertices[6] = new Point(x, y);

            x += sixthWidth;
            y += sixthWidth;
            allVertices[7] = new Point(x, y);
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            //int dashLength = 1;
            //int spaceLength = 2;
            //Program.DrawDottedPolygon(graphics, colour, allVertices, dashLength, spaceLength);
            Program.DrawPolygon(graphics, colour, allVertices);

            Rectangle bounds = GetBounds();
            Program.DrawText(graphics, colour, "ALU", bounds, (int)Program.Text.Justified.Center, (int)Program.Text.Alignment.Bottom);
        }

        public override Size GetSize()
        {
            Size size = new Size(300, 100);
            return size;
        }
    }
}