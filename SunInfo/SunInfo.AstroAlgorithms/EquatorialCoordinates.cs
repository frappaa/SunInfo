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
            double rightSide1 = Math.Sin(latitude.Value) * Math.Sin(Declination.Value) + Math.Cos(latitude.Value)*Math.Cos(Declination.Value)*Math.Cos(RightAscension.Value);
            return null;
        }
    }
}
