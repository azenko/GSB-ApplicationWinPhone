﻿#pragma checksum "C:\Users\tpepsi\documents\visual studio 2010\Projects\GSB-FicheFrais\GSB-FicheFrais\FichedeFrais.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A6D21C87F00FF62A2458B154E9A4E70"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18408
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace GSB_FicheFrais {
    
    
    public partial class FichedeFrais : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock etat_fiche;
        
        internal System.Windows.Controls.TextBlock ef;
        
        internal System.Windows.Controls.TextBlock montant_total_valider;
        
        internal System.Windows.Controls.TextBlock mtv;
        
        internal System.Windows.Controls.TextBlock Elem_Forfait;
        
        internal System.Windows.Controls.TextBlock Elem_HForfait;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/GSB-FicheFrais;component/FichedeFrais.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.etat_fiche = ((System.Windows.Controls.TextBlock)(this.FindName("etat_fiche")));
            this.ef = ((System.Windows.Controls.TextBlock)(this.FindName("ef")));
            this.montant_total_valider = ((System.Windows.Controls.TextBlock)(this.FindName("montant_total_valider")));
            this.mtv = ((System.Windows.Controls.TextBlock)(this.FindName("mtv")));
            this.Elem_Forfait = ((System.Windows.Controls.TextBlock)(this.FindName("Elem_Forfait")));
            this.Elem_HForfait = ((System.Windows.Controls.TextBlock)(this.FindName("Elem_HForfait")));
        }
    }
}
