namespace SunInfo.AstroAlgorithms
{
    public class PolarCoordinates
    {
        private readonly double _radius;
        private readonly Radian _angle;

        public PolarCoordinates(double radius, Radian angle)
        {
            _radius = radius;
            _angle = angle;
        }

        public Radian Angle
        {
            get { return _angle; }
        }

        public double Radius
        {
            get { return _radius; }
        }
    }
}
