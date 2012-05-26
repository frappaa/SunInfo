using System;
using System.Globalization;

namespace SunInfo.AstroAlgorithms
{
    public static class TimeUtils
    {
        public static double UtcToJulianDate(DateTime utcDateTime)
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
            double fract = Math.Abs((double)tsFromMidday.Hours) / 24.0 + (Math.Abs(tsFromMidday.Minutes) / (24.0 * 60.0)) + Math.Abs(tsFromMidday.Seconds / (24.0 * 60.0 * 60.0));
            if (tsFromMidday.Hours < 0)
            {
                return julianDay - fract;
            }
            return julianDay + fract;
        }

        public static DateTime JulianDateToUtc(double julianDate)
        {
            double jd = julianDate + .5 + 32044.0;
            double g = Math.Floor(jd / 146097.0);
            double dg = jd % 146097.0;
            double c = Math.Floor((Math.Floor(dg / 36524.0) + 1.0) * 3.0 / 4.0);
            double dc = dg - c * 36524.0;
            double b = Math.Floor(dc / 1461.0);
            double db = dc % 1461.0;
            double a = Math.Floor((Math.Floor(db / 365.0) + 1.0) * 3.0 / 4.0);
            double da = db - a * 365.0;
            double y = g * 400.0 + c * 100.0 + b * 4.0 + a;
            double m = Math.Floor((da * 5.0 + 308.0) / 153.0) - 2.0;
            double d = da - Math.Floor(((m + 4.0) * 153) / 5.0) + 122.0;
            double year = y - 4800.0 + Math.Floor((m + 2.0) / 12.0);
            double month = (m + 2.0) % 12.0 + 1.0;
            double day = d + 1.0;
            var utcDate = new DateTime((int) year, (int) month, (int) day);
            return utcDate.AddDays(jd - Math.Floor(jd));
        }
    }
}
