using Domein.Models;
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

namespace TuincentrumUI.PasOfferteAan
{
    /// <summary>
    /// Interaction logic for PasOfferteAanSelecteerAantalProductWindow.xaml
    /// </summary>
    public partial class PasOfferteAanSelecteerAantalProductWindow : Window
    {
        ObservableCollection<Product> products;
        PasOfferteAanWindow originalWindow;

        public PasOfferteAanSelecteerAantalProductWindow(Product product, PasOfferteAanWindow window)
        {
            InitializeComponent();
            products = [product];
            GeselecteerdeProductListBox.ItemsSource = products;

            originalWindow = window;


            List<int> aantal = new List<int>();
            int getal = 1;
            while (getal <= 999)
            {
                aantal.Add(getal);
                getal++;
            }
            AantalGeselecteerdeProductComboBox.ItemsSource = aantal;
        }

        private void VoegProductMetAantalToeButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = products[0];
            int aantal = (int)AantalGeselecteerdeProductComboBox.SelectedItem;

            originalWindow.UpdateSelectedProductsListBox(product, aantal);

            originalWindow.UpdatePrijs();

            originalWindow.Show();
            this.Close();

        }
    }
}
