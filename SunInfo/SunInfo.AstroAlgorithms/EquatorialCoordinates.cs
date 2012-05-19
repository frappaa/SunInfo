using System;

namespace SunInfo.AstroAlgorithms
{
    public class EquatorialCoordinates
    {
        private readonly Radian _rightAscension;
        private readonly Radian _declination;

        public EquatorialCoordinates(Radian rightAscension, Radian declination)
        {
            _rightAscension = rightAscension;
            _declination = declination;
        }

        public Radian Declination
        {
            get { return _declination; }
        }

        public Radian RightAscension
        {
            get { return _rightAscension; }
        }

        public HorizontalCoordinates ToHorizontalCoordinates(Radian latitude)
        {
            double rightSide1 = Math.Sin(latitude.Value) * Math.Sin(Declination.Value) +
                                Math.Cos(latitude.Value)*Math.Cos(Declination.Value)*Math.Cos(RightAscension.Value);

            double rightSide2 = Math.Cos(latitude.Value)*Math.Sin(Declination.Value) -
                                Math.Sin(latitude.Value)*Math.Cos(Declination.Value)*Math.Cos(RightAscension.Value);

            double rightSide3 = -Math.Cos(latitude.Value)*Math.Sin(RightAscension.Value);

            double x = rightSide2;
            double y = rightSide3;

            double r = Math.Sqrt( Math.Pow(y, 2) + Math.Pow(x, 2));
            var theta = new Radian(Math.Atan2(y, x));

            var azimuth = theta;

            x = r;
            y = rightSide1;

            r = Math.Sqrt( Math.Pow(y, 2) + Math.Pow(x, 2));
            theta = new Radian(Math.Atan2(y, x));

            var altitude = theta;

            return new HorizontalCoordinates(azimuth, altitude);
        }
    }
}
