using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace SunInfo
{
    public partial class Location : PhoneApplicationPage
    {
        public Location()
        {
            InitializeComponent();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnCheck(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}