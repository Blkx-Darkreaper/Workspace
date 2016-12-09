using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Global
    {
        public struct Factions
        {
            public static Faction spaceMarines = new Faction("Space Marines");
            public static Faction imperialGuard = new Faction("Imperial Guard");
            public static Faction chaosSpaceMarines = new Faction("Chaos Space Marines");
            public static Faction orks = new Faction("Orks");
            public static Faction eldar = new Faction("Eldar");
            public static Faction darkEldar = new Faction("Dark Eldar");
            public static Faction tau = new Faction("Tau");
            public static Faction tyrannids = new Faction("Tyrannids");
            public static Faction necrons = new Faction("Necrons");
            public static Faction teqs = new Faction("Teqs");
        }

        //public static MultiMap<Faction, Unit> allUnitsByFaction { get; set; }
        //public static MultiMap<Unit, Rectangle> allUnitFootprints { get; set; }

        //public static void addArmy(Army toAdd)
        //{
        //    Faction toAddFaction = toAdd.faction;
        //    foreach (Unit unit in toAdd.allUnits)
        //    {
        //        allUnitsByFaction.Add(toAddFaction, unit);

        //        foreach (Model member in unit.allUnitMembers)
        //        {
        //            decimal baseRadius = member.baseRadius;
        //            int length = 2 * Convert.ToInt32(baseRadius);
        //            Point centerPoint = member.currentLocation;

        //            int topLeftX = centerPoint.X - length / 2;
        //            int topLeftY = centerPoint.Y - length / 2;

        //            Rectangle footprint = new Rectangle(topLeftX, topLeftY, length, length);
        //        }
        //    }
        //}

        class MultiMap<K, V>
        {
            Dictionary<K, LinkedList<V>> dictionary = new Dictionary<K, LinkedList<V>>();

            public void Add(K key, V value)
            {
                LinkedList<V> toAddTo;

                bool containsKey = dictionary.ContainsKey(key);
                if (containsKey == false)
                {
                    toAddTo = new LinkedList<V>();
                    dictionary.Add(key, toAddTo);
                }

                toAddTo = dictionary[key];
                toAddTo.AddLast(value);
            }

            public void Remove(K key, V value)
            {
                LinkedList<V> toRemoveFrom = dictionary[key];
                toRemoveFrom.Remove(value);
            }

            public IEnumerable<K> Keys
            {
                get
                {
                    return dictionary.Keys;
                }
            }

            public IEnumerable<V> Values
            {
                get
                {
                    LinkedList<V> list = new LinkedList<V>();
                    foreach (K key in Keys)
                    {
                        foreach (V value in dictionary[key])
                        {
                            list.AddLast(value);
                        }
                    }

                    return list.AsEnumerable<V>();
                }
            }

            public LinkedList<V> this[K key]
            {
                get
                {
                    LinkedList<V> list;

                    bool containsKey = dictionary.ContainsKey(key);
                    if (containsKey == false)
                    {
                        return null;
                    }

                    list = dictionary[key];
                    return list;
                }
            }
        }

        public struct Ranges
        {
            public const int inchToIntConversionRatio = 4;

            public const decimal infantryBaseRadius = 0.5m;

            public const decimal coherency = 2;
            public const decimal minimumDistance = 1;

            public const decimal footMovement = 6;

            public const decimal lasPistol = 12;
            public const decimal boltgun = 24;
            public const decimal autocannon = 48;
        }

        public static int getAbsDistance(decimal distance)
        {
            int absDistance = Convert.ToInt32(Ranges.inchToIntConversionRatio * distance);
            return absDistance;
        }

        public static int rollDice(int numberOfDice)
        {
            Random dice = new Random();
            int maxRoll = numberOfDice * 6 + 1;

            int roll = dice.Next(numberOfDice, maxRoll);
            return roll;
        }

        public static int rollDiceKeepHighest(int numberOfRolls)
        {
            int highestRoll = 0;
            for (int i = 0; i < numberOfRolls; i++)
            {
                int roll = rollDice(1);
                if (roll < highestRoll)
                {
                    continue;
                }

                highestRoll = roll;
            }

            return highestRoll;
        }

        public static decimal getDistance(Point origin, Point destination)
        {
            int distanceX = destination.X - origin.X;
            int distanceY = destination.Y - origin.Y;

            decimal distance = (decimal)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            return distance;
        }

        public static bool targetIsInRange(Model attacker, Model target)
        {
            Point origin = attacker.currentLocation;
            Point destination = target.currentLocation;

            decimal range = attacker.ranged.range;

            decimal distance = getDistance(origin, destination);
            if (distance > range)
            {
                return false;
            }

            return true;
        }

        public static int getToWoundScore(int strength, int toughness)
        {
            int[,] woundChart = new int[,] {
                {4, 5, 6, 6, 0, 0, 0, 0, 0, 0}, // 1
                {3, 4, 5, 6, 6, 0, 0, 0, 0, 0}, // 2
                {2, 3, 4, 5, 6, 6, 0, 0, 0, 0}, // 3
                {2, 2, 3, 4, 5, 6, 6, 0, 0, 0}, // 4
                {2, 2, 2, 3, 4, 5, 6, 6, 0, 0}, // 5
                {2, 2, 2, 2, 3, 4, 5, 6, 6, 0}, // 6
                {2, 2, 2, 2, 2, 3, 4, 5, 6, 6}, // 7
                {2, 2, 2, 2, 2, 2, 3, 4, 5, 6}, // 8
                {2, 2, 2, 2, 2, 2, 2, 3, 4, 5}, // 9
                {2, 2, 2, 2, 2, 2, 2, 2, 3, 4}  // 10
            };

            int toWoundScore = woundChart[strength - 1, toughness - 1];
            return toWoundScore;
        }

        public static int getAssaultToHitScore(int attackersWeaponSkill, int opponentsWeaponSkill)
        {
            int[,] assaultHitChart = new int[,] {
                {4, 4, 5, 5, 5, 5, 5, 5, 5, 5}, // 1
                {3, 4, 4, 4, 4, 5, 5, 5, 5, 5}, // 2
                {3, 3, 4, 4, 4, 4, 5, 5, 5, 5}, // 3
                {3, 3, 3, 4, 4, 4, 4, 4, 5, 5}, // 4
                {3, 3, 3, 3, 4, 4, 4, 4, 4, 4}, // 5
                {3, 3, 3, 3, 3, 4, 4, 4, 4, 4}, // 6
                {3, 3, 3, 3, 3, 3, 4, 4, 4, 4}, // 7
                {3, 3, 3, 3, 3, 3, 3, 4, 4, 4}, // 8
                {3, 3, 3, 3, 3, 3, 3, 3, 4, 4}, // 9
                {3, 3, 3, 3, 3, 3, 3, 3, 3, 4}  // 10
            };

            int toHitScore = assaultHitChart[attackersWeaponSkill - 1, opponentsWeaponSkill - 1];
            return toHitScore;
        }

        class VehicleDamageModifier
        {
            public const string glancingHit = "Glancing hit";
            public const string hitByAP = "Hit by AP- weapon";
            public const string hitByAP1 = "Hit by AP1 weapon";
            public const string openTopped = "Target is open-topped";

            public static int getModifierValue(string modifier)
            {
                int modifierValue = 0;

                switch (modifier)
                {
                    case glancingHit:
                        modifierValue = -2;
                        break;

                    case hitByAP:
                        modifierValue = -1;
                        break;

                    case hitByAP1:
                    case openTopped:
                        modifierValue = 1;
                        break;

                    default:
                        modifierValue = 0;
                        break;
                }

                return modifierValue;
            }
        }

        public static string getVehicleDamage(Vehicle target, int rollResult)
        {
            return getVehicleDamage(target, rollResult, string.Empty);
        }

        public static string getVehicleDamage(Vehicle target, int roll, string modifier)
        {
            string result = string.Empty;
            int modifierValue = VehicleDamageModifier.getModifierValue(modifier);
            roll += modifierValue;

            if (roll < 1)
            {
                roll = 1;
            }

            if (roll > 6)
            {
                roll = 6;
            }

            switch (roll)
            {
                case 1:
                    result = "Crew - Shaken";
                    break;

                case 2:
                    result = "Crew - Stunned";
                    break;

                case 3:
                    target.isDamaged = true;
                    result = "Damaged - Weapon destroyed";
                    break;

                case 4:
                    target.isDamaged = true;
                    result = "Damaged - Immobilised";
                    break;

                case 5:
                    target.isDestroyed = true;
                    result = "Destroyed - Wrecked";
                    break;

                case 6:
                    target.isDestroyed = true;
                    result = "Destroyed - Explodes!";
                    break;
            }

            return result;
        }

        public static string getDeepStrikeMishapEffect(Unit deepStrikingUnit, int roll, Point deploymentLocation)
        {
            string effect = string.Empty;

            switch (roll)
            {
                case 1:
                case 2:
                    deepStrikingUnit.isDestroyed = true;
                    effect = "Terrible accident! The entire unit is destroyed!";
                    break;

                case 3:
                case 4:
                    deepStrikingUnit.deploy(deploymentLocation);
                    effect = "Misplaced.";
                    break;

                case 5:
                case 6:
                    deepStrikingUnit.isDelayed = true;
                    effect = "Delayed.";
                    break;
            }

            return effect;
        }

        public struct VehicleSpeed
        {
            public const string fastType = "Fast";
            public const string walkerType = "Walker";

            public const string stationary = "Stationary";
            public const string combatSpeed = "Combat speed";
            public const string cruisingSpeed = "Cruising speed";
            public const string flatOut = "Flat out";
        }

        public static List<Weapon> getAvailableVehicleWeapons(Vehicle attacker, Weapon preferedWeapon)
        {
            List<Weapon> availableWeapons = new List<Weapon>();
            string speedType = attacker.speedType;

            switch (speedType)
            {
                case VehicleSpeed.fastType:
                    addAvailableWeaponsToFastVehicle(attacker, preferedWeapon, availableWeapons);
                    break;

                case VehicleSpeed.walkerType:
                    addAvailableWeaponsToWalkerVehicle(attacker, preferedWeapon, availableWeapons);
                    break;

                default:
                    addAvailableWeaponsToVehicle(attacker, preferedWeapon, availableWeapons);
                    break;
            }

            return availableWeapons;
        }

        private static void addAvailableWeaponsToFastVehicle(Vehicle attacker, Weapon preferedWeapon, List<Weapon> availableWeapons)
        {
            string currentSpeed = attacker.currentSpeed;

            switch (currentSpeed)
            {
                case VehicleSpeed.stationary:
                case VehicleSpeed.combatSpeed:
                    availableWeapons.AddRange(attacker.offensiveWeapons);
                    availableWeapons.AddRange(attacker.defensiveWeapons);
                    break;

                case VehicleSpeed.cruisingSpeed:
                    availableWeapons.Add(preferedWeapon);
                    availableWeapons.AddRange(attacker.defensiveWeapons);
                    break;

                default:
                    break;
            }
        }

        private static void addAvailableWeaponsToWalkerVehicle(Vehicle attacker, Weapon preferedWeapon, List<Weapon> availableWeapons)
        {
            string currentSpeed = attacker.currentSpeed;

            switch (currentSpeed)
            {
                case VehicleSpeed.stationary:
                case VehicleSpeed.combatSpeed:
                    availableWeapons.AddRange(attacker.offensiveWeapons);
                    availableWeapons.AddRange(attacker.defensiveWeapons);
                    break;

                default:
                    break;
            }
        }

        private static void addAvailableWeaponsToVehicle(Vehicle attacker, Weapon preferedWeapon, List<Weapon> availableWeapons)
        {
            string currentSpeed = attacker.currentSpeed;

            switch (currentSpeed)
            {
                case VehicleSpeed.stationary:
                    availableWeapons.AddRange(attacker.offensiveWeapons);
                    availableWeapons.AddRange(attacker.defensiveWeapons);
                    break;

                case VehicleSpeed.combatSpeed:
                    availableWeapons.Add(preferedWeapon);
                    availableWeapons.AddRange(attacker.defensiveWeapons);
                    break;

                default:
                    break;
            }
        }

        public static int getReserveUnitArrivesScore(int turn)
        {
            int unitArrivesScore = 0;

            switch (turn)
            {
                case 1:
                    break;

                case 2:
                    unitArrivesScore = 4;
                    break;

                case 3:
                    unitArrivesScore = 3;
                    break;

                case 4:
                    unitArrivesScore = 2;
                    break;

                default:
                    unitArrivesScore = 1;
                    break;
            }

            return unitArrivesScore;
        }

        internal static int getDirection(Point origin, Point destination)
        {
            int distanceX = origin.X - destination.X;
            int distanceY = origin.Y - destination.Y;

            int direction = 360;

            if (distanceY == 0)
            {
                if (distanceX < 0)
                {
                    direction = 270;
                    return direction;
                }

                direction = 90;
                return direction;
            }

            if (distanceY < 0)
            {
                direction = 180;
            }

            double value = distanceX;
            double value2 = distanceY;

            direction += (int)Global.radiansToDegrees(Math.Atan(value / value2));
            direction %= 360;

            return direction;
        }

        public static int radiansToDegrees(double radians)
        {
            int degrees = (int)(radians * Math.PI / 180);
            return degrees;
        }

        public static int degreesToRadians(double degrees)
        {
            int radians = (int)(degrees * 180 / Math.PI);
            return radians;
        }
    }
}
