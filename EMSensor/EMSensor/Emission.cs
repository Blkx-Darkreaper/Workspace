using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EMSensor
{
    class Emission
    {
        public Point origin { get; set; }
        public decimal distancePropegated { get; private set; }
        private Dictionary<decimal, decimal> allSignals { get; set; }
        public bool noSignal { get; private set; }

        public Emission(Point inOrigin, Dictionary<decimal, decimal> inSignature)
        {
            origin = inOrigin;
            distancePropegated = 0;
            allSignals = inSignature;
            noSignal = false;
        }

        public void Propegate(decimal velocity, decimal timeElapsed)
        {
            if (noSignal == true)
            {
                return;
            }

            distancePropegated += velocity * timeElapsed;

            CheckSignalAttenuation();
        }

        private void CheckSignalAttenuation()
        {
            foreach (decimal signal in allSignals.Values)
            {
                decimal attenuatedSignal = GetAttenuatedSignal(distancePropegated, signal);
                if (attenuatedSignal > 1)
                {
                    return;
                }
            }

            noSignal = true;
        }

        private decimal GetAttenuatedSignal(decimal distance, decimal amplitude)
        {
            //decimal coefficient = 5;
            //decimal attenuatedSignal = coefficient * amplitude / distance;
            decimal attenuatedSignal = amplitude * 400 / (decimal)Math.Pow((double)distance + 20, 2);
            attenuatedSignal = Math.Round(attenuatedSignal, 2);
            return attenuatedSignal;
        }

        public decimal GetAttenuatedSignal(decimal frequency, Point receiver)
        {
            decimal distance = (decimal)Global.GetDistanceToPoint(origin, receiver);
            if (distance > distancePropegated)
            {
                return 0;
            }

            decimal amplitude;

            try
            {
                amplitude = allSignals[frequency];
            }
            catch (Exception)
            {
                amplitude = 0;
            }

            decimal output = GetAttenuatedSignal(distance, amplitude);
            return output;
        }

        public bool CheckSignalInRange(Point receiver, decimal sensitivity)
        {
            decimal range = (decimal)Global.GetDistanceToPoint(origin, receiver);
            decimal difference = Math.Abs(range - distancePropegated);

            if (difference > sensitivity)
            {
                return false;
            }

            return true;
        }
    }
}
