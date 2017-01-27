using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Component : Connector
    {
        public Point Center { get; protected set; }
        public Point ControlInputPoint { get; protected set; }

        public Component(Point center) : base(center, center)
        {
            this.Center = center;
            this.ControlInputPoint = center;
        }

        public Component(int x, int y) : this(new Point(x, y)) { }

        public virtual Rectangle GetBounds()
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            return GetBounds(width, height);
        }

        public virtual Rectangle GetBounds(int width, int height)
        {
            Rectangle bounds = new Rectangle(Center.X - width / 2, Center.Y - height / 2, width, height);
            return bounds;
        }

        public virtual Size GetSize()
        {
            return new Size(1, 1);
        }
    }
}