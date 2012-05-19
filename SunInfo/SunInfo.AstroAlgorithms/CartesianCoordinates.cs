using System;

namespace SunInfo.AstroAlgorithms
{
    public class CartesianCoordinates
    {
        private readonly double _x;
        private readonly double _y;

        public CartesianCoordinates(double x, double y)
        {
            _x = x;
            _y = y;
        }
        
        public double Y
        {
            get { return _y; }
        }

        public double X
        {
            get { return _x; }
        }

        public PolarCoordinates ToPolarCoordinates()
        {
            double r = Math.Sqrt(Math.Pow(_y, 2) + Math.Pow(_x, 2));
            var theta = new Radian(Math.Atan2(_y, _x));
            return new PolarCoordinates(r, theta);
        }
    }
}
