using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SunInfo
{
    public partial class Time : PhoneApplicationPage
    {
        private DateTime _dateTime = DateTime.UtcNow;
        public Time()
        {
            InitializeComponent();
            if (PhoneApplicationService.Current.State.ContainsKey(StateKeys.CurrLocalDateTime))
            {
                _dateTime = (DateTime)PhoneApplicationService.Current.State[StateKeys.CurrLocalDateTime];
            }
            datePicker.Value = _dateTime;
            timePicker.Value = _dateTime;

        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnCheck(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State[StateKeys.CurrLocalDateTime] = _dateTime;
            NavigationService.GoBack();
        }

        private void OnDateChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != null)
                _dateTime = (DateTime)e.NewDateTime;
        }

        private void OnTimeChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != null)
                _dateTime = (DateTime)e.NewDateTime;
        }
    }
}