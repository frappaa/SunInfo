using System;

namespace SunInfo.AstroAlgorithms
{
    public class Radian
    {
        private readonly double _radian;

        public Radian(double radian)
        {
            _radian = radian;
        }

        public static Radian operator +(Radian radian1, Radian radian2)
        {
            return new Radian(radian1.Value + radian2.Value);
        }

        public static Radian operator -(Radian radian1, Radian radian2)
        {
            return new Radian(radian1.Value - radian2.Value);
        }

        public double Value
        {
            get { return _radian; }
        }

        public Degree ToDegree()
        {
            return new Degree(_radian * (180.0 / Math.PI));
        }
    }
}
