using System;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public class SunDataProvider
    {
        private readonly SunInfoCalculator _calculator;
        
        public SunDataProvider()
        {
            _calculator = new SunInfoCalculator();    
        }

        public SunData Get(DateTime utcDateTime, Degree latitude, Degree longitude)
        {
            _calculator.SetTime(utcDateTime);
            var data = new SunData();
            data.JulianDate = _calculator.JulianDate;
            data.UtcTime = utcDateTime;
            data.LocalTime = utcDateTime.ToLocalTime();
            data.SunEarthDistAU = _calculator.SunEarthDistance;
            data.SunEarthDistKm = _calculator.SunEarthDistanceKm;
            data.AxialTilt = _calculator.AxialTilt;
            Radian rightAscension = _calculator.RightAscension;
            data.RightAscension = rightAscension.ToDegree().ToTimeSpan();
            Radian declination = _calculator.Declination;
            data.Declination = declination.ToDegree();
            data.AngularDiameter = _calculator.AngularDiameter.ToDegree();
            data.HourAngle = _calculator.HourAngle(longitude.ToRadian()).ToDegree().ToTimeSpan();
            var equatorialCoordinates = new EquatorialCoordinates(rightAscension, declination);
            var horizontalCoordinates = equatorialCoordinates.ToHorizontalCoordinates(latitude.ToRadian(), longitude.ToRadian(), utcDateTime);
            data.Azimuth = horizontalCoordinates.Azimuth.ToDegree();
            data.Altitude = horizontalCoordinates.Altitude.ToDegree();
            data.ShadowRatio = Utils.GetShadowRatio(data.Altitude);
            data.Sunrise = _calculator.Sunrise(longitude, latitude);
            data.Transit = _calculator.SolarTransit(longitude);
            data.Sunset = _calculator.Sunset(longitude, latitude);
            return data;
        }
    }
}
