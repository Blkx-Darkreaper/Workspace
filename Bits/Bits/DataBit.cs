using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class DataBit : Data
    {
        protected int radius { get; set; }

        public DataBit(Point center, float cruiseVelocity, float maxVelocity)
            : base(center, 1, cruiseVelocity, maxVelocity)
        {
            radius = 3;
        }

        public DataBit(int x, int y, float cruiseVelocity, float maxVelocity) : this(new Point(x, y), cruiseVelocity, maxVelocity) { }

        //public Bit(Branches startPath, int MAX_VELOCITY) : base(startPath, 1, MAX_VELOCITY) {
        //    radius = 3;
        //}

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            if (isHidden == true)
            {
                return;
            }

            Program.FillCircle(graphics, colour, Center, radius);
            Program.DrawCircle(graphics, colour, Center, radius);
        }
    }
}
