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
    public class FicheDeFraisTemplate
    {
        // Etat de la fiche
        public string etat { get; set; }

        // Montant Validé de la fiche
        public string montantValide { get; set; }

        // Libelle des lignes Forfaits
        public string libeForfait_A { get; set; }
        public string libeForfait_B { get; set; }
        public string libeForfait_C { get; set; }
        public string libeForfait_D { get; set; }

        // Quantité des lignes Forfaits
        public string quantForfait_A { get; set; }
        public string quantForfait_B { get; set; }
        public string quantForfait_C { get; set; }
        public string quantForfait_D { get; set; }

        // Collections Hors Forfait
        public List<HorsForfaitList> HorsForfaitList { get; set; }
    }

    public class HorsForfaitList
    {
        public String TheText { get; set; }
    }
}
