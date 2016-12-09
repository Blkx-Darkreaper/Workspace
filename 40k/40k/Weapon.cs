using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Weapon
    {
        public string type { get; set; }
        public const string pistol = "Pistol";
        public const string rapidFire = "Rapid fire";
        public const string assult = "Assault";
        public const string heavy = "Heavy";
        public const string ordnance = "Ordnance";
        public const string blast = "Blast";
        public const string twinLinked = "Twin-linked";
        public const string melta = "Melta";
        public const string barrage = "Barrage";

        public int strength;
        public int armourPiercingRating { get; set; }
        public int strengthBonus { get; set; }

        public Weapon(string inType, int inStrength, int inAPRating, int inStrengthBonus = 0)
        {
            type = inType;
            strength = inStrength;
            armourPiercingRating = inAPRating;
            strengthBonus = inStrengthBonus;
        }

        public int getWeaponStrength(int wieldersStrength)
        {
            int strength = wieldersStrength + strengthBonus;
            return wieldersStrength;
        }
    }
}
