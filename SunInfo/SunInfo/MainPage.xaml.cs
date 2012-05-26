using System;
using System.Device.Location;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using SunInfo.AstroAlgorithms;
using Microsoft.Devices.Sensors;

namespace SunInfo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _watcher;
        private Timer _timer;
        private readonly Motion _motion;

        private Degree _currLatitude = new Degree(0);
        private Degree _currLongitude = new Degree(0);

        private Degree _yaw = new Degree(0);
        private Degree _pitch = new Degree(0);
        private Degree _roll = new Degree(0);

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _timer = new Timer(OnTimerEvent, null, 0, 250);
            if (_motion == null && Motion.IsSupported)
            {
                _motion = new Motion();
                _motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(200);
                _motion.CurrentValueChanged += OnMotionCurrentValueChanged;
            }
            StartLocationService();
        }

        private void OnMotionCurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> args)
        {
            Dispatcher.BeginInvoke(() => MotionCurrentValueChanged(args.SensorReading));
        }

        private void MotionCurrentValueChanged(MotionReading args)
        {
            if (!_motion.IsDataValid)
            {
                return;
            }
            _yaw = new Radian(args.Attitude.Yaw).ToDegree();
            _pitch = new Radian(args.Attitude.Pitch).ToDegree();
            _roll = new Radian(args.Attitude.Roll).ToDegree();
        }

        private void StartLocationService()
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            _watcher.MovementThreshold = 50;
            _watcher.PositionChanged += OnPositionChanged;
            _watcher.Start();
        }

        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            Dispatcher.BeginInvoke(() => RefreshPosition(args.Position));
        }

        void RefreshPosition(GeoPosition<GeoCoordinate> position)
        {
            _currLatitude = new Degree(position.Location.Latitude);
            _currLongitude = new Degree(position.Location.Longitude);
        }

        void OnTimerEvent(object o)
        {
            Dispatcher.BeginInvoke(RefreshInfo);
        }

        void RefreshInfo()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            var sunInfoCalculator = new SunInfoCalculator(utcDateTime);
            textBlockJD.Text = string.Format("Julian Date: {0}", sunInfoCalculator.JulianDate.ToString("0.00000"));
            textBlockUTC.Text = string.Format("UTC: {0}", utcDateTime);
            textBlockLocalTime.Text = string.Format("Local Time: {0}", utcDateTime.ToLocalTime());
            textBlockSunEarthDistAU.Text = string.Format("Sun-Earth Distance (AU): {0}", sunInfoCalculator.SunEarthDistance.ToString("0.00000000"));
            textBlockSunEarthDistKm.Text = string.Format("Sun-Earth Distance (Km): {0}", sunInfoCalculator.SunEarthDistanceKm.ToString("### ### ##0"));
            textBlockAxialTilt.Text = string.Format("Axial Tilt: {0}", sunInfoCalculator.AxialTilt);
            Radian rightAscension = sunInfoCalculator.RightAscension;
            TimeSpan rightAscensionTimeSpan = rightAscension.ToDegree().ToTimeSpan();
            textBlockRA.Text = string.Format("Right Ascension: {0}h {1}m {2}s", rightAscensionTimeSpan.Hours, rightAscensionTimeSpan.Minutes, rightAscensionTimeSpan.Seconds);
            Radian declination = sunInfoCalculator.Declination;
            textBlockDec.Text = string.Format("Declination: {0}", declination.ToDegree());
            textBlockAngularDiameter.Text = string.Format("Angular Diameter: {0}", sunInfoCalculator.AngularDiameter.ToDegree());

            textBlockLat.Text = string.Format("Latitude: {0}", Utils.GetLatitudeString(_currLatitude));
            textBlockLon.Text = string.Format("Longitude: {0}", Utils.GetLongitudeString(_currLongitude));

            var horizontalCoordinates = new EquatorialCoordinates(rightAscension, declination).ToHorizontalCoordinates(_currLatitude.ToRadian(), _currLongitude.ToRadian(), utcDateTime);
            var hourAngleTimeSpan = sunInfoCalculator.HourAngle(_currLongitude.ToRadian()).ToDegree().ToTimeSpan();
            textBlockHourAngle.Text = string.Format("Hour Angle: {0}h {1}m {2}s", hourAngleTimeSpan.Hours, hourAngleTimeSpan.Minutes, hourAngleTimeSpan.Seconds);
            textBlockAz.Text = string.Format("Azimuth: {0}", horizontalCoordinates.Azimuth.ToDegree());
            textBlockAlt.Text = string.Format("Altitude: {0}", horizontalCoordinates.Altitude.ToDegree());
            double shadowRatio = Utils.GetShadowRatio(horizontalCoordinates.Altitude.ToDegree());
            textBlockShadowRatio.Text = string.Format("Shadow Ratio: {0}", double.IsNaN(shadowRatio) ? "-" : shadowRatio.ToString("0.000"));
            DateTime? sunrise = sunInfoCalculator.Sunrise(_currLongitude, _currLatitude);
            textBlockSunrise.Text = string.Format("Sunrise: {0}", sunrise == null ? "-" : sunrise.Value.ToLocalTime().ToString("HH:mm:ss"));
            DateTime? solarTransit = sunInfoCalculator.SolarTransit(_currLongitude);
            textBlockTransit.Text = string.Format("Transit: {0}", solarTransit == null ? "-" : solarTransit.Value.ToLocalTime().ToString("HH:mm:ss"));
            DateTime? sunset = sunInfoCalculator.Sunset(_currLongitude, _currLatitude);
            textBlockSunset.Text = string.Format("Sunset: {0}", sunset == null ? "-" : sunset.Value.ToLocalTime().ToString("HH:mm:ss"));
            
            textBlockYaw.Text = string.Format("Yaw: {0}", _yaw);
            textBlockPitch.Text = string.Format("Pitch: {0}", _pitch);
            textBlockRoll.Text = string.Format("Roll: {0}", _roll);
        }

        private void OnButtonRefreshClick(object sender, RoutedEventArgs e)
        {
            _watcher.Stop();
            _watcher.Start();
        }
    }
}