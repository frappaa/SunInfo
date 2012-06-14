using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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
            if (PhoneApplicationService.Current.State.ContainsKey(StateKeys.UseGps))
            {
                _useGps = (bool)PhoneApplicationService.Current.State[StateKeys.UseGps];
            }
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

            EnableControls();
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
                string globalMessage = String.Empty;
                string message;
                TryParseLatDeg(out message);
                globalMessage = globalMessage + message;
                TryParseLatMin(out message);
                globalMessage = globalMessage + message;
                TryParseLatSec(out message);
                globalMessage = globalMessage + message;
                TryParseLonDeg(out message);
                globalMessage = globalMessage + message;
                TryParseLonMin(out message);
                globalMessage = globalMessage + message;
                TryParseLonSec(out message);
                globalMessage = globalMessage + message;
                if (!String.IsNullOrEmpty(globalMessage))
                {
                    MessageBox.Show(globalMessage);
                }
                else
                {
                    if (_latDeg.HasValue && _latMin.HasValue && _latSec.HasValue)
                    {
                        var neg = (short) (pickerLatEmisph.SelectedIndex == 0 ? 1 : -1);
                        _latitude = new Degree((short) (_latDeg.Value*neg), (short) (_latMin.Value*neg),
                                               _latSec.Value*neg);
                    }

                    if (_lonDeg.HasValue && _lonMin.HasValue && _lonSec.HasValue)
                    {
                        var neg = (short) (pickerLonEmisph.SelectedIndex == 0 ? 1 : -1);
                        _longitude = new Degree((short) (_lonDeg.Value*neg), (short) (_lonMin.Value*neg),
                                                _lonSec.Value*neg);
                    }
                }
            }

            PhoneApplicationService.Current.State[StateKeys.CurrLatitude] = _latitude;
            PhoneApplicationService.Current.State[StateKeys.CurrLongitude] = _longitude;
            NavigationService.GoBack();
        }

        private void OnUseGpsChecked(object sender, RoutedEventArgs args)
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

        private void OnUseGpsUnchecked(object sender, RoutedEventArgs args)
        {
            EnableControls();
        }

        private static void OnLostFocus(TextBox textBox, ParseMethod parseMethod)
        {
            if (String.IsNullOrEmpty(textBox.Text))
            {
                return;
            }
            string message;
            if (parseMethod(out message))
            {
                return;
            }
            MessageBox.Show(message);
            textBox.Focus();
        }

        private delegate bool ParseMethod(out string message);


        private void OnTextBoxLatDegLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLatDeg, TryParseLatDeg);
        }

        private bool TryParseLatDeg(out string message)
        {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLatDeg.Text, out val) || val < 0 || val > 90)
            {
                message = "The latitude degree field must be an integer number between 0 and 90.";
                parsed = false;
            } else
            {
                _latDeg = val;
                if (val == 90)
                {
                    _latMin = _latSec = 0;
                    textBoxLatMin.Text = _latMin.Value.ToString(CultureInfo.InvariantCulture);
                    textBoxLatSec.Text = _latSec.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            return parsed;
        }

        private void OnTextBoxLatMinLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLatMin, TryParseLatMin);
        }

        private bool TryParseLatMin(out string message) {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLatMin.Text, out val) || val < 0 || val >= 60)
            {
                message = "The latitude minutes field must be an integer number between 0 and 59.";
                parsed = false;
            }
            else
            {
                if (_latDeg.HasValue && _latDeg.Value == 90 && val > 0)
                {
                    message = "The latitude minutes field must be 0.";
                    parsed = false;
                }
                else
                {
                    _latMin = val;
                }
            }
            return parsed;
        }

        private void OnTextBoxLatSecLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLatSec, TryParseLatSec);
        }

        private bool TryParseLatSec(out string message)
        {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLatSec.Text, out val) || val < 0 || val >= 60)
            {
                message ="The latitude seconds field must be an integer number between 0 and 59.";
                parsed = false;
            }
            else
            {
                if (_latDeg.HasValue && _latDeg.Value == 90 && val > 0)
                {
                    message = "The latitude seconds field must be 0.";
                    parsed = false;
                } else
                {
                    _latSec = val;
                }
            }
            return parsed;
        }

        private void OnTextBoxLonDegLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLonDeg, TryParseLonDeg);
        }

        private bool TryParseLonDeg(out string message)
        {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLonDeg.Text, out val) || val < 0 || val > 180)
            {
                message = "The longitude degree field must be an integer number between 0 and 180.";
                parsed = false;
            }
            else
            {
                _lonDeg = val;
                if (val == 180)
                {
                    _lonMin = _lonSec = 0;
                    textBoxLonMin.Text = _lonMin.Value.ToString(CultureInfo.InvariantCulture);
                    textBoxLonSec.Text = _lonSec.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            return parsed;
        }

        private void OnTextBoxLonMinLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLatMin, TryParseLonMin);
        }

        private bool TryParseLonMin(out string message)
        {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLonMin.Text, out val) || val < 0 || val >= 60)
            {
                message = "The longitude minutes field must be an integer number between 0 and 59.";
                parsed = false;
            }
            else
            {
                if (_lonDeg.HasValue && _lonDeg.Value == 180 && val > 0)
                {
                    message = "The longitude minutes field must be 0.";
                    parsed = false;
                }
                else
                {
                    _lonMin = val;
                }
            }
            return parsed;
        }

        private void OnTextBoxLonSecLostFocus(object sender, RoutedEventArgs e)
        {
            OnLostFocus(textBoxLatSec, TryParseLonSec);
        }

        private bool TryParseLonSec(out string message)
        {
            short val;
            message = String.Empty;
            bool parsed = true;
            if (!short.TryParse(textBoxLonSec.Text, out val) || val < 0 || val >= 60)
            {
                message = "The longitude seconds field must be an integer number between 0 and 59.";
                parsed = false;
            }
            else
            {
                if (_lonDeg.HasValue && _lonDeg.Value == 180 && val > 0)
                {
                    message = "The longitude seconds field must be 0.";
                    parsed = false;
                }
                else
                {
                    _lonSec = val;
                }
            }
            return parsed;
        }


    }
}