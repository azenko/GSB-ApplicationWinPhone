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
    public class HorsForfaitResult
    {
        public string id { get; set; }
        public string fiche_id { get; set; }
        public string libelle { get; set; }
        public string refuser { get; set; }
        public string date { get; set; }
        public string montant { get; set; }
    }
}
