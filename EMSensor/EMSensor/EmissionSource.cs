using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EMSensor
{
    class EmissionSource
    {
        public Dictionary<decimal, decimal> emissionSignature { get; set; }
        public Point location { get; set; }

        public EmissionSource(Point inLocation, List<decimal> signatureFrequencies, int min, int max)
        {
            emissionSignature = new Dictionary<decimal, decimal>();
            location = inLocation;

            foreach (decimal frequency in signatureFrequencies)
            {
                Random random = new Random();
                decimal amplitude = Math.Round((decimal)random.Next(min, max), 2);

                emissionSignature.Add(frequency, amplitude);
            }
        }

        private void FluctuateSignature()
        {
            Random rand = new Random();
            List<decimal> allFrequencies = new List<decimal>(emissionSignature.Keys);
            foreach (decimal frequency in allFrequencies)
            {
                decimal fluctuation = (decimal) Math.Round(20 * (rand.NextDouble() - 0.5), 2);
                emissionSignature[frequency] += fluctuation;
            }
        }

        public Emission Update()
        {
            FluctuateSignature();
            Emission signals = new Emission(location, emissionSignature);
            return signals;
        }
    }
}
