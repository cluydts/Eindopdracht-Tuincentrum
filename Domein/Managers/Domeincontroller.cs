using Domein.Exceptions;
using Domein.Interfaces;
using Domein.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Managers
{
    public class Domeincontroller
    {
        private IFileProcessor fileProcessor;
        private ITuinCentrumRepository tuinCentrumRepository;
        public Domeincontroller(IFileProcessor fileProcessor, ITuinCentrumRepository tuinCentrumRepository)
        {
            this.fileProcessor = fileProcessor;
            this.tuinCentrumRepository = tuinCentrumRepository;
        }

        public void UploadKlant(string filename)
        {
            List<Klant> klanten = fileProcessor.Leesklanten(filename);
            foreach (Klant k in klanten)
            {
                if (!tuinCentrumRepository.HeeftKlant(k))
                {
                    tuinCentrumRepository.Schrijfklant(k);
                }
            }
        }
        public void UploadOfferte(string filename)
        {
            List<Offerte> offertes = fileProcessor.LeesOffertes(filename);
            foreach (Offerte offerte in offertes)
            {
                if (!tuinCentrumRepository.HeeftOfferte(offerte))
                {
                    tuinCentrumRepository.schrijfOfferte(offerte);
                }
            }
        }
        public void UploadOfferte_product(string filename)
        {
            List<Offerte_Product> Ops = fileProcessor.LeesOfferte_Producten(filename);
            foreach (Offerte_Product oP in Ops)
            {
                if (!tuinCentrumRepository.HeeftOfferte_Product(oP))
                {
                    tuinCentrumRepository.SchrijfOfferte_Product(oP);
                }
            }
        }
        public void UploadProduct(string filename)
        {
            List<Product> producten = fileProcessor.LeesProducten(filename);
            foreach (Product product in producten)
            {
                if (!tuinCentrumRepository.HeeftProduct(product))
                {
                    tuinCentrumRepository.SchrijfProduct(product);
                }
            }
        }


        public List<Klant> GeefKlanten()
        {
            try
            {
                return tuinCentrumRepository.LeesKlanten(); ;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefKlanten", ex);
            }
        }
        public List<Product> GeefProducten()
        {
            try
            {
                return tuinCentrumRepository.LeesProducten();
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefProducten", ex);
            }
        }
        public List<Offerte> GeefOffertes()
        {
            try
            {
                return tuinCentrumRepository.LeesOfferteMetProducten();
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefOffertes", ex);
            }
        }

        public List<Offerte> GeeftZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr)
        {
            try
            {
                return tuinCentrumRepository.LeesZoekOfferteMetProductenOpStatistieken(klantNr, datum, OffertNr);
            }
            catch (Exception ex )
            {

                throw new DomeinException("GeeftZoekOfferteOpStatistieken", ex);
            }
        }

        public List<KlantStatistieken> GeefZoekKlantenOpStatistieken(string klantNr, string naam)
        {
            try
            {
                return tuinCentrumRepository.LeesZoekKlantenOpStatistieken(klantNr, naam);
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefZoekKlantenOpStatistieken", ex);
            }
        }

        public List<string> GeefProductNamen()
        {
            try
            {
                List<Product> producten = tuinCentrumRepository.LeesProducten();
              

                List<string> result = producten.Select(item => $"{item.Naam}, {item.Beschrijving}, {item.Id}, €{item.prijs}").OrderBy(item => item).ToList();
                
                return result;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefProductNamen", ex);
            }
        }

        public List<string> geefKlantenNamen()
        {
            try
            {
                List<Klant> klanten = tuinCentrumRepository.LeesKlanten();

                
                List<string> result = klanten.Select(klant => $"{klant.Naam} | {klant.id}").OrderBy(naam => naam).ToList();

                return result;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefKlantenNamen", ex);
            }
        }

        public void UploadAangemaaktOfferte(Offerte offerte)
        {
            if (!tuinCentrumRepository.HeeftOfferte(offerte))
            {
                tuinCentrumRepository.schrijfOfferte(offerte);
            }
        }

        public void UploadAangemaakteOfferte_Product(Offerte_Product offerteProduct)
        {
            if (!tuinCentrumRepository.HeeftOfferte_Product(offerteProduct))
            {
                tuinCentrumRepository.SchrijfOfferte_Product(offerteProduct);
            }
        }

        public bool HeeftOfferteAl(Offerte offerte)
        {
            return tuinCentrumRepository.HeeftOfferte(offerte);
        }

        public bool HeeftKlantOpId(int klantNr)
        {
            return tuinCentrumRepository.HeeftKlantOpId(klantNr);
        }

        public int GeefMeestRecenteOfferteId()
        {
            try
            {
                return tuinCentrumRepository.LeesAantalOffertes()+1;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefAantalOffertes", ex);
            }
        }
    }
}