using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TuincentrumUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MaakOfferteOpButton_Click(object sender, RoutedEventArgs e)
        {
            MaakOffertesOpWindow window = new MaakOffertesOpWindow();
            window.Show();
        }

        private void ZoekOffertesOpButton_Click(object sender, RoutedEventArgs e)
        {
            ZoekOffertesOpWindow window = new ZoekOffertesOpWindow();
            window.Show();
        }

        private void ZoekKlantenOpButton_Click(object sender, RoutedEventArgs e)
        {
            ZoekKlantenOpWindow window = new ZoekKlantenOpWindow();
            window.Show();
        }
    }
}