using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SunInfo.AstroAlgorithms;

namespace SunInfo
{
    public partial class Location : PhoneApplicationPage
    {
        private bool? _useGps = true;
        private Degree _latitude = new Degree(0);
        private Degree _longitude = new Degree(0);

        public Location()
        {
            InitializeComponent();
            if (PhoneApplicationService.Current.State.ContainsKey(StateKeys.CurrLatitude))
            {
                _latitude = (Degree)PhoneApplicationService.Current.State[StateKeys.CurrLatitude];
            }
            if (PhoneApplicationService.Current.State.ContainsKey(StateKeys.CurrLongitude))
            {
                _longitude = (Degree)PhoneApplicationService.Current.State[StateKeys.CurrLongitude];
            }
            textBoxLatitude.Text = Utils.GetLatitudeString(_latitude);
            textBoxLongitude.Text = Utils.GetLongitudeString(_longitude);
            checkBoxUseGps.IsChecked = _useGps;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnCheck(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State[StateKeys.UseGps] = _useGps;
            PhoneApplicationService.Current.State[StateKeys.CurrLatitude] = _latitude;
            PhoneApplicationService.Current.State[StateKeys.CurrLongitude] = _longitude;
            NavigationService.GoBack();
        }

        private void OnUseGpsChecked(object sender, System.Windows.RoutedEventArgs args)
        {
            _useGps = checkBoxUseGps.IsChecked;
            textBoxLatitude.IsEnabled = _useGps.HasValue && !_useGps.Value;
            textBoxLongitude.IsEnabled = _useGps.HasValue && !_useGps.Value;
        }

        private void OnUseGpsUnchecked(object sender, System.Windows.RoutedEventArgs args)
        {
            _useGps = checkBoxUseGps.IsChecked;
            textBoxLatitude.IsEnabled = _useGps.HasValue && !_useGps.Value;
            textBoxLongitude.IsEnabled = _useGps.HasValue && !_useGps.Value;
        }
    }
}