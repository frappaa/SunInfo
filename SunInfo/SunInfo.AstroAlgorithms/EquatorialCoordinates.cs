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

        public HorizontalCoordinates ToHorizontalCoordinates(Radian latitude, Radian longitude, DateTime utcTime)
        {
            var hourHangle = new SunInfoCalculator(utcTime).HourAngle(longitude);

            double rightSide1 = Math.Sin(latitude.Value) * Math.Sin(Declination.Value) +
                                Math.Cos(latitude.Value) * Math.Cos(Declination.Value) * Math.Cos(hourHangle.Value);

            double rightSide2 = Math.Cos(latitude.Value) * Math.Sin(Declination.Value) -
                                Math.Sin(latitude.Value) * Math.Cos(Declination.Value) * Math.Cos(hourHangle.Value);

            double rightSide3 = -Math.Cos(latitude.Value) * Math.Sin(hourHangle.Value);

            PolarCoordinates polarCoordinates = new CartesianCoordinates(rightSide2, rightSide3).ToPolarCoordinates();
            var azimuth = polarCoordinates.Angle;

            polarCoordinates = new CartesianCoordinates(polarCoordinates.Radius, rightSide1).ToPolarCoordinates();
            var altitude = polarCoordinates.Angle;

            double mustBe1 = polarCoordinates.Radius;

            return new HorizontalCoordinates(azimuth, altitude);
        }
    }
}
