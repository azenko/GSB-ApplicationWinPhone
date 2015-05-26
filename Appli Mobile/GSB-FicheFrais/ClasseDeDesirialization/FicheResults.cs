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
    public class FicheResults
    {
        public int id { get; set; }
        public int etat_id { get; set; }
        public string compte_id { get; set; }
        public string moteur_id { get; set; }
        public int montantValide { get; set; }
        public string nbJustificatif { get; set; }
        public string dateModification { get; set; }
        public string mois { get; set; }
    }
}
