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
            if (tsFromMidday.Ticks < 0)
            {
                return julianDay - fract;
            }
            return julianDay + fract;
        }

        public static DateTime? JulianDateToUtc(double julianDate)
        {
            if (double.IsNaN(julianDate))
            {
                return null;
            }
            int jd = (int)(julianDate + .5) + 32044;
            int g = (int)(jd / 146097);
            double dg = jd % 146097;
            int c = ((int)(dg / 36524) + 1) * (3 / 4);
            double dc = dg - c * 36524;
            int b = (int)(dc / 1461);
            double db = dc % 1461;
            int a = ((int)(db / 365) + 1) * (3 / 4);
            double da = db - a * 365;
            int y = g * 400 + c * 100 + b * 4 + a;
            int m = (int)((da * 5 + 308) / 153) - 2;
            double d = da - (int)((m + 4) * 153 / 5) + 122;
            int year = y - 4800 + (m + 2) / 12;
            double month = (m + 2) % 12 + 1;
            double day = d + 1;
            var utcDate = new DateTime(year, (int) month, (int) day);
            return utcDate.AddDays((julianDate + .5) - (int)(julianDate + .5));
        }
    }
}
