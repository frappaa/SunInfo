using System;

namespace SunInfo.AstroAlgorithms
{
    public class SunInfoCalculator
    {
        private readonly double _julianDate;

        public SunInfoCalculator(DateTime utcDateTime)
        {
            _julianDate = CalculateJulianDate(utcDateTime);
        }

        public double SunEarthDistance
        {
            get { return 1.00014 - .01671*Math.Cos(MeanSunAnomaly.ToRadian().Value) - .00014*Math.Cos(2*MeanSunAnomaly.ToRadian().Value); }
        }

        public double SunEarthDistanceKm
        {
            get { return SunEarthDistance*149597870.700; }
        }

        public Radian AngularDiameter
        {
            get { return new Radian(2*Math.Atan((0.5*1392000)/SunEarthDistanceKm)); }
        }

        public double DaysFrom2000
        {
            get { return _julianDate - 2451545.0; }
        }

        public double Jears10000From2000
        {
            get { return (DaysFrom2000/365.25)/10000; }
        }

        private Degree MeanSunLongitude
        {
            get { return new Degree(280.460) + new Degree(.9856474) * DaysFrom2000; }
        }

        private Degree EclipticSunLongitude
        {
            get { return MeanSunLongitude + new Degree(1.915)*Math.Sin(MeanSunAnomaly.ToRadian().Value) + new Degree(.020)*Math.Sin(2*MeanSunAnomaly.ToRadian().Value); }
        }

        private Degree MeanSunAnomaly
        {
            get { return new Degree(357.528) + new Degree(.9856003)*DaysFrom2000; }
        }

        private static double CalculateJulianDate(DateTime utcDateTime)
        {
            int month = utcDateTime.Month;
            int day = utcDateTime.Day;
            int year = utcDateTime.Year;
            if (month < 3)
            {
                month += 12;
                year -= 1;
            }
            double julianDay = day + (153 * month - 457) / 5 + 365 * year + (year / 4) - (year / 100) + (year / 400) + 1721119;
            var tsFromMidday = utcDateTime.TimeOfDay - new TimeSpan(12, 0, 0);
            double fract = Math.Abs(tsFromMidday.Hours) / 24.0 + (tsFromMidday.Minutes / (24.0 * 60.0)) + (tsFromMidday.Seconds / (24.0 * 60.0 * 60.0));
            if (tsFromMidday.Hours < 0)
            {
                return julianDay - fract;
            }
            return julianDay + fract;
        }

        public double JulianDate
        {
            get { return _julianDate; }
        }

        public Degree AxialTilt
        {
            //get { return new Degree(23.439) - new Degree(.0000004)*DaysFrom2000; }
            get { return new Degree(23, 26, 21.448)
                - new Degree(0,0,4680.93)*Jears10000From2000
                - new Degree(0,0,1.55)*Math.Pow(Jears10000From2000,2)
                + new Degree(0,0,1999.25)*Math.Pow(Jears10000From2000,3)
                - new Degree(0,0,51.38)*Math.Pow(Jears10000From2000,4)
                - new Degree(0,0,249.67)*Math.Pow(Jears10000From2000,5)
                - new Degree(0,0,39.05)*Math.Pow(Jears10000From2000,6)
                + new Degree(0,0,7.12)*Math.Pow(Jears10000From2000,7)
                + new Degree(0,0,27.87)*Math.Pow(Jears10000From2000,8)
                + new Degree(0,0,5.79)*Math.Pow(Jears10000From2000,9)
                + new Degree(0,0,2.45)*Math.Pow(Jears10000From2000,10);
            }
        }

        public Radian RightAscension
        {
            get { return new Radian(Math.Atan(Math.Cos(AxialTilt.ToRadian().Value)*Math.Tan(EclipticSunLongitude.ToRadian().Value))); }
        }

        public Radian Declination
        {
            get { return new Radian(Math.Asin(Math.Sin(AxialTilt.ToRadian().Value) * Math.Sin(EclipticSunLongitude.ToRadian().Value))); }
        }

        public HorizontalCoordinates HorizontalCoordinates(Radian latitude)
        {
            return new EquatorialCoordinates(RightAscension, Declination).ToHorizontalCoordinates(latitude);
        }
    }
}
