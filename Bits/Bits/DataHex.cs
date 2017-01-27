using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    class DataHex : DataByte
    {
        public DataHex(Point center, float cruiseVelocity, float maxVelocity)
            : base(center, 64, cruiseVelocity, maxVelocity)
        {
            this.facing = 0;
            this.rotationSpeed = 0;
            this.width = 10;
        }

        public DataHex(int x, int y, float cruiseVelocity, float maxVelocity) : base(new Point(x, y), cruiseVelocity, maxVelocity) { }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            if (isHidden == true)
            {
                return;
            }

            Program.FillHexagon(graphics, colour, Center, facing, width);
            Program.DrawHexagon(graphics, colour, Center, facing, width);
        }
    }
}
