﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dama_pije_sama_V2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IgranjeSIgracimaPage : ContentPage
    {
        public List<Karta> karte = new List<Karta>();
        static Random rnd = new Random();
        private readonly List<Igrac> ListaIgraca = new List<Igrac>();
        private Igrac trenutniIgrac;
        private int brojac = 0;
        private int Broj_odigranih_karata = 0;
        private string BrojacSekundi;
        Stopwatch StopWatch;
        public IgranjeSIgracimaPage(List<Igrac> igraci)
        {
            InitializeComponent();
            StvoriListu();
            ListaIgraca = igraci;
            slikaKarte.Source = karte.Find(x => x.Naziv == "PocetnaKarta").Naziv;
            OpisZadatkaLabel.Text = karte.Find(x => x.Naziv == "PocetnaKarta").Naredba;
            BrKarteLabel.Text = $"{karte.Count - 1}";
            trenutniIgrac = ListaIgraca.ElementAt(brojac);

            PokreniStopericu();
        }

        private void PokreniStopericu()
        {
            StopWatch = Stopwatch.StartNew();
        }

        private void StvoriListu()
        {
            karte.Add(new Karta("AsHerc", "KATEGORIJE"));
            karte.Add(new Karta("AsZelje", "KATEGORIJE"));
            karte.Add(new Karta("AsBundeva", "KATEGORIJE"));
            karte.Add(new Karta("AsZir", "KATEGORIJE"));
            karte.Add(new Karta("KraljHerc", "BIRAŠ TKO PIJE"));
            karte.Add(new Karta("KraljZelje", "BIRAŠ TKO PIJE"));
            karte.Add(new Karta("KraljBundeva", "BIRAŠ TKO PIJE"));
            karte.Add(new Karta("KraljZir", "BIRAŠ TKO PIJE"));
            karte.Add(new Karta("PocetnaKarta", "Swipe desno za početak"));
            karte.Add(new Karta("BabaHerc", "DAMA PIJE SAMA"));
            karte.Add(new Karta("BabaZelje", "DAMA PIJE SAMA"));
            karte.Add(new Karta("BabaBundeva", "DAMA PIJE SAMA"));
            karte.Add(new Karta("BabaZir", "DAMA PIJE SAMA"));
            karte.Add(new Karta("DeckoHerc", "POGODI BOJU"));
            karte.Add(new Karta("DeckoZelje", "POGODI BOJU"));
            karte.Add(new Karta("DeckoZir", "POGODI BOJU"));
            karte.Add(new Karta("DeckoBundeva", "POGODI BOJU"));
            karte.Add(new Karta("DesetkaHerc", "NIKAD NISAM"));
            karte.Add(new Karta("DesetkaZelje", "NIKAD NISAM"));
            karte.Add(new Karta("DesetkaBundeva", "NIKAD NISAM"));
            karte.Add(new Karta("DesetkaZir", "NIKAD NISAM"));
            karte.Add(new Karta("DevetkaHerc", "SVI PIJU"));
            karte.Add(new Karta("DevetkaZelje", "SVI PIJU"));
            karte.Add(new Karta("DevetkaBundeva", "SVI PIJU"));
            karte.Add(new Karta("DevetkaZir", "SVI PIJU"));
            karte.Add(new Karta("OsmicaHerc", "PIJE DESNO OD TEBE"));
            karte.Add(new Karta("OsmicaZelje", "PIJE DESNO OD TEBE"));
            karte.Add(new Karta("OsmicaBundeva", "PIJE DESNO OD TEBE"));
            karte.Add(new Karta("OsmicaZir", "PIJE DESNO OD TEBE"));
            karte.Add(new Karta("SedmicaHerc", "PIJE LIJEVO OD TEBE"));
            karte.Add(new Karta("SedmicaZelje", "PIJE LIJEVO OD TEBE"));
            karte.Add(new Karta("SedmicaBundeva", "PIJE LIJEVO OD TEBE"));
            karte.Add(new Karta("SedmicaZir", "PIJE LIJEVO OD TEBE"));
        }

        private async void slikaKarte_UpSwiped(object sender, SwipedEventArgs e)
        {
            try
            {
                karte.Clear();
                StvoriListu();
                OpisZadatkaLabel.Text = "Špil resetiran i promiješan";
                await PostaviPocetnuKartu();
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Prebrzo to radiš. Uspori s mijenjanjem karata.", 3000));
            }
        }

        private async Task PostaviPocetnuKartu()
        {
            try
            {
                await slikaKarte.TranslateTo(0, -15, 125, Easing.Linear);
                await slikaKarte.TranslateTo(0, 0, 125, Easing.Linear);
                slikaKarte.TranslationY = 0;
                slikaKarte.Source = karte.Find(x => x.Naziv == "PocetnaKarta").Naziv;
                BrKarteLabel.Text = $"{karte.Count - 1}";
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Prebrzo to radiš. Uspori s mijenjanjem karata.", 3000));
            }
        }

        private async void slikaKarte_RightSwiped(object sender, SwipedEventArgs e)
        {
            try
            {
                await PromjeniKartuIIgraca();
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayToastAsync("Prebrzo to radiš. Uspori s mijenjanjem karata.", 3000));
            }
        }

        private async Task<int> PromjeniKartuIIgraca()
        {
            if (karte.Count == 0)
            {
                OpisZadatkaLabel.Text = "Špil je prazan!";
                return await Task.FromResult(0);
            }

            if (brojac == ListaIgraca.Count - 1)
            {
                brojac = 0;
            }
            else
            {
                brojac++;
            }
            Broj_odigranih_karata++;
            karte.RemoveAll(x => x.Naziv == "PocetnaKarta");
            int r = rnd.Next(karte.Count);
            slikaKarte.Source = karte.ElementAt(r).Naziv;
            await slikaKarte.TranslateTo(-10, 0, 25);
            await slikaKarte.TranslateTo(10, 0, 25);
            await slikaKarte.TranslateTo(-5, 0, 25);
            await slikaKarte.TranslateTo(5, 0, 25);
            await slikaKarte.TranslateTo(-2.5, 0, 25);
            await slikaKarte.TranslateTo(2.5, 0, 25);
            slikaKarte.TranslationX = 0;
            OpisZadatkaLabel.Text = $"{trenutniIgrac.Ime}: " + karte.ElementAt(r).Naredba;

            if (OpisZadatkaLabel.Text.Contains("SVI PIJU"))
            {
                Vibration.Vibrate();
            }

            karte.RemoveAt(r);
            BrKarteLabel.Text = $"{karte.Count}";
            trenutniIgrac = ListaIgraca.ElementAt(brojac);
            return await Task.FromResult(0);
        }

        private void slikaKarte_DownSwiped(object sender, SwipedEventArgs e)
        {
            _ = Navigation.PopAsync(true);
        }

        private async Task UpisiNovuIgru()
        {
            StopWatch.Stop();
            BrojacSekundi = StopWatch.Elapsed.TotalSeconds.ToString("0");
            Igra novaIgra = new Igra()
            {
                Datum = DateTime.Now,
                PopisIgraca = StvoriListuIgraca(),
                BrOdigranihKarata = Broj_odigranih_karata,
                DuljinaIgre = BrojacSekundi,
                BrojIgraca = ListaIgraca.Count,
            };
            await App.IgraRepo.AddNewIgraAsync(novaIgra);
        }

        private string StvoriListuIgraca()
        {
            string igraci = "";
            foreach (Igrac igrac in ListaIgraca)
            {
                igraci += $"{igrac.Ime}, ";
            }
            igraci = igraci.Trim();
            return igraci.TrimEnd(',');
        }

        protected override bool OnBackButtonPressed()
        {
            UpisiNovuIgruAsync();
            _ = Navigation.PopAsync(false);
            _ = Navigation.PopAsync(true);
            return true;
        }

        private async void UpisiNovuIgruAsync()
        {
            await UpisiNovuIgru();
        }
    }
}