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

namespace TuincentrumUI
{
    /// <summary>
    /// Interaction logic for ZoekKlantenOpWindow.xaml
    /// </summary>
    public partial class ZoekKlantenOpWindow : Window
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
        ObservableCollection<Klant> AlleKlanten;
        public ZoekKlantenOpWindow()
        {
            InitializeComponent();
            tuinRepo = new TuinCentrumRepository(connectionstring);
            processor = new FileProcessor();
            dc = new Domeincontroller(processor, tuinRepo);
        }

        private void ZoekButton_Click(object sender, RoutedEventArgs e)
        {
            string klantNr = KlantnummerTextBox.Text;
            string naam = NaamTextBox.Text;

            StatistiekenKlantenDataGrid.ItemsSource = dc.GeefZoekKlantenOpStatistieken(klantNr, naam);
        }

        private void KlantnummerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNr = KlantnummerTextBox.Text;


            int caretPosition = KlantnummerTextBox.SelectionStart;
            bool isNumeriek = int.TryParse(klantNr, out int getal);


            if (!isNumeriek)
            {
                string numeriekeTekst = new string(klantNr.Where(char.IsDigit).ToArray());

                if (numeriekeTekst != klantNr)
                {
                    KlantnummerTextBox.Text = numeriekeTekst;
                    KlantnummerTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNr.Length - numeriekeTekst.Length));
                }
            }
            else
            {
                VoerMethodeUit();
            }
        }

        private void NaamTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNaam = NaamTextBox.Text;
            bool isNummeriek;

            int caretPosition = NaamTextBox.SelectionStart;
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
                    NaamTextBox.Text = letteriekeTekst;
                    NaamTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNaam.Length - letteriekeTekst.Length));
                }
            }
            else
            {
                VoerMethodeUit();
            }
        }

        private void VoerMethodeUit()
        {
            string klantNr = KlantnummerTextBox.Text;
            string naam = NaamTextBox.Text;

            StatistiekenKlantenDataGrid.ItemsSource = dc.GeefZoekKlantenOpStatistieken(klantNr, naam);
        }
    }
}
