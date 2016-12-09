using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortyK
{
    class Terrain
    {
        protected int elevation { get; private set; }
        protected const int lowLyingTerrain = 1;
        protected const int highTerrain = 2;
        protected const int veryHighTerrain = 3;

        protected string providedCover { get; private set; }
        protected bool isDangerous { get; private set; }
        protected bool isImpassable { get; private set; }

        public Terrain(int inElevation = 0, string inCover = Cover.clear, bool inIsDangerous = false, bool inIsImpassable = false)
        {
            elevation = inElevation;
            providedCover = inCover;
            isDangerous = inIsDangerous;
            isImpassable = inIsImpassable;
        }
    }
}
