using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
    public partial class Fiche : PhoneApplicationPage
    {
        public String username = null;
        public int user_id = 0;
        public String theauthkey = null;

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
            string authkey;
            if (NavigationContext.QueryString.TryGetValue("authkey", out authkey))
            {
                this.theauthkey = authkey;
            }
            GetCompteID(this.username);
            base.OnNavigatedTo(e);
        }

        public void GetCompteID(String username)
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "Compte/username/" + this.username + "/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<GetUserResult>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    this.user_id = response.Data[0].id;
                    LoadFiche();
                }
            });
        }

        public void LoadFiche()
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "FicheFrais/compte_id/" + this.user_id + "/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<FicheResults>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    pivotFiche.Items.Clear();
                    foreach (FicheResults result in response.Data)
                    {
                        PivotItem pi = new PivotItem();
                        DateTime dtf = DateTime.ParseExact(result.mois, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        pi.Header = UppercaseFirst(dtf.ToString("MMMM")) + " " + dtf.Year;

                        FichedeFrais FdF = new FichedeFrais();

                        FicheDeFraisTemplate tpl = new FicheDeFraisTemplate();
                        GetEtat(result, pi, FdF, tpl);
                    }
                }
            });
        }

        public void GetEtat(FicheResults result, PivotItem pi, FichedeFrais FdF, FicheDeFraisTemplate tpl)
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "Etat/id/" + result.etat_id + "/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<EtatResult>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tpl.etat = response.Data[0].libelle;
                    tpl.montantValide = result.montantValide + " €";

                    GetTypeForfait(result, pi, FdF, tpl);
                }
            });
        }

        public void GetTypeForfait(FicheResults result, PivotItem pi, FichedeFrais FdF, FicheDeFraisTemplate tpl)
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "Forfait/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<TypeForfaitResult>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    GetQuantite(response.Data, result, pi, FdF, tpl);
                }
            });
        }

        public void GetQuantite(List<TypeForfaitResult> tfr, FicheResults result, PivotItem pi, FichedeFrais FdF, FicheDeFraisTemplate tpl)
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "LigneForfait/fiche_id/" + result.id + "/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<LigneForfaitResult>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tpl.quantForfait_A = response.Data[0].quantite;
                    tpl.libeForfait_A = tfr[0].libelle;
                    tpl.quantForfait_B = response.Data[1].quantite;
                    tpl.libeForfait_B = tfr[1].libelle;
                    tpl.quantForfait_C = response.Data[2].quantite;
                    tpl.libeForfait_C = tfr[2].libelle;
                    tpl.quantForfait_D = response.Data[3].quantite;
                    tpl.libeForfait_D = tfr[3].libelle;

                    GetHorsForfait(result, pi, FdF, tpl);
                }
            });
        }

        public void GetHorsForfait(FicheResults result, PivotItem pi, FichedeFrais FdF, FicheDeFraisTemplate tpl)
        {
            var client = new RestClient();
            client.BaseUrl = "http://asukazenko.pw/gsbapi/index.php";
            client.AddDefaultHeader("Cache-Control", "no-cache");

            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.Resource = "HorsForfait/fiche_id/" + result.id + "/?auth_key=" + this.theauthkey;

            client.ExecuteAsync<List<HorsForfaitResult>>(request, (response)
            =>
            {
                if (response.ResponseStatus != ResponseStatus.Error
                    && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<HorsForfaitList> SeulValide = new List<HorsForfaitList>();
                    HorsForfaitList hfla = new HorsForfaitList();
                    hfla.TheText = "Date - Libelle - Montant";
                    SeulValide.Add(hfla);

                    foreach (HorsForfaitResult hfr in response.Data)
                    {
                        if (hfr.refuser == "0")
                        {
                            HorsForfaitList hfl = new HorsForfaitList();
                            hfl.TheText = hfr.date + " - " + hfr.libelle + " - " + hfr.montant + " €";
                            SeulValide.Add(hfl);
                        }
                    }

                    if (SeulValide.Count == 1)
                    {
                        HorsForfaitList hfl = new HorsForfaitList();
                        hfl.TheText = "Aucune Ligne Présente";
                        SeulValide.Add(hfl);
                    }

                    tpl.HorsForfaitList = SeulValide;

                    FdF.DataContext = tpl;

                    pi.Content = FdF;

                    pivotFiche.Items.Add(pi);
                }
            });
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private void appbar_buttonadd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Non Implémenté");
        }

        private void appbar_buttonedit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Non Implémenté");
        }
    }
}