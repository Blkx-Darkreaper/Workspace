using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class LocationMap
    {
        public List<Location> allLocations { get; set; }
        public Location start { get; set; }
        public Location end { get; set; }

        public LocationMap()
        {
            allLocations = new List<Location>();
        }

        public void Add(Location toAdd)
        {
            allLocations.Add(toAdd);
        }

        public void SetStartAndEnd()
        {
            start = allLocations.First();
            end = allLocations.Last();
        }
    }
}
