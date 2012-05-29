using System;
using System.Device.Location;
using System.Globalization;
using System.Threading;
using Microsoft.Phone.Controls;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _geoCoordinateWatcher;
        
        private Timer _timer;

        private Degree _currLatitude = new Degree(0);
        private Degree _currLongitude = new Degree(0);

        private readonly SunDataProvider _sunDataProvider = new SunDataProvider();

        public MainPage()
        {
            InitializeComponent();
            _timer = new Timer(OnTimerEvent, null, 0, 1000);
            StartLocationService();
        }

        private void StartLocationService()
        {
            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            _geoCoordinateWatcher.MovementThreshold = 50;
            _geoCoordinateWatcher.PositionChanged += OnPositionChanged;
            _geoCoordinateWatcher.Start();
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
            textBlockLat.Text = Utils.GetLatitudeString(_currLatitude);
            textBlockLon.Text = Utils.GetLongitudeString(_currLongitude);

            DateTime utcDateTime = DateTime.UtcNow;
            var sunData = _sunDataProvider.Get(utcDateTime, _currLatitude, _currLongitude);
            FillUiItems(sunData);
        }

        private void FillUiItems(SunData sunData)
        {
            textBlockJD.Text = sunData.JulianDate.ToString("0.00000");
            textBlockUTC.Text = sunData.UtcTime.ToString(CultureInfo.InvariantCulture);
            textBlockLocalTime.Text = sunData.LocalTime.ToLocalTime().ToString(CultureInfo.InvariantCulture);
            textBlockSunEarthDistAU.Text = sunData.SunEarthDistAU.ToString("0.00000000");
            textBlockSunEarthDistKm.Text = sunData.SunEarthDistKm.ToString("### ### ##0");
            textBlockAxialTilt.Text = sunData.AxialTilt.ToString();
            TimeSpan rightAscension = sunData.RightAscension;
            textBlockRA.Text = string.Format("{0}h {1}m {2}s", rightAscension.Hours, rightAscension.Minutes, rightAscension.Seconds);
            textBlockDec.Text = sunData.Declination.ToString();
            textBlockAngularDiameter.Text = sunData.AngularDiameter.ToString();
            var hourAngle = sunData.HourAngle;
            textBlockHourAngle.Text = string.Format("{0}h {1}m {2}s", hourAngle.Hours, hourAngle.Minutes, hourAngle.Seconds);
            textBlockAz.Text = sunData.Azimuth.ToString();
            textBlockAlt.Text = sunData.Altitude.ToString();
            double shadowRatio = sunData.ShadowRatio;
            textBlockShadowRatio.Text = double.IsNaN(shadowRatio) ? "-" : shadowRatio.ToString("0.000");
            DateTime? sunrise = sunData.Sunrise;
            textBlockSunrise.Text = sunrise == null ? "-" : sunrise.Value.ToLocalTime().ToString("HH:mm");
            DateTime? solarTransit = sunData.Transit;
            textBlockTransit.Text = solarTransit == null ? "-" : solarTransit.Value.ToLocalTime().ToString("HH:mm");
            DateTime? sunset = sunData.Sunset;
            textBlockSunset.Text = sunset == null ? "-" : sunset.Value.ToLocalTime().ToString("HH:mm");
        }
    }
}