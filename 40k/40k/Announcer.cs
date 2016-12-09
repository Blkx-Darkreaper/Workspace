using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FortyK
{
    class Announcer
    {
        public struct GeneralActions
        {
            public const string pass = "Passed";
            public const string fail = "Failed";
            public const string score = "Scoring";
            public const string roll = "Rolled";
        }

        public struct ModelActions
        {
            public const string move = "Moved";
            public const string melee = "Attacked";
            public const string wound = "Wounded";
        }

        public struct UnitActions
        {
            public const string assault = "Is assaulting";
        }

        public static void announceAction(string action)
        {
            string output = string.Format("{0}.", action);
            Debug.WriteLine(output);
        }

        public static void announceUnitAction(Unit subject, string action, Unit other)
        {
            string armyName = subject.armyName;
            string name = subject.name;
            string otherArmy = other.armyName;
            string otherName = other.name;
            string output = string.Format("{0}'s {1} {2} {3}'s {4}.", armyName, name, action.ToLower(), otherArmy, otherName);
            Debug.WriteLine(output);
        }

        public static void announceModelAction(Model subject, string action)
        {
            string name = subject.name;
            string output = string.Format("{0} {1}.", name, action.ToLower());
            Debug.WriteLine(output);
        }

        public static void announceModelAction(Model subject, string action, Model other)
        {
            string name = subject.name;
            string otherName = other.name;
            string output = string.Format("{0} {1} {2}.", name, action.ToLower(), otherName);
            Debug.WriteLine(output);
        }
    }
}
