using System;
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
    public class LoginList
    {
        public SuccessContainer success { get; set; }
        public ErrorContainer error { get; set; }
    }

    public class SuccessContainer
    {
        public int code { get; set; }
        public string status { get; set; }
    }

    public class ErrorContainer
    {
        public int code { get; set; }
        public string status { get; set; }
    }
}
