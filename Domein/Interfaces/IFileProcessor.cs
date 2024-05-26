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
        List<Klant> Leesklanten(string filename);
        List<Product> LeesProducten(string filename);
        List<Offerte> LeesOffertes(string filename);
        List<Offerte_Product> LeesOfferte_Producten(string filename);

    }
}
