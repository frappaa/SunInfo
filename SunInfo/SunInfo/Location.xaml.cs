using System;
using System.Globalization;
using System.Windows;
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

        private short? _latDeg = null;
        private short? _latMin = null;
        private short? _latSec = null;

        private short? _lonDeg = null;
        private short? _lonMin = null;
        private short? _lonSec = null;

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

            _latDeg = _latitude.Degrees;
            _latMin = _latitude.Minutes;
            _latSec = (short?) Math.Round(_latitude.Seconds);

            textBoxLatDeg.Text = _latDeg.Value.ToString(CultureInfo.InvariantCulture);
            textBoxLatMin.Text = _latMin.Value.ToString(CultureInfo.InvariantCulture);
            textBoxLatSec.Text = _latSec.Value.ToString(CultureInfo.InvariantCulture);

            pickerLatEmisph.SelectedIndex = _latitude.IsNegative ? 1 : 0;
            
            _lonDeg = _longitude.Degrees;
            _lonMin = _longitude.Minutes;
            _lonSec = (short?)Math.Round(_longitude.Seconds);

            textBoxLonDeg.Text = _lonDeg.Value.ToString(CultureInfo.InvariantCulture);
            textBoxLonMin.Text = _lonMin.Value.ToString(CultureInfo.InvariantCulture);
            textBoxLonSec.Text = _lonSec.Value.ToString(CultureInfo.InvariantCulture);

            pickerLonEmisph.SelectedIndex = _longitude.IsNegative ? 1 : 0;

            checkBoxUseGps.IsChecked = _useGps;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnCheck(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State[StateKeys.UseGps] = _useGps;
            if (_useGps.HasValue && !_useGps.Value)
            {
                if (_latDeg.HasValue && _latMin.HasValue && _latSec.HasValue)
                {
                    var neg = (short)(pickerLatEmisph.SelectedIndex == 0 ? 1 : -1);
                    _latitude = new Degree((short) (_latDeg.Value * neg), (short) (_latMin.Value * neg), _latSec.Value * neg);
                }

                if (_lonDeg.HasValue && _lonMin.HasValue && _lonSec.HasValue)
                {
                    var neg = (short)(pickerLonEmisph.SelectedIndex == 0 ? 1 : -1);
                    _longitude = new Degree((short)(_lonDeg.Value * neg), (short)(_lonMin.Value * neg), _lonSec.Value * neg);
                }
            }

            PhoneApplicationService.Current.State[StateKeys.CurrLatitude] = _latitude;
            PhoneApplicationService.Current.State[StateKeys.CurrLongitude] = _longitude;
            NavigationService.GoBack();
        }

        private void OnUseGpsChecked(object sender, System.Windows.RoutedEventArgs args)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            _useGps = checkBoxUseGps.IsChecked;
            bool isEnabled = _useGps.HasValue && !_useGps.Value;
            textBoxLatDeg.IsEnabled = isEnabled;
            textBoxLatMin.IsEnabled = isEnabled;
            textBoxLatSec.IsEnabled = isEnabled;
            pickerLatEmisph.IsEnabled = isEnabled;
            textBoxLonDeg.IsEnabled = isEnabled;
            textBoxLonMin.IsEnabled = isEnabled;
            textBoxLonSec.IsEnabled = isEnabled;
            pickerLonEmisph.IsEnabled = isEnabled;
        }

        private void OnUseGpsUnchecked(object sender, System.Windows.RoutedEventArgs args)
        {
            EnableControls();
        }

        private void OnTextBoxLatDegLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(textBoxLatDeg.Text))
            {
                short val;
                if (!short.TryParse(textBoxLatDeg.Text, out val) || val < 0 || val > 90)
                {
                    MessageBox.Show("The latitude degree field must be an integer number between 0 and 90.");
                    textBoxLatDeg.Focus();
                } else
                {
                    _latDeg = val;
                }
            }
        }

        private void OnTextBoxLatMinLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLatMin.Text))
            {
                short val;
                if (!short.TryParse(textBoxLatMin.Text, out val) || val < 0 || val >= 60)
                {
                    MessageBox.Show("The latitude minutes field must be an integer number between 0 and 59.");
                    textBoxLatMin.Focus();
                }
                else
                {
                    if (_latDeg.HasValue && _latDeg.Value == 90 && val > 0)
                    {
                        MessageBox.Show("The latitude minutes field must be 0.");
                        textBoxLatMin.Focus();
                    }
                    else
                    {
                        _latMin = val;
                    }
                }
            }
        }

        private void OnTextBoxLatSecLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLatSec.Text))
            {
                short val;
                if (!short.TryParse(textBoxLatSec.Text, out val) || val < 0 || val >= 60)
                {
                    MessageBox.Show("The latitude seconds field must be an integer number between 0 and 59.");
                    textBoxLatSec.Focus();
                }
                else
                {
                    if (_latDeg.HasValue && _latDeg.Value == 90 && val > 0)
                    {
                        MessageBox.Show("The latitude seconds field must be 0.");
                        textBoxLatSec.Focus();
                    } else
                    {
                        _latSec = val;
                    }
                }
            }
        }

        private void OnTextBoxLonDegLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLonDeg.Text))
            {
                short val;
                if (!short.TryParse(textBoxLonDeg.Text, out val) || val < 0 || val > 180)
                {
                    MessageBox.Show("The longitude degree field must be an integer number between 0 and 180.");
                    textBoxLonDeg.Focus();
                }
                else
                {
                    _lonDeg = val;
                }
            }
        }

        private void OnTextBoxLonMinLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLonMin.Text))
            {
                short val;
                if (!short.TryParse(textBoxLonMin.Text, out val) || val < 0 || val >= 60)
                {
                    MessageBox.Show("The longitude minutes field must be an integer number between 0 and 59.");
                    textBoxLonMin.Focus();
                }
                else
                {
                    if (_lonDeg.HasValue && _lonDeg.Value == 180 && val > 0)
                    {
                        MessageBox.Show("The longitude minutes field must be 0.");
                        textBoxLonMin.Focus();
                    }
                    else
                    {
                        _lonMin = val;
                    }
                }
            }
        }

        private void OnTextBoxLonSecLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLonSec.Text))
            {
                short val;
                if (!short.TryParse(textBoxLonSec.Text, out val) || val < 0 || val >= 60)
                {
                    MessageBox.Show("The longitude seconds field must be an integer number between 0 and 59.");
                    textBoxLonSec.Focus();
                }
                else
                {
                    if (_lonDeg.HasValue && _lonDeg.Value == 180 && val > 0)
                    {
                        MessageBox.Show("The longitude seconds field must be 0.");
                        textBoxLonSec.Focus();
                    }
                    else
                    {
                        _lonSec = val;
                    }
                }
            }
        }
    }
}