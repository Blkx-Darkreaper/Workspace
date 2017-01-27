using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Gate : Component
    {
        public bool IsOpen { get; protected set; }
        protected int facing { get; set; }

        public Gate(Point center)
            : base(center)
        {
            this.IsOpen = true;
            UpdateFacing();
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            base.Draw(graphics, colour, display);

            int radius = 8;
            Program.FillCircle(graphics, Color.White, Center, radius);
            Program.DrawCircle(graphics, colour, Center, radius);

            //if (facing == -1)
            //{
            //    return;
            //}

            int width = radius;
            int lineThickness = 2;
            int copies = 2;
            Program.DrawChevrons(graphics, colour, display, Center, facing, width, lineThickness, copies);
        }

        public virtual void SetIsOpen(bool isOpen)
        {
            this.IsOpen = isOpen;
            UpdateFacing();
        }

        public virtual void Toggle()
        {
            this.IsOpen = !this.IsOpen;
            UpdateFacing();
        }

        public virtual void UpdateFacing()
        {
            int bearing;
            if (IsOpen == true)
            {
                bearing = 90;
            }
            else
            {
                bearing = 180;
            }

            this.facing = bearing;
        }
    }
}