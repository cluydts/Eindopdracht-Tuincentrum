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
using TuincentrumUI.PasOfferteAan;

namespace TuincentrumUI
{
    /// <summary>
    /// Interaction logic for PasOfferteAanWindow.xaml
    /// </summary>
    public partial class PasOfferteAanWindow : Window
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
      
        ObservableCollection<KeyValuePair<Product, int>> origineleProducten;
      
        int origineleOfferteId;
        Offerte offert;
        public PasOfferteAanWindow(Offerte offerte)
        {

            InitializeComponent();
            processor = new FileProcessor();
            tuinRepo = new TuinCentrumRepository(connectionstring);
            dc = new Domeincontroller(processor, tuinRepo);
          
            AlleProductenListBox.ItemsSource = dc.GeefProducten();

            offert = offerte;
            origineleOfferteId = (int)offerte.id;


            List<Klant> klanten = dc.GeefKlanten();
            KlantNummerComboBox.ItemsSource = klanten;

            OfferteNummerTextBox.Text = offerte.id.ToString();

            KlantNummerComboBox.SelectedItem = klanten.FirstOrDefault(k => k.id == offerte.Klant.id);

            DatumDatePicker.SelectedDate = offerte.Datum;
            AfhaalCheckBox.IsChecked = offerte.Afhaal;
            AanlegCheckBox.IsChecked = offerte.Aanleg;


            origineleProducten = new ObservableCollection<KeyValuePair<Product, int>>(offerte.Dproductaantal);
            OrigineleProductenListBox.ItemsSource = origineleProducten;
            UpdatePrijs();


        }
        private void VoegProductToeButton_Click(object sender, RoutedEventArgs e)
        {



            if (AlleProductenListBox.SelectedItem != null)
            {
                Product geselecteerdproduct = (Product)AlleProductenListBox.SelectedItem;

                PasOfferteAanSelecteerAantalProductWindow window = new PasOfferteAanSelecteerAantalProductWindow(geselecteerdproduct, this);
                window.Show();
                this.Hide();
            }


        }

        private void VerwijderAlleProductenButton_Click(object sender, RoutedEventArgs e)
        {
            origineleProducten.Clear();
            UpdatePrijs();
        }

        private void VerwijderProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrigineleProductenListBox.SelectedItem != null)
            {

                KeyValuePair<Product, int> selectedKvp = (KeyValuePair<Product, int>)OrigineleProductenListBox.SelectedItem;

                origineleProducten.Remove(selectedKvp);

                UpdatePrijs();
            }



            OrigineleProductenListBox.ItemsSource = origineleProducten;
        }

        public void UpdateSelectedProductsListBox(Product geselecteerdeProduct, int aantal)
        {


            bool isNietToegevoegd = true;

            for (int i = 0; i < origineleProducten.Count; i++)
            {

                if (origineleProducten[i].Key == geselecteerdeProduct)
                {
                    int nieuwAantal = origineleProducten[i].Value + aantal;

                    origineleProducten[i] = new KeyValuePair<Product, int>(geselecteerdeProduct, nieuwAantal);

                    isNietToegevoegd = false;
                    break;
                }

            }
            if (isNietToegevoegd)
            {
                origineleProducten.Add(new KeyValuePair<Product, int>(geselecteerdeProduct, aantal));
            }
            OrigineleProductenListBox.ItemsSource = null;
            OrigineleProductenListBox.ItemsSource = origineleProducten;

        }

        public void UpdatePrijs()
        {
            string[] parts;
            double totaal = OrigineleProductenListBox.Items.Count;

            int aantalproducten = 0;
            if (OrigineleProductenListBox.Items.Count == 0)
            {
                PrijsTextBlock.Text = $"€ {totaal}";
            }
            else
            {
                Offerte offerte = new Offerte(99999, DateTime.Now, new Klant(1, "naam", "adress"), AfhaalCheckBox.IsChecked.Value, AanlegCheckBox.IsChecked.Value, aantalproducten);

                foreach (KeyValuePair<Product, int> kvp in OrigineleProductenListBox.Items)
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

        private void PasOfferteAanButton_Click(object sender, RoutedEventArgs e)
        {
            Klant klant = (Klant)KlantNummerComboBox.SelectedItem;
            int aantalproducten = origineleProducten.Count();

            Dictionary<string, string> legeWaardes = new Dictionary<string, string>
            {
                { "OfferteNr", OfferteNummerTextBox.Text },
                { "Datum", DatumDatePicker.SelectedDate?.ToString("yyyy-MM-dd") },

                { "Afhaal", AfhaalCheckBox.IsChecked.Value.ToString() },
                { "Aanleg", AanlegCheckBox.IsChecked.Value.ToString() }
            };

            if (KlantNummerComboBox.SelectedItem is null)
            {
                legeWaardes.Add("KlantNr", "");
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
            else if (origineleProducten.Count == 0)
            {
                MessageBox.Show("Geen producten werden geselecteerd.");
            }
            else if (!dc.HeeftKlantOpId((int)klant.id))
            {
                MessageBox.Show("klantNummer bestaat niet.");
            }
            else
            {
                offert.id = int.Parse(OfferteNummerTextBox.Text);
                offert.Datum = DateTime.Parse(DatumDatePicker.SelectedDate?.ToString("yyyy-MM-dd"));
                offert.Klant = klant;
                offert.Afhaal = bool.Parse(AfhaalCheckBox.IsChecked.Value.ToString());
                offert.Aanleg = bool.Parse(AanlegCheckBox.IsChecked.Value.ToString());
                offert.Producten = aantalproducten;
                offert.Dproductaantal = origineleProducten.ToDictionary();

                if (origineleOfferteId != offert.id)
                {
                    if (dc.HeeftOfferteAl(offert))
                    {
                        MessageBox.Show($"offertID: {offert.id} bestaat al, kies een andere ID.");
                    }
                    else
                    {
                        dc.wijzigOfferte(offert, origineleOfferteId);
                        MessageBox.Show("Offerte succesvol gewijzigd.");
                        this.Close();
                    }
                }
                else
                {
                    dc.wijzigOfferte(offert, origineleOfferteId);
                    MessageBox.Show("Offerte succesvol gewijzigd.");
                    this.Close();
                }
            }
        }
    }
}

