using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

namespace PixelEarth
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void insolation()
        {
            Program.GenerateWorld(360, 180);

            DateTime dateTime = Program.GetDateTime(0);

            int arcticIndex = 8280;
            int cancerIndex = 24120;
            int equatorIndex = 32400;
            int capricornIndex = 40680;
            int antarcticIndex = 56520;
            int[] latitudeIndexes = new int[] { arcticIndex, cancerIndex, equatorIndex, capricornIndex, antarcticIndex };

            int dateline = 0;
            int centralTimezone = 90;
            int greenwichTimezone = 180;
            int omskTimezone = 270;
            int[] timezones = new int[] { dateline, centralTimezone, greenwichTimezone, omskTimezone };

            //while (dateTime.Day == 22)
            while (dateTime.Hour == 0)
            {
                Debug.Print(dateTime.ToString());
                foreach (int offset in timezones)
                {
                    foreach (int index in latitudeIndexes)
                    {
                        int adjIndex = index + offset;
                        if (adjIndex > 12960)
                        {
                            adjIndex = 12960;
                        }

                        Grid grid = (Grid)Program.allEntities[adjIndex];
                        //double lightLevel = grid.GetInsolation(dateTime, grid.latitude, grid.longitude);

                        double latitude = grid.latitude;
                        double longitude = grid.longitude;

                        double insolation = grid.GetInsolation(dateTime, latitude, longitude);

                        Debug.Print(String.Format("{0}\t{1}\t{2}", latitude, longitude, insolation));
                    }
                }

                dateTime = dateTime.AddHours(1);
            }
        }

        [TestMethod]
        public void lightLevel()
        {
            Program.GenerateWorld(360, 180);

            DateTime dateTime = Program.GetDateTime(0);

            int arcticIndex = 8280;
            int cancerIndex = 24120;
            int equatorIndex = 32400;
            int capricornIndex = 40680;
            int antarcticIndex = 56520;
            int[] latitudeIndexes = new int[] { arcticIndex, cancerIndex, equatorIndex, capricornIndex, antarcticIndex };

            int dateline = 0;
            int centralTimezone = 90;
            int greenwichTimezone = 180;
            int omskTimezone = 270;
            int[] timezones = new int[] { dateline, centralTimezone, greenwichTimezone, omskTimezone };

            //while (dateTime.Day == 22)
            while (dateTime.Hour == 0)
            {
                Debug.Print(dateTime.ToString());
                Debug.Print(String.Format("Latitude{0}Longitude{0}Insolation", "\t"));
                foreach (int offset in timezones)
                {
                    foreach (int index in latitudeIndexes)
                    {
                        int adjIndex = index + offset;
                        if (adjIndex > 129960)
                        {
                            adjIndex = 129960;
                        }

                        Grid grid = (Grid)Program.allEntities[adjIndex];
                        //double lightLevel = grid.GetInsolation(dateTime, grid.latitude, grid.longitude);

                        double latitude = grid.latitude;
                        double longitude = grid.longitude;

                        double lightLevel = grid.GetLightLevel(dateTime, latitude, longitude);

                        Debug.Print(String.Format("{0}\t{1}\t{2}", latitude, longitude, lightLevel));
                    }
                }

                dateTime = dateTime.AddHours(1);
            }
        }
    
        [TestMethod]
        public void timezones()
        {
            int arcticIndex = 8280;
            int cancerIndex = 24120;
            int equatorIndex = 32400;
            int capricornIndex = 40680;
            int antarcticIndex = 56520;
            int[] latitudeIndexes = new int[] { arcticIndex, cancerIndex, equatorIndex, capricornIndex, antarcticIndex };

            int dateline = 0;
            int centralTimezone = 90;
            int greenwichTimezone = 180;
            int omskTimezone = 270;
            int[] timezones = new int[] { dateline, centralTimezone, greenwichTimezone, omskTimezone };

            Program.GenerateWorld(360, 180);

            DateTime gmt = Program.GetDateTime(0);
            Debug.Print(gmt.ToString());
            Debug.Print(String.Format("Longitude{0}Julian Date{0}Local Time", "\t"));
            foreach (int offset in timezones)
            {
                Grid grid = (Grid)Program.allEntities[cancerIndex + offset];

                double longitude = grid.longitude;
                double julianDate = grid.GetAdjustedJulianDate(gmt, longitude);
                DateTime localDate = Program.GetDateTimeFromJulianDate(julianDate);
                //string localTime = String.Format("{0}:{1}", localDate.Hour, localDate.Minute);
                string localTime = localDate.ToString();
                Debug.Print(String.Format("{0}\t{1}\t{2}", longitude, julianDate, localTime));
            }
        }

        [TestMethod]
        public void checkInsolation()
        {
            DateTime gmt = Program.GetDateTime(0);
            double insolation;
            double latitude, longitude;

            Grid victoria = new Grid(57, 42, 1, Color.Black);
            latitude = victoria.latitude;
            Assert.IsTrue(latitude == 48);

            longitude = victoria.longitude;
            Assert.IsTrue(longitude == -123);

            insolation = victoria.GetInsolation(gmt, latitude, longitude);
            Assert.IsTrue(insolation == 0);


            Grid southernVictoriaGreenwich = new Grid(180, 138, 1, Color.Black);
            latitude = southernVictoriaGreenwich.latitude;
            Assert.IsTrue(latitude == -48);

            longitude = southernVictoriaGreenwich.longitude;
            Assert.IsTrue(longitude == 0);

            insolation = Math.Round(southernVictoriaGreenwich.GetInsolation(gmt, latitude, longitude), 3);
            Assert.IsTrue(insolation == 0.319);
        }
    }
}
