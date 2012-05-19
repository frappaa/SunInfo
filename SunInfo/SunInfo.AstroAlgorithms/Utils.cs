using System;

namespace SunInfo.AstroAlgorithms
{
    public static class Utils
    {
        public static string GetLatitudeString(Degree latitude)
        {
            string emisphere = latitude.Value > 0 ? "N" : (latitude.Value < 0 ? "S" : "N/S");
            return String.Format("{0}° {1}' {2}\" {3}", latitude.Degrees.ToString("00"), latitude.Minutes.ToString("00"), latitude.Seconds.ToString("00"), emisphere);
        }

        public static string GetLongitudeString(Degree longitude)
        {
            string emisphere = longitude.Value > 0 ? "E" : (longitude.Value < 0 ? "W" : "E/W");
            return String.Format("{0}° {1}' {2}\" {3}", longitude.Degrees.ToString("00"), longitude.Minutes.ToString("00"), longitude.Seconds.ToString("00"), emisphere);
        }

        public static double GetShadowRatio(Degree altitude)
        {
            var complementAngle = (new Degree(90) - altitude);
            if ((complementAngle.Value < 0) || (complementAngle.Value > 90))
            {
                return double.NaN;
            }
            return Math.Tan(complementAngle.ToRadian().Value);
        }

        //public static void ToDegreesMinutesSeconds(double degreesWithDecimals, out short degrees, out short minutes, out double seconds)
        //{
        //    var latlongAbs = Math.Abs(degreesWithDecimals);
        //    degrees = (short)latlongAbs;
        //    double minutesNotTruncated = (latlongAbs - degrees) * (100.0 * (60.0 / 100.0));
        //    minutes = (short)minutesNotTruncated;
        //    seconds = (minutesNotTruncated - minutes) * (100.0 * (60.0 / 100.0));
        //}

        //public static double DegreeToRadian(double angle)
        //{
        //    return Math.PI * angle / 180.0;
        //}

        //public static double RadianToDegree(double angle)
        //{
        //    return angle * (180.0 / Math.PI);
        //}
    }
}
