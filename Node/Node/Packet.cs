using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Node
{
    public class Packet : Entity
    {
        public Packet(Point location, int size, Color mainColour) : base(location, size, mainColour) { }

        public Packet(Point location, int size, Color mainColour, Color backgroundColour) : base(location, size, mainColour, backgroundColour) { }

        public override void DrawLayer2(Graphics graphics)
        {
            DrawLine(graphics, mainColour, 1);
        }
    }
}
