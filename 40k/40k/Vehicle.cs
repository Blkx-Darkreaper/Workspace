using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Vehicle : Model
    {
        public bool isDestroyed { get; set; }
        public bool isDamaged { get; set; }
        public string speedType { get; private set; }
        public string currentSpeed { get; private set; }

        public int frontArmour { get; private set; }

        public LinkedList<Weapon> offensiveWeapons { get; set; }
        public LinkedList<Weapon> defensiveWeapons { get; set; }

        public Vehicle(string inName, string inType, string inSpeedType, int inWounds, int inStrength, int inToughness, int inBallistic, int inArmour, int inInvulnerable = 0) :
            base(inName, inType, inWounds, inArmour, inInvulnerable)
        {
            isDestroyed = false;
            isDamaged = false;
            speedType = inSpeedType;
            currentSpeed = Global.VehicleSpeed.stationary;

            offensiveWeapons = new LinkedList<Weapon>();
            defensiveWeapons = new LinkedList<Weapon>();
        }

        public void addOffensiveWeapon(Weapon toAdd)
        {
            offensiveWeapons.AddLast(toAdd);
        }

        public Weapon removeOffensiveWeapon(Weapon toRemove)
        {
            offensiveWeapons.Remove(toRemove);
            return toRemove;
        }

        public void addDefensiveWeapon(Weapon toAdd)
        {
            defensiveWeapons.AddLast(toAdd);
        }

        public Weapon removeDefensiveWeapon(Weapon toRemove)
        {
            defensiveWeapons.Remove(toRemove);
            return toRemove;
        }
    }
}
