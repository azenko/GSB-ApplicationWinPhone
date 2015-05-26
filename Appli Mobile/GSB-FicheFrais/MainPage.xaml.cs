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

using RestSharp;

namespace GSB_FicheFrais
{

    public partial class MainPage : PhoneApplicationPage
    {
        public String theauthkey = "d4349b94516a530a74ecba09662fb225382fbed89d0dd141858d6ffceb1fde76";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {

            string the_password = "";
            string the_username = "";

            the_username = username.Text;
            the_password = password.Password;

            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "login/" + the_username + "/" + the_password + "/?auth_key=" + theauthkey;

            client.ExecuteAsync<LoginList>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    int code = 0;
                    if (response.Data.success != null)
                    {
                        code = response.Data.success.code;
                    }
                    else if (response.Data.error != null)
                    {
                        code = response.Data.error.code;
                    }

                    if (code == 200)
                    {
                        NavigationService.Navigate(new Uri("/Fiche.xaml?user=" + the_username + "&authkey=" + theauthkey, UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show("Mauvais Nom d'utilisateur/Mot De Passe");
                        this.username.Text = "";
                        this.password.Password = "";
                    }
                }
                else
                {
                    MessageBox.Show("API Error");
                }
            });
        }
    }
}