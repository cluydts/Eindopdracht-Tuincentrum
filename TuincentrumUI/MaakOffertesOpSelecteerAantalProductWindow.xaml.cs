using System;
using System.Collections.Generic;
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
    /// Interaction logic for MaakOffertesOpSelecteerAantalProductWindow.xaml
    /// </summary>
    public partial class MaakOffertesOpSelecteerAantalProductWindow : Window
    {
        private MaakOffertesOpWindow originalWindow;
        public MaakOffertesOpSelecteerAantalProductWindow(string product, MaakOffertesOpWindow window)
        {
            InitializeComponent();
            GeselecteerdeProductTextBox.Text = product;
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

            int selectedAmount = (int)AantalGeselecteerdeProductComboBox.SelectedItem;

            string geselecteerdeProductMetAantal = "";

            geselecteerdeProductMetAantal = GeselecteerdeProductTextBox.Text + " | " + selectedAmount.ToString();
            originalWindow.UpdateSelectedProductsListBox(geselecteerdeProductMetAantal);

            originalWindow.UpdatePrijs();

            originalWindow.Show();
            this.Close();

        }
    }
}
