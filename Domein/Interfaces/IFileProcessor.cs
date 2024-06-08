using Domein.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Interfaces
{
    public interface IFileProcessor
    {
        Dictionary<int,Klant> Leesklanten(string filename);
        Dictionary<int, Product> LeesProducten(string filename);
        Dictionary<int, Offerte> LeesOffertes(string filename, Dictionary<int, Klant> klanten);
        List<string[]> LeesOfferte_Producten(string filename);

        (List<Klant>, List<Product>, List<Offerte>) LeesData(string filename);

    }
}
