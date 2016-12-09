using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Grid : Terrain
    {
        public decimal size { get; private set; }
        public Point centerLocation { get; private set; }

        public Grid(decimal inSize, Point inLocation, int inElevation = 0, string inCover = Cover.clear, bool inIsDangerous = false, bool inIsImpassable = false)
            : base(inElevation, inCover, inIsDangerous, inIsImpassable)
        {
            size = inSize;
            centerLocation = inLocation;
        }
    }
}
