using Domein.Interfaces;
using Domein.Managers;
using Domein.Models;
using Persistentie_File;
using Persistentie_SQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TuincentrumUI
{
    /// <summary>
    /// Interaction logic for MaakOffertesOpWindow.xaml
    /// </summary>
    public partial class MaakOffertesOpWindow : Window
    {

        string connectionstring = @"Data Source=Vaulter\SQLEXPRESS;
                                      Initial Catalog=TuinCentrum;
                                      Integrated Security=True;
                                      PersistSecurityInfo=True;
                                      Encrypt=True;
                                      TrustServerCertificate=True";
        IFileProcessor processor;
        ITuinCentrumRepository tuinRepo;
        Domeincontroller dc;
        ObservableCollection<string> alleProducten;
        List<string> GeselecteerdeProducten;
        List<string> Klanten;

        public MaakOffertesOpWindow()
        {
            InitializeComponent();
            processor = new FileProcessor();
            tuinRepo = new TuinCentrumRepository(connectionstring);
            dc = new Domeincontroller(processor, tuinRepo);
            alleProducten = new ObservableCollection<string>(dc.GeefProductNamen());
            AlleProductenListBox.ItemsSource = alleProducten;
            GeselecteerdeProducten = new List<string>();
            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
            Klanten = new List<string>(dc.geefKlantenNamen());
            KlantNummerComboBox.ItemsSource = Klanten;
            DatumDatePicker.SelectedDate = DateTime.Now;
            OfferteNummerTextBox.Text = dc.GeefMeestRecenteOfferteId().ToString();


        }
        private void VoegProductToeButton_Click(object sender, RoutedEventArgs e)
        {
            string[] parts;

            string product = "";
            if (AlleProductenListBox.SelectedItem != null)
            {
                string[] geselecteerdproduct = AlleProductenListBox.SelectedItem.ToString().Split(',');
                product += AlleProductenListBox.SelectedItem.ToString();
                MaakOffertesOpSelecteerAantalProductWindow window = new MaakOffertesOpSelecteerAantalProductWindow(product, this);
                window.Show();
                this.Hide();
            }


        }

        private void VerwijderAlleProductenButton_Click(object sender, RoutedEventArgs e)
        {
            GeselecteerdeProducten.Clear();
            GeselecteerdeProductenListBox.ItemsSource = null;
            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
            UpdatePrijs();
        }

        private void VerwijderProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (GeselecteerdeProductenListBox.SelectedItem != null)
            {
                string product = GeselecteerdeProductenListBox.SelectedItem.ToString();
                GeselecteerdeProducten.Remove(product);
                GeselecteerdeProductenListBox.ItemsSource = null;
                GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
                UpdatePrijs();
            }



            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
        }

        public void UpdateSelectedProductsListBox(string ProductMetNaam)
        {
            string[] partsItem;
            string[] GeselectProduct = ProductMetNaam.Split('|');
            int index = 0;
            bool isNietToegevoegd = true;
            foreach (string item in GeselecteerdeProducten)
            {
                index = GeselecteerdeProducten.IndexOf(item);
                partsItem = item.Split('|');
                int huidigeAantal = int.Parse(partsItem[1]);
                int toegevoegdAantal = int.Parse(GeselectProduct[1]);
                if (GeselectProduct[0].Trim() == partsItem[0].Trim())
                {
                    huidigeAantal += toegevoegdAantal;
                    string geupdateProductMetNaam = $"{partsItem[0]}| {huidigeAantal.ToString()}";
                    GeselecteerdeProducten[index] = geupdateProductMetNaam;
                    isNietToegevoegd = false;
                    break;
                }

            }
            if (isNietToegevoegd)
            {
                GeselecteerdeProducten.Add(ProductMetNaam);
            }
            GeselecteerdeProductenListBox.ItemsSource = null;
            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
        }

        public void UpdatePrijs()
        {
            string[] parts;
            int totaal = 0;

            int aantalproducten = 0;
            Offerte offerte = new Offerte(99999, DateTime.Now, 1, AfhaalCheckBox.IsChecked.Value, AanlegCheckBox.IsChecked.Value, aantalproducten);

            foreach (string item in GeselecteerdeProductenListBox.Items)
            {
                if (GeselecteerdeProductenListBox.Items.Count == 0)
                {
                    PrijsTextBlock.Text = $"€ {totaal}";
                }

                parts = item.Split(',', '|');
                int prijs = int.Parse(parts[3].Replace('€', ' ').Trim());
                int aantal = int.Parse(parts[4].Trim());
                aantalproducten += aantal;
                int bedrag = prijs * aantal;
                totaal += bedrag;
                Product product = new Product(int.Parse(parts[2].Trim()), "naam", "wetenschappelijke naam", prijs, "Beschrijving");
                offerte.Dproductaantal.Add(product, aantal);
            }

            offerte.Producten = aantalproducten;
            PrijsTextBlock.Text = $"€ {offerte.BerekenTotalePrijs()}";


        }

        private bool ProductBestaatAl()
        {
            string[] parts;
            string[] geselecteerdproduct = AlleProductenListBox.SelectedItem.ToString().Split(',');

            foreach (string item in GeselecteerdeProducten)
            {
                parts = item.Split(',');
                if (parts[0] == geselecteerdproduct[0])
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsLeeg(Dictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                if (item.Value == "" || item.Value is null)
                {
                    return true;
                }
            }
            return false;
        }

        private void MaakOfferteAanButton_Click(object sender, RoutedEventArgs e)
        {
            string[] parts;
            string klantnr;
            if (KlantNummerComboBox.SelectedItem is null)
            {
                klantnr = "";
            }
            else
            {
                parts = KlantNummerComboBox.SelectedItem.ToString().Split("|");
                klantnr = parts[1].Trim();
            }


            string OfferteNr = OfferteNummerTextBox.Text;
            string Datum = DatumDatePicker.SelectedDate?.ToString("yyyy-MM-dd");

            string Afhaal = AfhaalCheckBox.IsChecked.Value.ToString();
            string Aanleg = AanlegCheckBox.IsChecked.Value.ToString();

            Dictionary<string, string> legeWaardes = new Dictionary<string, string>();
            legeWaardes.Add("OfferteNr", OfferteNr);
            legeWaardes.Add("Datum", Datum);
            legeWaardes.Add("KlantNr", klantnr);
            legeWaardes.Add("Afhaal", Afhaal);
            legeWaardes.Add("Aanleg", Aanleg);


            int Result;
            if (IsLeeg(legeWaardes))
            {
                string legewaarden = "";
                foreach (var item in legeWaardes)
                {
                    if (item.Value == "" || item.Value is null)
                    {
                        legewaarden += $"- {item.Key}\n";
                    }

                }
                MessageBox.Show($"\nEr ontbreek volgende informatie: \n{legewaarden}");
            }
            else if (!int.TryParse(OfferteNr, out Result) || !int.TryParse(klantnr, out Result))
            {
                MessageBox.Show("OfferteNr en Klantnummer vereisen numerieke waardes.");
            }
            else if (GeselecteerdeProducten.Count == 0)
            {
                MessageBox.Show("Geen producten werden geselecteerd.");
            }
            else if (!dc.HeeftKlantOpId(int.Parse(klantnr)))
            {
                MessageBox.Show("klantNummer bestaat niet.");
            }
            else
            {
                int aantalProducten = 0;


                foreach (string item in GeselecteerdeProducten)
                {
                    parts = item.Split('|');
                    aantalProducten += int.Parse(parts[1].Trim());

                }


                Offerte offerte = new(int.Parse(OfferteNr), DateTime.Parse(Datum), int.Parse(klantnr), bool.Parse(Afhaal), bool.Parse(Aanleg), aantalProducten);

                if (dc.HeeftOfferteAl(offerte))
                {
                    MessageBox.Show("OfferteNummer bestaat al.");
                }
                else
                {
                    dc.UploadAangemaaktOfferte(offerte);

                    foreach (string item in GeselecteerdeProducten)
                    {
                        parts = item.Split(new char[] { ',', '|' });
                        int productId = int.Parse(parts[2].Trim());
                        int offerteNr = int.Parse(OfferteNr);
                        
                        aantalProducten = 0;
                        aantalProducten = int.Parse(parts[4].Trim());
                        Offerte_Product O_P = new Offerte_Product(offerteNr, productId, aantalProducten);
                        dc.UploadAangemaakteOfferte_Product(O_P);
                    }
                }
                MessageBox.Show("Offerte succesvol aangemaakt.");
                this.Close();
            }
        }

        private void AanlegCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (AfhaalCheckBox.IsChecked is true)
            {
                
                AanlegCheckBox.IsChecked = false;
            }
            
        }

        private void AfhaalCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(AfhaalCheckBox.IsChecked is true)
            {
                AanlegCheckBox.IsChecked = false;
            }
        }
    }
}
