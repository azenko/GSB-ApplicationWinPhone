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

namespace GSB_FicheFrais
{
    public partial class Fiche : PhoneApplicationPage
    {
        public String username = null;

        public Fiche()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string user;
            if (NavigationContext.QueryString.TryGetValue("user", out user))
            {
                this.username = user;
            }
            base.OnNavigatedTo(e);
        }
    }
}