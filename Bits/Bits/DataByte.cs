using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class DataByte : Data
    {
        protected int rotationSpeed { get; set; }
        protected int width { get; set; }

        public DataByte(Point center, float cruiseVelocity, float maxVelocity) : this(center, 8, cruiseVelocity, maxVelocity) { }

        public DataByte(Point center, int value, float cruiseVelocity, float maxVelocity)
            : base(center, value, cruiseVelocity, maxVelocity)
        {
            this.facing = 45;
            this.rotationSpeed = 20;
            this.width = 8;
        }

        public DataByte(int x, int y, float cruiseVelocity, float maxVelocity) : base(x, y, 8, cruiseVelocity, maxVelocity) { }

        public DataByte(int x, int y, int value, float cruiseVelocity, float maxVelocity) : this(new Point(x, y), value, cruiseVelocity, maxVelocity) { }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            if (isHidden == true)
            {
                return;
            }

            Program.FillSquare(graphics, colour, Center, facing, width);
            Program.DrawSquare(graphics, colour, Center, facing, width);
        }

        public override void Update(float timeElapsed)
        {
            Turn(rotationSpeed);

            base.Update(timeElapsed);
        }
    }
}
