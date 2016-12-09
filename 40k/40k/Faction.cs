using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Faction
    {
        public string name { get; private set; }

        public Faction(string inName)
        {
            name = inName;
        }
    }
}
