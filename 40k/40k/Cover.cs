using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Cover
    {
        public const string clear = "No cover";
        public const string razorWire = "Razor wire";
        public const string wireMesh = "Wire mesh";
        public const string highGrass = "High grass";
        public const string crops = "Crops";
        public const string bushes = "Bushes";
        public const string hedge = "Hedge";
        public const string fence = "Fence";
        public const string unit = "Unit";
        public const string trenche = "Trenche";
        public const string gunPit = "Gun pit";
        public const string tankTraps = "Tank traps";
        public const string emplacement = "Emplacement";
        public const string sandbags = "Sandbags";
        public const string barricade = "Barricade";
        public const string logs = "Logs";
        public const string pipes = "Pipes";
        public const string crates = "Crates";
        public const string barrels = "Barrels";
        public const string hillCrest = "Hill crest";
        public const string woods = "Woods";
        public const string jungle = "Jungle";
        public const string wreckage = "Wreckage";
        public const string crater = "Crater";
        public const string rubble = "Rubble";
        public const string rocks = "Rocks";
        public const string ruins = "Ruins";
        public const string wall = "Wall";
        public const string building = "Building";
        public const string wreckedVehicle = "Wrecked vehicle";
        public const string fortification = "Fortification";

        public static int getCoverSave(string coverType)
        {
            int coverSave;
            switch (coverType)
            {
                case razorWire:
                case wireMesh:
                    coverSave = 6;
                    break;

                case highGrass:
                case crops:
                case bushes:
                case hedge:
                case fence:
                    coverSave = 5;
                    break;

                case unit:
                case trenche:
                case gunPit:
                case tankTraps:
                case emplacement:
                case sandbags:
                case barricade:
                case logs:
                case pipes:
                case crates:
                case barrels:
                case hillCrest:
                case woods:
                case jungle:
                case wreckage:
                case crater:
                case rubble:
                case rocks:
                case ruins:
                case wall:
                case building:
                case wreckedVehicle:
                    coverSave = 4;
                    break;

                case fortification:
                    coverSave = 3;
                    break;

                default:
                    coverSave = 0;
                    break;
            }

            return coverSave;
        }
    }
}
