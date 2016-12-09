using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Army
    {
        public string name { get; set; }
        public Faction faction { get; private set; }
        public LinkedList<Unit> allUnits { get; set; }

        public Army(string inName, Faction inFaction)
        {
            name = inName;
            faction = inFaction;
            allUnits = new LinkedList<Unit>();
        }

        public void addUnit(Unit toAdd)
        {
            toAdd.armyName = name;
            allUnits.AddLast(toAdd);
        }

        public void removeUnit(Unit toRemove)
        {
            allUnits.Remove(toRemove);
        }

        public Army clone()
        {
            Army cloneArmy = new Army(name, faction);
            foreach (Unit unit in allUnits)
            {
                Unit cloneUnit = unit.clone();
                cloneArmy.addUnit(cloneUnit);
            }

            return cloneArmy;
        }
    }
}
