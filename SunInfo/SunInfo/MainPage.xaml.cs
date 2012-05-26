using System;
using System.Device.Location;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _watcher;
        private Timer _timer;

        private Degree _currLatitude = new Degree(0);
        private Degree _currLongitude = new Degree(0);

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _timer = new Timer(OnTimerEvent, null, 0, 250);
            StartLocationService();
        }

        private void StartLocationService()
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) {MovementThreshold = 50};
            _watcher.PositionChanged += OnPositionChanged;
            _watcher.Start();
        }

        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => RefreshPosition(e.Position));
        }

        void RefreshPosition(GeoPosition<GeoCoordinate> position)
        {
            _currLatitude = new Degree(position.Location.Latitude);
            _currLongitude = new Degree(position.Location.Longitude);
        }

        void OnTimerEvent(object o)
        {
            Deployment.Current.Dispatcher.BeginInvoke(RefreshInfo);
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
            textBlockTransit.Text = string.Format("Transit: {0}", sunInfoCalculator.SolarTransit(_currLongitude).ToLocalTime().ToString("HH:mm:ss"));
        }

        private void OnButtonRefreshClick(object sender, RoutedEventArgs e)
        {
            _watcher.Stop();
            _watcher.Start();
        }
    }
}