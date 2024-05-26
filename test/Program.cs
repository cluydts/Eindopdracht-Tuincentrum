using Domein.Interfaces;
using Domein.Managers;
using Domein.Models;
using Persistentie_File;
using Persistentie_SQL;
using System.Security.Cryptography.X509Certificates;

namespace test
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = @"Data Source=Vaulter\SQLEXPRESS;
                                      Initial Catalog=TuinCentrum;
                                      Integrated Security=True;
                                      PersistSecurityInfo=True;
                                      Encrypt=True;
                                      TrustServerCertificate=True";

            //string klantenFile = @"C:\Users\jeroe\Documents\Documents\ProgrammerenGevorderd\programmeren gevorderd 2\Eindopdracht\tuin\klanten.txt";
            //string offerteFile = @"C:\Users\jeroe\Documents\Documents\ProgrammerenGevorderd\programmeren gevorderd 2\Eindopdracht\tuin\offertes.txt";
            //string productenFile = @"C:\Users\jeroe\Documents\Documents\ProgrammerenGevorderd\programmeren gevorderd 2\Eindopdracht\tuin\producten.txt";
            //string offerte_productenFile = @"C:\Users\jeroe\Documents\Documents\ProgrammerenGevorderd\programmeren gevorderd 2\Eindopdracht\tuin\offerte_producten.txt";

            IFileProcessor processor = new FileProcessor();
            ITuinCentrumRepository tuinRepo = new TuinCentrumRepository(connectionstring);

            Domeincontroller dc = new Domeincontroller(processor, tuinRepo);

            //dc.UploadKlant(klantenFile);
            //dc.UploadOfferte(offerteFile);
            //dc.UploadProduct(productenFile);
            //dc.UploadOfferte_product(offerte_productenFile);
            //List<Offerte> offertes = dc.GeefOffertes();

            Offerte offerte1 = new Offerte(100, new DateTime(22 / 05 / 2024), 5, true, false, 5);

            Product product1 = new Product(45, "Product 1", "wetenschappelijk prodcuct 1", 10.0, "beschrijving product 1");
            Product product2 = new Product(50, "Product 2", "wetenschappelijk prodcuct 2", 15.5, "beschrijving product 2");
            Product product3 = new Product(55, "Product 3", "wetenschappelijk prodcuct 3", 14.0, "beschrijving product 3");

            offerte1.VoegProductToe(product1, 2);
            offerte1.VoegProductToe(product2, 2);
            offerte1.VoegProductToe(product3, 1);

            double prijs = offerte1.BerekenTotalePrijs();
            Console.WriteLine(prijs);
        }
    }
}
