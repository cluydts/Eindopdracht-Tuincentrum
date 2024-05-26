using Domein.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Models
{
    public class Offerte
    {
        private int _id;
        private int _klantNummer;
        private int _producten;
        private Dictionary<Product, int> _Dproductaantal;


        public int? id { get { return _id; } set { if (value is null) throw new DomeinException("id"); _id = (int)value; } }
        public DateTime Datum { get; set; }
        public int KlantNummer { get { return _klantNummer; } set { if (value <= 0) throw new DomeinException("klantnummer"); _klantNummer = (int)value; } }
        public bool Afhaal { get; set; }
        public bool Aanleg { get; set; }
        public int Producten { get { return _producten; } set { if (value < 0) { throw new DomeinException("Producten"); } _producten = value; } }
        public Dictionary<Product, int> Dproductaantal = new Dictionary<Product, int>();

        public double prijs { get; set; }

        public Offerte(int id, DateTime datum, int klantNummer, bool afhaal, bool aanleg, int producten)
        {
            this.id = id;
            Datum = datum;
            KlantNummer = klantNummer;
            Afhaal = afhaal;
            Aanleg = aanleg;
            Producten = producten;

        }

        public double BerekenTotalePrijs()
        {
            double totalePrijs = 0;
            if (Dproductaantal.Count != 0)
            {

                foreach (var item in Dproductaantal)
                {
                    totalePrijs += item.Key.prijs * item.Value;
                }
                // Voeg eventuele korting toe
                totalePrijs -= BerekenKorting(totalePrijs);
                // Voeg eventuele leveringskosten toe
                if (!Afhaal)
                {
                    totalePrijs += BerekenLeveringskosten(totalePrijs);
                }

                if (Aanleg is true)
                {
                    totalePrijs += BerekenAanlegkosten(totalePrijs);
                }
               
            }
            return totalePrijs;
        }

        // Methode om korting te berekenen op basis van de totale prijs
        private double BerekenKorting(double totalePrijs)
        {
            if (totalePrijs > 5000)
            {
                return totalePrijs * 0.10;
            }
            else if (totalePrijs > 2000)
            {
                return totalePrijs * 0.05;
            }
            return 0;
        }

        // Methode om leveringskosten te berekenen op basis van de totale prijs
        private double BerekenLeveringskosten(double totalePrijs)
        {
            if (totalePrijs < 500)
            {
                return 100;
            }
            else if (totalePrijs < 1000)
            {
                return 50;
            }
            return 0;
        }

        // Methode om aanlegkosten te berekenen op basis van de totale prijs
        private double BerekenAanlegkosten(double totalePrijs)
        {
            if (totalePrijs > 5000)
            {
                return totalePrijs * 0.05;
            }
            else if (totalePrijs > 2000)
            {
                return totalePrijs * 0.10;
            }
            return totalePrijs * 0.15;
        }

        private bool IsTeveelProduct()
        {
            int aantal = 0;
            foreach (var item in Dproductaantal)
            {
                aantal++;

            }
            if (aantal > Producten)
            {
                return true;
            }
            return false;   
        }

        public void VoegProductToe(Product product, int aantal)
        {
            if (IsTeveelProduct())
            {
                throw new DomeinException("voegProductToe");
            }
            else
            {
                Dproductaantal.Add(product, aantal);
            }

        }
    }
}

