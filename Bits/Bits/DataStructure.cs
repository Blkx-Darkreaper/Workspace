using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public abstract class DataStructure : Entity
    {
        public Point Center { get; protected set; }

        public DataStructure(Point center)
            : base()
        {
            this.Center = center;
        }

        public DataStructure(int x, int y) : this(new Point(x, y)) { }

        public virtual Size GetSize()
        {
            return new Size(1, 1);
        }

        public virtual Rectangle GetBounds()
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            return GetBounds(width, height);
        }

        public virtual Rectangle GetBounds(int width, int height)
        {
            int cornerX = Center.X - width / 2;
            int cornerY = Center.Y - height / 2;

            Rectangle bounds = new Rectangle(cornerX, cornerY, width, height);
            return bounds;
        }
    }
}