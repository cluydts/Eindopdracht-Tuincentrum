using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Models
{
    public class Offerte_Product
    {
       
        public int OfferteId { get; set; }
        public int ProductId { get; set; }
        public int aantalExemplaren { get; set; }

        public Offerte_Product(int offerteId, int productId, int aantalExemplaren)
        {
            OfferteId = offerteId;
            ProductId = productId;
            this.aantalExemplaren = aantalExemplaren;
        }
    }
}
