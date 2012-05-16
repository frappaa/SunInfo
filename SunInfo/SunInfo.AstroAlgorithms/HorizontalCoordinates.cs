using System;

namespace SunInfo.AstroAlgorithms
{
    public class HorizontalCoordinates
    {
        private readonly Radian _azimuth;
        private readonly Radian _altitude;

        public HorizontalCoordinates(Radian azimuth, Radian altitude)
        {
            _azimuth = azimuth;
            _altitude = altitude;
        }

        public Radian Altitude
        {
            get { return _altitude; }
        }

        public Radian Azimuth
        {
            get { return _azimuth; }
        }

        public EquatorialCoordinates ToEquatorialCoordinates(Radian latitude)
        {
            throw new NotImplementedException();
        }
    }
}
