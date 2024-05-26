using Domein.Interfaces;
using Domein.Managers;
using Domein.Models;
using Persistentie_File;
using Persistentie_SQL;

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
        }
    }
}
