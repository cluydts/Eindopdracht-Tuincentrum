using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Models
{
    public class KlantStatistieken
    {
        public string Naam {  get; set; }
        public string Adres { get; set;}
        public int id { get; set; }
        public int AantalOffertes { get; set; }
        public int OfferteNummer { get; set; }
        public double TotalePrijs { get; set; }

        public KlantStatistieken(string naam, string adres, int id, int aantalOffertes, int offerteNummer, double totalePrijs)
        {
            Naam = naam;
            Adres = adres;
            this.id = id;
            AantalOffertes = aantalOffertes;
            OfferteNummer = offerteNummer;
            TotalePrijs = totalePrijs;
        }
    }
}
