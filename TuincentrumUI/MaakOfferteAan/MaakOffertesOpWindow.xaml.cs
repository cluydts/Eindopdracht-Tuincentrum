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
       
        ObservableCollection<KeyValuePair<Product, int>> GeselecteerdeProducten;
       

        public MaakOffertesOpWindow()
        {
            InitializeComponent();
            processor = new FileProcessor();
            tuinRepo = new TuinCentrumRepository(connectionstring);
            dc = new Domeincontroller(processor, tuinRepo);
           
            AlleProductenListBox.ItemsSource = dc.GeefProducten();
            GeselecteerdeProducten = new ObservableCollection<KeyValuePair<Product, int>>();

            KlantNummerComboBox.ItemsSource = dc.GeefKlanten();
            DatumDatePicker.SelectedDate = DateTime.Now;
            OfferteNummerTextBox.Text = dc.GeefMeestRecenteOfferteId().ToString();


        }
        private void VoegProductToeButton_Click(object sender, RoutedEventArgs e)
        {



            if (AlleProductenListBox.SelectedItem != null)
            {
                Product geselecteerdproduct = (Product)AlleProductenListBox.SelectedItem;

                MaakOffertesOpSelecteerAantalProductWindow window = new MaakOffertesOpSelecteerAantalProductWindow(geselecteerdproduct, this);
                window.Show();
                this.Hide();
            }


        }

        private void VerwijderAlleProductenButton_Click(object sender, RoutedEventArgs e)
        {
            GeselecteerdeProducten.Clear();



            UpdatePrijs();
        }

        private void VerwijderProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (GeselecteerdeProductenListBox.SelectedItem != null)
            {

                KeyValuePair<Product, int> selectedKvp = (KeyValuePair<Product, int>)GeselecteerdeProductenListBox.SelectedItem;

                GeselecteerdeProducten.Remove(selectedKvp);

                UpdatePrijs();
            }



            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;
        }

        public void UpdateSelectedProductsListBox(Product geselecteerdeProduct, int aantal)
        {


            bool isNietToegevoegd = true;

            for (int i = 0; i < GeselecteerdeProducten.Count; i++)
            {

                if (GeselecteerdeProducten[i].Key == geselecteerdeProduct)
                {
                    int nieuwAantal = GeselecteerdeProducten[i].Value + aantal;

                    GeselecteerdeProducten[i] = new KeyValuePair<Product, int>(geselecteerdeProduct, nieuwAantal);

                    isNietToegevoegd = false;
                    break;
                }

            }
            if (isNietToegevoegd)
            {
                GeselecteerdeProducten.Add(new KeyValuePair<Product, int>(geselecteerdeProduct, aantal));
            }
            GeselecteerdeProductenListBox.ItemsSource = null;
            GeselecteerdeProductenListBox.ItemsSource = GeselecteerdeProducten;

        }

        public void UpdatePrijs()
        {
            string[] parts;
            double totaal = GeselecteerdeProductenListBox.Items.Count;

            int aantalproducten = 0;
            if (GeselecteerdeProductenListBox.Items.Count == 0)
            {
                PrijsTextBlock.Text = $"€ {totaal}";
            }
            else
            {
                Offerte offerte = new Offerte(99999, DateTime.Now, new Klant(1, "naam", "adress"), AfhaalCheckBox.IsChecked.Value, AanlegCheckBox.IsChecked.Value, aantalproducten);

                foreach (KeyValuePair<Product, int> kvp in GeselecteerdeProductenListBox.Items)
                {

                    double bedrag = kvp.Key.prijs * kvp.Value;
                    totaal += bedrag;


                    offerte.VoegProductToe(kvp.Key, kvp.Value);

                }
                offerte.Producten = aantalproducten;
                PrijsTextBlock.Text = $"€ {Math.Round(offerte.BerekenTotalePrijs(), 2)}";
            }
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


        private void AfhaalCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (AfhaalCheckBox.IsChecked is true)
            {
                AanlegCheckBox.IsChecked = false;
            }
            UpdatePrijs();
        }
        private void AanlegCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (AfhaalCheckBox.IsChecked is true)
            {

                AanlegCheckBox.IsChecked = false;

            }
            UpdatePrijs();

        }

        private void AfhaalCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            UpdatePrijs();

        }

        private void AanlegCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            UpdatePrijs();
        }

        private void OfferteNummerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNr = OfferteNummerTextBox.Text;


            int caretPosition = OfferteNummerTextBox.SelectionStart;
            bool isNumeriek = int.TryParse(klantNr, out int getal);


            if (!isNumeriek)
            {
                string numeriekeTekst = new string(klantNr.Where(char.IsDigit).ToArray());

                if (numeriekeTekst != klantNr)
                {
                    OfferteNummerTextBox.Text = numeriekeTekst;
                    OfferteNummerTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNr.Length - numeriekeTekst.Length));
                }
            }

        }

        private void ZoekProductTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNaam = ZoekProductTextBox.Text;
            bool isNummeriek;

            int caretPosition = ZoekProductTextBox.SelectionStart;
            if (caretPosition == 0)
            {
                isNummeriek = false;
            }
            else
            {
                string input = klantNaam[klantNaam.Length - 1].ToString();
                isNummeriek = int.TryParse(input, out int getal);
            }

            if (isNummeriek)
            {
                string letteriekeTekst = new string(klantNaam.Where(char.IsLetter).ToArray());

                if (letteriekeTekst != klantNaam)
                {
                    ZoekProductTextBox.Text = letteriekeTekst;
                    ZoekProductTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNaam.Length - letteriekeTekst.Length));
                }
            }
            else
            {

                AlleProductenListBox.ItemsSource = null;
                AlleProductenListBox.ItemsSource = dc.GeefZoekProductOp(klantNaam);

            }

        }

        private void KlantNummerComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;
            comboBox.IsDropDownOpen = true;
            KlantNummerComboBox.ItemsSource = null;
            KlantNummerComboBox.ItemsSource = dc.GeefZoekKlantOp(KlantNummerComboBox.Text);
        }
        private void MaakOfferteAanButton_Click(object sender, RoutedEventArgs e)
        {
            Klant klant = (Klant)KlantNummerComboBox.SelectedItem;
            List<string[]> offerteProductenData = new List<string[]>();

            int aantalproducten = GeselecteerdeProducten.Count();

            string OfferteNr = OfferteNummerTextBox.Text;
            string Datum = DatumDatePicker.SelectedDate?.ToString("yyyy-MM-dd");

            string Afhaal = AfhaalCheckBox.IsChecked.Value.ToString();
            string Aanleg = AanlegCheckBox.IsChecked.Value.ToString();

            Dictionary<string, string> legeWaardes = new Dictionary<string, string>
            {
                { "OfferteNr", OfferteNr },
                { "Datum", Datum },

                { "Afhaal", Afhaal },
                { "Aanleg", Aanleg }
            };

            if (klant is null)
            {
                legeWaardes.Add("KlantNr", null);
            }
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
            else if (GeselecteerdeProducten.Count == 0)
            {
                MessageBox.Show("Geen producten werden geselecteerd.");
            }
            else if (!dc.HeeftKlantOpId((int)klant.id))
            {
                MessageBox.Show("klantNummer bestaat niet.");
            }
            else
            {

                Offerte offerte = new(int.Parse(OfferteNr), DateTime.Parse(Datum), klant, bool.Parse(Aanleg), bool.Parse(Aanleg), aantalproducten);

                if (dc.HeeftOfferteAl(offerte))
                {
                    MessageBox.Show("OfferteNummer bestaat al.");
                }
                else
                {


                    foreach (KeyValuePair<Product, int> kvp in GeselecteerdeProducten)
                    {

                        offerteProductenData.Add([OfferteNr.ToString(), kvp.Key.Id.ToString(), kvp.Value.ToString()]);

                    }

                    dc.UploadAangemaaktOfferte(offerte, offerteProductenData);

                }
                MessageBox.Show("Offerte succesvol aangemaakt.");
                this.Close();
            }
        }
    }
}
