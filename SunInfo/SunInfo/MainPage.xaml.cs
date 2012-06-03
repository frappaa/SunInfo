﻿using System;
using System.Device.Location;
using System.Globalization;
using System.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _geoCoordinateWatcher;
        
        private Timer _timer;

        private Degree _currLatitude = new Degree(0);
        private Degree _currLongitude = new Degree(0);

        private DateTime _currUtcDateTime;
        private readonly SimulationStep _simulationStep;

        private readonly SunDataProvider _sunDataProvider = new SunDataProvider();

        public MainPage()
        {
            InitializeComponent();
            _timer = new Timer(OnTimerEvent, null, 0, 1000);
            StartLocationService();
            _currUtcDateTime = DateTime.UtcNow;
            _simulationStep = new SimulationStep();
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
            var sunData = _sunDataProvider.Get(_currUtcDateTime, _currLatitude, _currLongitude);
            FillUiItems(sunData);
            _currUtcDateTime += _simulationStep.ToTimeSpan();
        }

        private void FillUiItems(SunData sunData)
        {
            textBlockJD.Text = sunData.JulianDate.ToString("0.00000");
            textBlockUTC.Text = sunData.UtcTime.ToString(CultureInfo.InvariantCulture);
            textBlockLocalTime.Text = sunData.LocalTime.ToString(CultureInfo.InvariantCulture);
            textBlockSunEarthDistAU.Text = sunData.SunEarthDistAU.ToString("0.00000000");
            textBlockSunEarthDistKm.Text = sunData.SunEarthDistKm.ToString("### ### ##0");
            textBlockAxialTilt.Text = sunData.AxialTilt.ToString(true);
            textBlockRA.Text = string.Format("{0}h {1}m {2}s", sunData.RightAscension.Hours, sunData.RightAscension.Minutes, sunData.RightAscension.Seconds);
            textBlockDec.Text = sunData.Declination.ToString();
            textBlockAngularDiameter.Text = sunData.AngularDiameter.ToString(true);
            textBlockHourAngle.Text = string.Format("{0}h {1}m {2}s", sunData.HourAngle.Hours, sunData.HourAngle.Minutes, sunData.HourAngle.Seconds);
            textBlockAz.Text = sunData.Azimuth.ToString();
            textBlockAlt.Text = sunData.Altitude.ToString();
            textBlockShadowRatio.Text = double.IsNaN(sunData.ShadowRatio) ? "-" : sunData.ShadowRatio.ToString("0.000");
            DateTime? sunrise = sunData.Sunrise;
            textBlockSunrise.Text = sunrise == null ? "-" : sunrise.Value.ToLocalTime().ToString("HH:mm");
            DateTime? solarTransit = sunData.Transit;
            textBlockTransit.Text = solarTransit == null ? "-" : solarTransit.Value.ToLocalTime().ToString("HH:mm");
            DateTime? sunset = sunData.Sunset;
            textBlockSunset.Text = sunset == null ? "-" : sunset.Value.ToLocalTime().ToString("HH:mm");
        }

        private void OnRew(object sender, EventArgs e)
        {
            _simulationStep.Rew();
            RefreshInfo();
        }

        private void OnNormal(object sender, EventArgs e)
        {
            _simulationStep.Normal();
            RefreshInfo();
        }

        private void OnNow(object sender, EventArgs e)
        {
            _currUtcDateTime = DateTime.UtcNow;
            RefreshInfo();
        }

        private void OnFf(object sender, EventArgs e)
        {
            _simulationStep.Ff();
            RefreshInfo();
        }

        private void OnAbout(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void OnTime(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["CurrLocalDateTime"] = _currUtcDateTime.ToLocalTime();
            NavigationService.Navigate(new Uri("/Time.xaml", UriKind.Relative));
        }

        private void OnLocation(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Location.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("CurrLocalDateTime"))
            {
                _currUtcDateTime = ((DateTime)PhoneApplicationService.Current.State["CurrLocalDateTime"]).ToUniversalTime();
                RefreshInfo();
            }
        }
    }
}