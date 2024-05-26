using Domein.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Interfaces
{
    public interface ITuinCentrumRepository
    {
        bool HeeftKlant(Klant klant);
        bool HeeftProduct(Product product);
        bool HeeftOfferte(Offerte offerte);
        bool HeeftOfferte_Product(Offerte_Product offerte_product);
        void Schrijfklant(Klant klant);
        void SchrijfProduct(Product product);
        void schrijfOfferte(Offerte offerte);
        void SchrijfOfferte_Product(Offerte_Product offerte_product);
        List<Klant> LeesKlanten();
        List<Product> LeesProducten();
        List<Offerte> LeesOffertes();
        List<Offerte> LeesOfferteMetProducten();
        List<Offerte> LeesZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr);
        List<KlantStatistieken> LeesZoekKlantenOpStatistieken(string klantNr, string naam);
        List<Offerte> LeesZoekOfferteMetProductenOpStatistieken(string klantNr, string datum, string OffertNr);
        bool HeeftKlantOpId(int klantId);
        public int LeesAantalOffertes();
    }
}
