using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Node
{
    public class Data : Entity
    {
        public Data(Point location, int size, Color mainColour) : base(location, size, mainColour) { }

        public Data(int x, int y, int size, Color mainColour) : this(new Point(x, y), size, mainColour) { }

        public Data(Point location, int size, Color mainColour, Color backgroundColour) : base(location, size, mainColour, backgroundColour) { }

        public Data(int x, int y, int size, Color mainColour, Color backgroundColour) : this(new Point(x, y), size, mainColour, backgroundColour) { }

        public override void DrawLayer2(Graphics graphics)
        {
            DrawSquare(graphics, mainColour, 2);
        }
    }
}
