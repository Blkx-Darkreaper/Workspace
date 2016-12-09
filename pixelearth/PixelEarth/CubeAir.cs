using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEarth
{
    class CubeAir : Cube
    {
        public CubeAir(double width, double temp)
            : base(width, density: 1.225, temperature: temp, opacity: 0.01, albedo: 0.01, heatCapacity: 1.005)
        {
            phase = Phase.Gas;
        }
    }
}
