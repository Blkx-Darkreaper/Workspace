using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class RangedWeapon : Weapon
    {
        public int range { get; set; }
        public int shots { get; set; }

        public RangedWeapon(string inType, int inRange, int inShots, int inStrength, int inAPRating)
            : base(inType, inStrength, inAPRating)
        {
            range = inRange;
            shots = inShots;
        }
    }
}
