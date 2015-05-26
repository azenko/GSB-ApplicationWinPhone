using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GSB_FicheFrais
{
    public class GetUserResult
    {
        public int id { get; set; }
        public string username { get; set; }
        public string username_canonical { get; set; }
        public string email { get; set; }
        public string email_canonical { get; set; }
        public string enabled { get; set; }
        public string salt { get; set; }
        public string password { get; set; }
        public string last_login { get; set; }
        public string locked { get; set; }
        public string expired { get; set; }
        public string expires_at { get; set; }
        public string confirmation_token { get; set; }
        public string password_requested_at { get; set; }
        public string roles { get; set; }
        public string credentials_expired { get; set; }
        public string credentials_expire_at { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string adresse { get; set; }
        public string cp { get; set; }
        public string ville { get; set; }
        public string dateEmbauche { get; set; }
    }

}
