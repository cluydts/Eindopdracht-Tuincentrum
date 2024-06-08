using Domein.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Models
{
    public class Klant
    {
        private int _id;
        private string _naam;
        private string _Adres;
        public int? id { get { return _id; }set { if (value is null) { throw new DomeinException("ongeldige input Id.");  } _id = (int)value; } }
        public string Naam { get { return _naam; } set {if (value is null) {throw new DomeinException("ongeldige input °naam.");} _naam = value; } }
        public string adres{ get { return _Adres; } set {if (value is null) {throw new DomeinException("ongeldiige input adres."); } _Adres = value; } }

        public Klant(int id, string naam, string adress)
        {
            this.id = id;
            Naam = naam;
            this.adres = adress;
        }

    
    }
}
