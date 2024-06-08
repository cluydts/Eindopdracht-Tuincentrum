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
        bool HeeftOfferte_Product(string offerteNr, string productId, string aantalProducten);


        void Schrijfklanten(List<Klant> klanten);
        void SchrijfProducten(List<Product> producten);
        void schrijfOffertes(List<Offerte> offerte, List<string[]> OfferteProductenData);
        void SchrijfOfferte_Producten(List<string[]> OfferteProducten);


        List<Klant> LeesKlanten();
        List<Product> LeesProducten();
        List<Offerte> LeesOffertes();



        public List<Offerte> LeesZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr, string KlantNaam);
        List<KlantStatistieken> LeesZoekKlantenOpStatistieken(string klantNr, string naam);
        public List<Offerte> LeesZoekOfferteMetProductenOpStatistieken(string klantNr, string datum, string OffertNr, string KlantNaam);


        bool HeeftKlantOpId(int klantId);
        public int LeesAantalOffertes();

        public List<Product> leesZoekProductenOp(string naam);
        public List<Klant> leesZoekKlantOp(string naam);

        public void UpdateOfferte(Offerte offerte, int orginielOfferteId);

    }
}
