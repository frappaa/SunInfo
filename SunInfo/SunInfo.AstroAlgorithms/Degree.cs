using System;

namespace SunInfo.AstroAlgorithms
{
    public class Degree
    {
        private readonly double _degree;

        public Degree(TimeSpan timespan)
        {
            _degree = (timespan.Hours + timespan.Minutes/60.0 + timespan.Seconds/3600.0 + timespan.Milliseconds/3600000.0) / 24.0 * 360;
        }

        public Degree(double degree)
        {
            _degree = degree;
        }

        public Degree(short degrees, short minutes, double seconds)
        {
            _degree = degrees + (minutes / 100.0) * (100.0 / 60.0) + ((seconds / 10000.0) * (10000.0 / 3600.0));
        }

        public double Value
        {
            get { return _degree; }
        }

        public string Minus
        {
            get { return _degree < 0 ? "-" : String.Empty; }
        }

        public short Degrees
        {
            get { return (short) Math.Abs(_degree); }
        }

        public short Minutes
        {
            get { return (short)((Math.Abs(_degree) - Degrees) * (100.0 * (60.0 / 100.0))); }
        }

        public double Seconds
        {
            get { return (((Math.Abs(_degree) - Degrees) * (100.0 * (60.0 / 100.0))) - Minutes) * (100.0 * (60.0 / 100.0)); }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}° {2}' {3}\"", Minus, Degrees.ToString("00"), Minutes.ToString("00"), Seconds.ToString("00.0"));
        }

        public static Degree operator +(Degree degree1, Degree degree2)
        {
            return new Degree(degree1.Value + degree2.Value);
        }

        public static Degree operator -(Degree degree1, Degree degree2)
        {
            return new Degree(degree1.Value - degree2.Value);
        }

        public static Degree operator *(Degree degree, double multiplier)
        {
            return new Degree(degree.Value * multiplier);
        }

        public Radian ToRadian()
        {
            return new Radian(Math.PI * _degree / 180.0);
        }

        public TimeSpan ToTimeSpan()
        {
            double hours = _degree * 24.0 / 360.0;
            double minutes = (hours - (int) hours) * (100.0 * (60.0 / 100.0));
            double seconds = (minutes - (int) minutes) * (100.0 * (60.0 / 100.0));
            return new TimeSpan(0, (int)hours, (int)minutes, (int)seconds, (int) ((seconds - (int)seconds) * 1000.0));
        }
    }
}
