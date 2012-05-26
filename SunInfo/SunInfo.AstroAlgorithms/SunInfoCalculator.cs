using System;

namespace SunInfo.AstroAlgorithms
{
    public class SunInfoCalculator
    {
        private readonly double _julianDate;

        private Degree _axialTilt;

        public SunInfoCalculator(DateTime utcDateTime)
        {
            _julianDate = TimeUtils.UtcToJulianDate(utcDateTime);
        }

        public double SunEarthDistance
        {
            get
            {
                double meanSunAnomaly = MeanSunAnomaly.ToRadian().Value;
                return 1.00014 - .01671 * Math.Cos(meanSunAnomaly) - .00014 * Math.Cos(2 * meanSunAnomaly);
            }
        }

        public double SunEarthDistanceKm
        {
            get { return SunEarthDistance * 149597870.700; }
        }

        public Radian AngularDiameter
        {
            get { return new Radian(2 * Math.Atan((0.5 * 1392000) / SunEarthDistanceKm)); }
        }

        public double DaysFrom2000
        {
            get { return _julianDate - 2451545.0009; }
        }

        public Radian GreenwichSiderealTime
        {
            get { return new Degree(TimeSpan.FromHours((18.697374558 + 24.06570982441908 * DaysFrom2000) % 24)).ToRadian(); }
        }

        public Radian HourAngle(Radian longitude)
        {
            var hourAngle = GreenwichSiderealTime - (-longitude) - RightAscension;
            if (hourAngle.Value < 0)
            {
                hourAngle = new Degree(TimeSpan.FromHours(24.0)).ToRadian() + hourAngle;
            }
            return hourAngle;
        }

        public double Jears10000From2000
        {
            get { return (DaysFrom2000 / 365.25) / 10000; }
        }

        private Degree MeanSunLongitude
        {
            get { return new Degree(280.460 + .9856474 * DaysFrom2000); }
        }

        private Degree EclipticSunLongitude
        {
            get
            {
                double meanSunAnomaly = MeanSunAnomaly.ToRadian().Value;
                return MeanSunLongitude + new Degree(1.915 * Math.Sin(meanSunAnomaly) + .020 * Math.Sin( 2.0 * meanSunAnomaly));
            }
        }

        private Degree MeanSunAnomaly
        {
            get { return new Degree(357.5291 + .98560028 * DaysFrom2000); }
        }

        private Degree EquationOfCenter
        {
            get
            {
                double meanSunAnomaly = MeanSunAnomaly.ToRadian().Value;
                return new Degree(1.9148 * Math.Sin(meanSunAnomaly) + .02 * Math.Sin(2 * meanSunAnomaly) + .0003 * Math.Sin(3 * meanSunAnomaly));
            }
        }

        public double JulianCycleSince2000(Degree longitude)
        {
            return Math.Floor((DaysFrom2000 - (-longitude.Value / 360.0)) + 0.5);
        }

        public double ApproximateSolarNoon(Degree longitude)
        {
            return 2451545.0009 + (-longitude.Value/360.0) + JulianCycleSince2000(longitude);
        }

        public DateTime SolarTransit(Degree longitude)
        {
            double meanSunAnomaly = MeanSunAnomaly.ToRadian().Value;
            return TimeUtils.JulianDateToUtc(ApproximateSolarNoon(longitude) + .0053 * Math.Sin(meanSunAnomaly) - .0069 * Math.Sin(2 * EclipticSunLongitude.ToRadian().Value));
        }

        public double JulianDate
        {
            get { return _julianDate; }
        }

        public Degree AxialTilt
        {
            get
            {
                return _axialTilt ?? (_axialTilt = new Degree(23, 26, 21.448)
                                                   - new Degree(0, 0, 4680.93) * Jears10000From2000
                                                   - new Degree(0, 0, 1.55) * Math.Pow(Jears10000From2000, 2)
                                                   + new Degree(0, 0, 1999.25) * Math.Pow(Jears10000From2000, 3)
                                                   - new Degree(0, 0, 51.38) * Math.Pow(Jears10000From2000, 4)
                                                   - new Degree(0, 0, 249.67) * Math.Pow(Jears10000From2000, 5)
                                                   - new Degree(0, 0, 39.05) * Math.Pow(Jears10000From2000, 6)
                                                   + new Degree(0, 0, 7.12) * Math.Pow(Jears10000From2000, 7)
                                                   + new Degree(0, 0, 27.87) * Math.Pow(Jears10000From2000, 8)
                                                   + new Degree(0, 0, 5.79) * Math.Pow(Jears10000From2000, 9)
                                                   + new Degree(0, 0, 2.45) * Math.Pow(Jears10000From2000, 10));
            }
        }

        public Radian RightAscension
        {
            get { return new Radian(Math.Atan(Math.Cos(AxialTilt.ToRadian().Value) * Math.Tan(EclipticSunLongitude.ToRadian().Value))); }
        }

        public Radian Declination
        {
            get { return new Radian(Math.Asin(Math.Sin(AxialTilt.ToRadian().Value) * Math.Sin(EclipticSunLongitude.ToRadian().Value))); }
        }

        //public HorizontalCoordinates HorizontalCoordinates(Radian latitude, Radian longitude)
        //{
        //    return new EquatorialCoordinates(RightAscension, Declination).ToHorizontalCoordinates(latitude, longitude, DaysFrom2000);
        //}
    }
}
