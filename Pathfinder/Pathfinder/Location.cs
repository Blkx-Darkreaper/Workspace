using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pathfinder
{
    class Location
    {
        public int difficulty { get; set; }
        public Point position { get; set; }

        public Location(Point inPosition)
        {
            position = inPosition;
            difficulty = getRandom(1, 20);
        }

        public Location(Point inPosition, int inDifficulty)
            : this(inPosition)
        {
            difficulty = inDifficulty;
        }

        public int getRandom(int min, int max)
        {
            int random = (int)new Random().Next(min, max);
            return random;
        }
    }
}
