﻿using Domein.Interfaces;
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
    /// Interaction logic for ZoekOffertesOpWindow.xaml
    /// </summary>
    /// 
   
    public partial class ZoekOffertesOpWindow : Window
    {
        List<Offerte> offertes = new List<Offerte>();
        string connectionstring = @"Data Source=Vaulter\SQLEXPRESS;
                                      Initial Catalog=TuinCentrum;
                                      Integrated Security=True;
                                      PersistSecurityInfo=True;
                                      Encrypt=True;
                                      TrustServerCertificate=True";
        IFileProcessor processor;
        ITuinCentrumRepository tuinRepo;
        Domeincontroller dc;
        public ZoekOffertesOpWindow()
        {
            InitializeComponent();
            tuinRepo = new TuinCentrumRepository(connectionstring);
            processor = new FileProcessor();
            dc = new Domeincontroller(processor, tuinRepo);

        }

    

        private void DatumDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            VoerMethodeUit();
        }

        private void VoerMethodeUit()
        {
            string klantNr = KlantNrTextBox.Text;
            string datum = DatumDatePicker.SelectedDate?.ToString("yyyy-MM-dd");
            string offerteid = OfferteTextBox.Text;
            string klantNaam = klantNaamTextBox.Text;

            offertes = dc.GeeftZoekOfferteOpStatistieken(klantNr, datum, offerteid, klantNaam);

            StatistiekenOffertesDataGrid.ItemsSource = offertes;

        }
        private void OfferteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


            string offerteid = OfferteTextBox.Text;
            int caretPosition = OfferteTextBox.SelectionStart;
            bool isNumeriek = int.TryParse(offerteid, out int getal);

            if (!isNumeriek)
            {
                string numeriekeTekst = new string(offerteid.Where(char.IsDigit).ToArray());

                if (numeriekeTekst != offerteid)
                {
                    OfferteTextBox.Text = numeriekeTekst;
                    OfferteTextBox.SelectionStart = Math.Max(0, caretPosition - (offerteid.Length - numeriekeTekst.Length));
                }
            }
            else
            {
                VoerMethodeUit();
            }
        }

        private void KlantNrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNr = KlantNrTextBox.Text;
            int caretPosition = KlantNrTextBox.SelectionStart;
            bool isNumeriek = int.TryParse(klantNr, out int getal);


            if (!isNumeriek)
            {
                string numeriekeTekst = new string(klantNr.Where(char.IsDigit).ToArray());

                if (numeriekeTekst != klantNr)
                {
                    KlantNrTextBox.Text = numeriekeTekst;
                    KlantNrTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNr.Length - numeriekeTekst.Length));
                }
            }
            else
            {
                VoerMethodeUit();
            }

        }

        private void StatistiekenOffertesDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var offerte = StatistiekenOffertesDataGrid.SelectedCells;
        }

        private void StatistiekenOffertesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Offerte offere = (Offerte)StatistiekenOffertesDataGrid.SelectedItem;

            PasOfferteAanWindow window = new PasOfferteAanWindow(offere);
            window.Show();
            this.Hide();
        }

        private void klantNaamTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string klantNaam = klantNaamTextBox.Text;
            bool isNummeriek;

            int caretPosition = klantNaamTextBox.SelectionStart;
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
                    klantNaamTextBox.Text = letteriekeTekst;
                    klantNaamTextBox.SelectionStart = Math.Max(0, caretPosition - (klantNaam.Length - letteriekeTekst.Length));
                }
            }
            else
            {
                VoerMethodeUit();
            }
        }
    }
}
