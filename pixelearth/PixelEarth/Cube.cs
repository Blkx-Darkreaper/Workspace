using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEarth
{
    public class Cube
    {
        public enum Phase { Solid, Liquid, Gas}
        public Phase phase { get; protected set; }
        protected double width { get; set; }    // meters
        protected double density { get; set; }  // Kg/cubic meter
        public double temperature { get; protected set; }   // degrees Kelvin
        protected double opacity { get; set; }
        protected double albedo { get; set; }
        protected double heatCapacity { get; set; } // Jules/Kg * degree Kelvin

        public bool isSolid { get { return phase.Equals(Phase.Solid); } }
        public bool isLiquid { get { return phase.Equals(Phase.Liquid); } }
        public bool isGas { get { return phase.Equals(Phase.Gas); } }

        public Cube(double width, double density, double temperature, double opacity, double albedo, double heatCapacity)
        {
            this.width = width;
            this.density = density;
            this.temperature = temperature;
            this.opacity = opacity;
            this.albedo = albedo;
            this.heatCapacity = heatCapacity;
        }

        public virtual void Insolate(float hoursElapsed, double insolation, out double leftoverEnergy, out double reflectedEnergy) {
            double area = width * width;
            double solarEnergy = insolation * area * hoursElapsed;

            double incidentEnergy = solarEnergy * opacity;
            leftoverEnergy = solarEnergy - incidentEnergy;

            reflectedEnergy = incidentEnergy * albedo;
            double absorbedEnergy = incidentEnergy - reflectedEnergy;

            double volume = width * width * width;
            double mass = density * volume;

            double deltaTemp = absorbedEnergy / mass / heatCapacity;
            temperature += deltaTemp;
            temperature -= hoursElapsed;    //debug to simulate cooling
        }
    }
}
