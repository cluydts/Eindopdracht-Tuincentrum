using Domein.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Models
{
    public class Product
    {
        private int _id;   
        private string _naam;
        private string _wetenschappelijkeNaam;
        private double _prijs;
        private string _beschrijving;
        public int? Id { get { return _id; }  set { if (value is null) throw new DomeinException("id"); _id = (int)value; } }
        public string Naam { get { return _naam; } set { if (value is null) throw new DomeinException("naam");  _naam = value; } }
        public string WetenschappelijkeNaam { get { return _wetenschappelijkeNaam; } set { if (value is null) throw new DomeinException("wetenschappelijke naam"); _wetenschappelijkeNaam = value; } }
        public double prijs { get { return _prijs; } set { if (value < 0) throw new DomeinException("prijs"); _prijs = value; } }
        public string Beschrijving { get { return _beschrijving; } set { if (value is null) throw new DomeinException("beschrijving"); _beschrijving = value; } }

        public Product(int id, string naam, string wetenschappelijkeNaam, double prijs, string beschrijving)
        {
            Id = id;
            Naam = naam;
            WetenschappelijkeNaam = wetenschappelijkeNaam;
            this.prijs = prijs;
            Beschrijving = beschrijving;
        }


    }
}
