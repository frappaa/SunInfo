using System;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public class SunData
    {
        public double JulianDate { get; set; }
        public DateTime UtcTime { get; set; }
        public DateTime LocalTime { get; set; }
        public double SunEarthDistAU { get; set; }
        public double SunEarthDistKm { get; set; }
        public Degree AxialTilt { get; set; }
        public TimeSpan RightAscension { get; set; }
        public Degree Declination { get; set; }
        public Degree AngularDiameter { get; set; }
        public TimeSpan HourAngle { get; set; }
        public Degree Azimuth { get; set; }
        public Degree Altitude { get; set; }
        public double ShadowRatio { get; set; }
        public DateTime? Sunrise { get; set; }
        public DateTime? Transit { get; set; }
        public DateTime? Sunset { get; set; }
    }
}
