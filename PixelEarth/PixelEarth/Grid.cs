using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PixelEarth
{
    public class Grid : Entity
    {
        public double longitude { get; protected set; }
        public double latitude { get; protected set; }
        public const int MAX_INSOLATION = 1413;

        public Grid(int x, int y, int size, Color mainColour)
            : base(new Point(x, y), size, mainColour)
        {
            //double constant = Entity.metersPerPixel;
            //this.longitude = x / constant - 180;
            this.longitude = x - 180;
            this.latitude = 90 - y;
        }

        public Grid(Point location, int size, Color mainColour)
            : base(location, size, mainColour)
        {
            //double constant = Entity.metersPerPixel;
            //this.longitude = location.X / constant - 180;
            this.longitude = location.X - 180;
            this.latitude = 90 - location.Y;
        }

        public override void Update(float timeElapsed)
        {
            base.Update(timeElapsed);

            DateTime dateTime = Program.GetDateTime();

            //double lightLevel = GetLightLevel(dateTime, latitude, longitude);
            //SetInsolation(lightLevel);

            double insolation = GetInsolation(dateTime, latitude, longitude);
            SetInsolation(insolation);
        }

        public double GetInsolationNormal(DateTime dateTime)
        {
            int dayOfYear = dateTime.DayOfYear;
            int totalDays = 365;

            int year = dateTime.Year;

            bool leapYear = false;
            if (year % 4 != 0)
            {
                leapYear = false;
            }
            else if (year % 100 != 0)
            {
                leapYear = true;
            }
            else if (year % 400 != 0)
            {
                leapYear = false;
            }
            else
            {
                leapYear = true;
            }

            if (leapYear == true)
            {
                totalDays++;
            }

            double insolationNormal = (1413 - 1321) / 2 * Math.Cos(2 * Math.PI * (dayOfYear + 10.243) / totalDays) + (1413 + 1321) / 2;
            return insolationNormal;
        }

        public double GetLightLevel(DateTime dateTime, double latitude, double longitude)
        {
            int timezone = GetTimezoneDiff(longitude);

            double hours = dateTime.Hour;
            double mins = dateTime.Minute;
            double secs = dateTime.Second;
            double time = hours + mins / 60 + secs / 3600;

            double localTime = time + timezone;
            if (localTime < 0)
            {
                localTime += 24;
            }
            localTime %= 24;

            int dayOfYear = dateTime.DayOfYear;
            double seasonalOffset = -23.43 * Math.Sin(2 * Math.PI * (dayOfYear - 80) / 365.2425);

            double insolationNormal = GetInsolationNormal(dateTime);

            double halfLight = insolationNormal / 2;

            double lightLevel = (halfLight * Math.Sin(Math.PI * (localTime - 6) / 12) + halfLight) * Math.Cos(Math.PI * (latitude + seasonalOffset) / 180);
            if (lightLevel < 0)
            {
                lightLevel = 0;
            }

            return lightLevel;
        }

        public double GetInsolation(DateTime dateTime, double latitude, double longitude)
        {
            double julianDate = GetAdjustedJulianDate(dateTime, longitude);
            double julianCentury = GetJulianCentury(julianDate);

            double meanSolarLongitude = GetMeanSolarLongitude(julianCentury);
            double meanSolarAnomaly = GetMeanSolarAnomaly(julianCentury);

            double solarAppLongitude = GetSolarAppLongitude(meanSolarLongitude, meanSolarAnomaly, julianCentury);
            double meanEclipticObliquity = GetMeanEclipticObliquity(julianCentury);
            double solarDeclination = GetSolarDeclination(solarAppLongitude, meanEclipticObliquity, julianCentury);

            double orbitalEccentricity = GetOrbitalEccentricity(julianCentury);
            double solarTimeMins = GetSolarTimeMins(dateTime, longitude, meanSolarLongitude, meanSolarAnomaly, orbitalEccentricity, meanEclipticObliquity, julianCentury);
            double solarHourAngle = GetSolarHourAngle(solarTimeMins);

            double solarZenithAngle = GetSolarZenithAngle(latitude, solarDeclination, solarHourAngle);

            double corrSolarElevationAngle = GetCorrectedSolarElevationAngle(solarZenithAngle);

            double percentInsolation = Program.Sin(corrSolarElevationAngle);
            if (percentInsolation < 0)
            {
                percentInsolation = 0;
            }

            double insolationNormal = GetInsolationNormal(dateTime);

            double insolation = insolationNormal * percentInsolation;
            return insolation;
        }

        public void SetInsolation(double lightLevel)
        {
            int brightness = (int)Math.Round(lightLevel / MAX_INSOLATION * 255, 0);
            mainColour = Color.FromArgb(255, brightness, brightness, brightness);
        }

        public double GetAdjustedJulianDate(DateTime dateTime, double longitude)
        {
            double julianDate = Program.GetJulianDate(dateTime);

            double timezoneDiff = GetTimezoneDiff(longitude);

            julianDate += timezoneDiff / 24;
            return julianDate;
        }

        public int GetTimezoneDiff(double longitude)
        {
            int timezoneDiff = (int)Math.Floor(longitude * 24 / 360);
            return timezoneDiff;
        }

        public double GetJulianCentury(double julianDate)
        {
            double julianCentury = (julianDate - 2451545) / 36525;
            return julianCentury;
        }

        public double GetMeanSolarLongitude(double julianCentury)
        {
            double meanSolarLongitude = (280.46646 + julianCentury * (36000.76983 + julianCentury * 0.0003032)) % 360;
            return meanSolarLongitude;
        }

        public double GetMeanSolarAnomaly(double julianCentury)
        {
            double meanSolarAnomaly = 357.52911 + julianCentury * (35999.05029 - 0.0001537 * julianCentury);
            return meanSolarAnomaly;
        }

        public double GetOrbitalEccentricity(double julianCentury)
        {
            double orbitalEccentricity = 0.016708634 - julianCentury * (0.000042037 + 0.0000001267 * julianCentury);
            return orbitalEccentricity;
        }

        public double GetMeanEclipticObliquity(double julianCentury)
        {
            double eclipticObliquity = 23 + (26 + ((21.448 - julianCentury * (46.815 + julianCentury * (0.00059 - julianCentury * 0.001813)))) / 60) / 60;
            return eclipticObliquity;
        }

        public double GetSolarAppLongitude(double meanSolarLongitude, double meanSolarAnomaly, double julianCentury)
        {
            double solarEquationOfCtr = Program.Sin(meanSolarAnomaly) * (1.914602 - julianCentury * (0.004817 + 0.000014 * julianCentury)) + Program.Sin(2 * meanSolarAnomaly) * (0.019993 - 0.000101 * julianCentury) + Program.Sin(3 * meanSolarAnomaly) * 0.000289;

            double trueSolarLongitude = meanSolarLongitude + solarEquationOfCtr;

            double solarAppLongitude = trueSolarLongitude - 0.00569 - 0.00478 * Program.Sin(125.04 - 1934.136 * julianCentury);
            return solarAppLongitude;
        }

        public double GetCorrectedEclipticObliquity(double eclipticObliquity, double julianCentury)
        {
            double correctedObliquity = eclipticObliquity + 0.00256 * Program.Cos(125.04 - 1934.136 * julianCentury);
            return correctedObliquity;
        }

        public double GetSolarDeclination(double solarAppLongitude, double eclipticObliquity, double julianCentury)
        {
            double correctedObliquity = GetCorrectedEclipticObliquity(eclipticObliquity, julianCentury);

            double solarDeclination = Program.Asin(Program.Sin(correctedObliquity) * Program.Sin(solarAppLongitude));
            return solarDeclination;
        }

        public double GetSolarTimeMins(DateTime dateTime, double longitude, double meanSolarLongitude, double meanSolarAnomaly, double orbitalEccentricity, double meanEclipticObliquity, double julianCentury)
        {
            double correctedObliquity = GetCorrectedEclipticObliquity(meanEclipticObliquity, julianCentury);

            double varY = Math.Pow(Program.Tan(correctedObliquity / 2), 2);

            double equationOfTime = 4 * Program.RadiansToDegrees(varY * Program.Sin(2 * meanSolarLongitude) - 2 * orbitalEccentricity * Program.Sin(meanSolarAnomaly) + 4 * orbitalEccentricity * varY * Program.Sin(meanSolarAnomaly) * Program.Cos(2 * meanSolarLongitude) - 0.5 * Math.Pow(varY, 2) * Program.Sin(4 * meanSolarLongitude) - 1.25 * Math.Pow(orbitalEccentricity, 2) * Program.Sin(2 * meanSolarAnomaly));

            int timezoneDiff = (int)Math.Floor(longitude * 24 / 360);

            double hours = (dateTime.Hour + timezoneDiff) % 24;
            if (hours < 0)
            {
                hours += 24;
            }

            double mins = dateTime.Minute;
            double secs = dateTime.Second;
            double time = (hours + mins / 60 + secs / 3600) / 24;

            double solarTimeMins = (time * 1440 + equationOfTime + 4 * longitude - 60 * timezoneDiff) % 1440;
            return solarTimeMins;
        }

        public double GetSolarHourAngle(double solarTimeMins)
        {
            double solarHourAngle;

            if (solarTimeMins / 4 < 0)
            {
                solarHourAngle = solarTimeMins / 4 + 180;
            }
            else
            {
                solarHourAngle = solarTimeMins / 4 - 180;
            }

            return solarHourAngle;
        }

        public double GetSolarZenithAngle(double latitude, double solarDeclination, double solarHourAngle)
        {
            double solarZenithAngle = Program.Acos(Program.Sin(latitude) * Program.Sin(solarDeclination) + Program.Cos(latitude) * Program.Cos(solarDeclination) * Program.Cos(solarHourAngle));
            return solarZenithAngle;
        }

        public double GetCorrectedSolarElevationAngle(double solarZenithAngle)
        {
            double solarElevationAngle = 90 - solarZenithAngle;
            double atmRefractCorrection;
            if (solarElevationAngle > 85)
            {
                atmRefractCorrection = 0;
            }
            else if (solarElevationAngle > 5)
            {
                atmRefractCorrection = 58.1 / Program.Tan(solarElevationAngle) - 0.07 / Math.Pow(Program.Tan(solarElevationAngle), 3) + 0.000086 / Math.Pow(Program.Tan(solarElevationAngle), 5);
            }
            else if (solarElevationAngle > -0.575)
            {
                atmRefractCorrection = 1735 + solarElevationAngle * (-518.2 + solarElevationAngle * (103.4 + solarElevationAngle * (-12.79 + solarElevationAngle * 0.711)));
            }
            else
            {
                atmRefractCorrection = -20.772 / Program.Tan(solarElevationAngle);
            }

            atmRefractCorrection /= 3600;

            double correctedSolarElevationAngle = solarElevationAngle + atmRefractCorrection;
            return correctedSolarElevationAngle;
        }
    }
}