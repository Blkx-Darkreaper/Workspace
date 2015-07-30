using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EMSensor
{
    class Sensor
    {
        public Environment signalData { get; set; }
        public List<decimal> allFrequencies { get; set; }
        public decimal sensitivity { get; set; }
        public decimal precision { get; set; }

        public Sensor(Environment inData, List<decimal> inFrequencies, decimal inSensitivity, decimal inPrecision)
        {
            signalData = inData;
            allFrequencies = inFrequencies;
            sensitivity = inSensitivity;
            precision = inPrecision;
        }

        public void Update(decimal timeElapsed)
        {
            signalData.Update(timeElapsed);
        }

        public Dictionary<decimal, List<Point>> GetAllReadings(int screenWidth, int screenHeight, int bearingPoints, Point receiver)
        {
            List<decimal> allBearings = new List<decimal>();
            decimal bearingIncrement = 360 / bearingPoints;
            for (decimal i = -180; i < 180; i += bearingIncrement)
            {
                allBearings.Add(i);
            }

            Dictionary<decimal, List<Point>> allReadings = new Dictionary<decimal, List<Point>>();

            List<Emission> allEmissions = signalData.GetEmissionsWithinRange(receiver, sensitivity);

            foreach (decimal frequency in allFrequencies)
            {
                List<Point> allPoints = new List<Point>();

                foreach (decimal bearing in allBearings)
                {
                    decimal readingAtBearing = 0;

                    foreach (Emission emission in allEmissions)
                    {
                        decimal signalToAdd = GetSignalAtBearing(bearing, receiver, emission, frequency);
                        if (signalToAdd <= 0)
                        {
                            continue;
                        }

                        readingAtBearing += signalToAdd;
                    }
                    readingAtBearing = Math.Round(readingAtBearing, 2);

                    // Add variation to reading
                    decimal variance = Math.Round(precision * (decimal)new Random().NextDouble() - 0.5m * precision, 2);
                    readingAtBearing += variance;

                    int drawX = (int)((bearing + 180) / 360 * screenWidth);
                    int drawY;
                    Point nextPoint;
                    int value = (int)(500 * readingAtBearing / (readingAtBearing + 120));

                    drawY = screenHeight - value - 5;
                    if (drawY < 0)
                    {
                        drawY = 0;
                    }

                    nextPoint = new Point(drawX, drawY);
                    allPoints.Add(nextPoint);
                }

                allReadings.Add(frequency, allPoints);
            }

            return allReadings;
        }

        //public Dictionary<decimal, Dictionary<decimal, decimal>> GetAllSignals(Point receiver, List<decimal> allBearings, List<decimal> allFrequencies)
        //{
        //    Dictionary<decimal, Dictionary<decimal, decimal>> allReadings = new Dictionary<decimal, Dictionary<decimal, decimal>>();
        //    List<Emission> allEmissions = signalData.GetEmissionsWithinRange(receiver, sensitivity);

        //    foreach (Emission incomingSignals in allEmissions)
        //    {
        //        foreach (decimal frequency in allFrequencies)
        //        {
        //            foreach (decimal bearing in allBearings)
        //            {
        //                decimal signalReading = GetSignalAtBearing(bearing, receiver, incomingSignals, frequency);
        //                decimal variance = Math.Round(precision * (decimal)new Random().NextDouble() - 0.5m * precision, 2);

        //                signalReading += variance;

        //                var bearingReading = allReadings[frequency][bearing];
        //                if (bearingReading == null)
        //                {
        //                    bearingReading = signalReading;
        //                    continue;
        //                }

        //                bearingReading += signalReading;
        //            }
        //        }
        //    }

        //    return allReadings;
        //}

        //private Dictionary<decimal, decimal> GetSignalAlongBearings(Point receiver, Emission incomingSignals, List<decimal> allBearings, decimal frequency)
        //{
        //    Dictionary<decimal, decimal> signalReadings = new Dictionary<decimal, decimal>();

        //    foreach (decimal bearing in allBearings)
        //    {
        //        decimal signalAtBearing = GetSignalAtBearing(bearing, receiver, incomingSignals, frequency);
        //        signalReadings.Add(bearing, signalAtBearing);
        //    }

        //    return signalReadings;
        //}

        private decimal GetSignalAtBearing(decimal bearing, Point receiver, Emission incomingSignals, decimal frequency)
        {
            decimal signal = incomingSignals.GetAttenuatedSignal(frequency, receiver);
            if (signal <= 0)
            {
                return 0;
            }

            Point origin = incomingSignals.origin;
            decimal distance = (decimal)Global.GetDistanceToPoint(origin, receiver);
            decimal absBearing = (decimal)Global.GetBearingToPoint(origin, receiver);
            decimal convertedBearing = absBearing;
            if (convertedBearing > 180)
            {
                convertedBearing -= 360;
            }

            decimal relativeBearing = bearing - convertedBearing;
            if (relativeBearing < -180)
            {
                relativeBearing += 360;
            }

            decimal diffusedSignal = GetDiffusedSignal(relativeBearing, signal, distance);
            diffusedSignal = Math.Round(diffusedSignal, 2);

            return diffusedSignal;
        }

        private decimal GetDiffusedSignal(decimal bearing, decimal amplitude, decimal distance)
        {
            double coefficient = 30;
            double arc = (180 + coefficient) - coefficient * Math.Sqrt((double)distance);

            if (bearing > 90)
            {
                return 0;
            }

            if (bearing < -90)
            {
                return 0;
            }

            double exponent = (double)-(bearing * bearing) / (0.022 * Math.Pow(arc, 2));
            decimal diffusedSignal = amplitude * (decimal)Math.Pow(1.5, exponent);
            diffusedSignal = Math.Round(diffusedSignal, 2);
            return diffusedSignal;
        }
    }
}
