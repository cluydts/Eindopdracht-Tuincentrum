using Domein.Exceptions;
using Domein.Interfaces;
using Domein.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domein.Managers
{
    public class Domeincontroller
    {
        private IFileProcessor fileProcessor;
        private ITuinCentrumRepository tuinRepo;
        public Domeincontroller(IFileProcessor fileProcessor, ITuinCentrumRepository tuinCentrumRepository)
        {
            this.fileProcessor = fileProcessor;
            this.tuinRepo = tuinCentrumRepository;
        }
        public void UploadData(string path)
        {
            (List<Klant>, List<Product>, List<Offerte>) data = fileProcessor.LeesData(path);

            if (true)
            {

            }
            tuinRepo.Schrijfklanten(data.Item1);
            tuinRepo.SchrijfProducten(data.Item2);
            tuinRepo.schrijfOffertes(data.Item3, fileProcessor.LeesOfferte_Producten(path + "\\offerte_producten.txt"));

        }

        public void UploadOffertes(string filename)
        {
            List<Offerte> offertes = fileProcessor.LeesData(filename).Item3;

            foreach (Offerte offerte in offertes)
            {
                if (!tuinRepo.HeeftOfferte(offerte))
                {
                    //tuinCentrumRepository.schrijfOfferte(offerte);
                }
            }
        }
        public void UploadOfferte_producten(string filename)
        {
            List<string[]> offertsProducten = fileProcessor.LeesOfferte_Producten(filename);

            tuinRepo.SchrijfOfferte_Producten(offertsProducten);

        }



        public List<Klant> GeefKlanten()
        {
            try
            {
                return tuinRepo.LeesKlanten().OrderBy(klant => klant.Naam).ToList();
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
                return tuinRepo.LeesProducten();
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
                return tuinRepo.LeesOffertes();
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefOffertes", ex);
            }
        }

        public List<Offerte> GeeftZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr, string klantNaam)
        {
            try
            {
                return tuinRepo.LeesZoekOfferteMetProductenOpStatistieken(klantNr, datum, OffertNr, klantNaam);
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeeftZoekOfferteOpStatistieken", ex);
            }
        }

        public List<KlantStatistieken> GeefZoekKlantenOpStatistieken(string klantNr, string naam)
        {
            try
            {
                return tuinRepo.LeesZoekKlantenOpStatistieken(klantNr, naam);
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
                List<Product> producten = tuinRepo.LeesProducten();


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
                List<Klant> klanten = tuinRepo.LeesKlanten();


                List<string> result = klanten.Select(klant => $"{klant.Naam} | {klant.id}").OrderBy(naam => naam).ToList();

                return result;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefKlantenNamen", ex);
            }
        }


        public void UploadAangemaaktOfferte(Offerte offerte, List<string[]> productOfferteData)
        {
            if (!tuinRepo.HeeftOfferte(offerte))
            {
                List<Offerte> offertes = new List<Offerte> { offerte };
                tuinRepo.schrijfOffertes(offertes, productOfferteData);
            }
        }

        public void UploadAangemaakteOfferte_Producten(List<string[]> data)
        {

            tuinRepo.SchrijfOfferte_Producten(data);

        }

        public bool HeeftOfferteAl(Offerte offerte)
        {
            return tuinRepo.HeeftOfferte(offerte);
        }

        public bool HeeftKlantOpId(int klantNr)
        {
            return tuinRepo.HeeftKlantOpId(klantNr);
        }

        public int GeefMeestRecenteOfferteId()
        {
            try
            {
                return tuinRepo.LeesAantalOffertes() + 1;
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefAantalOffertes", ex);
            }
        }

        public List<Product> GeefZoekProductOp(string naam)
        {
            try
            {
                return tuinRepo.leesZoekProductenOp(naam);
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefZoekProductOp", ex);
            }
        }

        public List<Klant> GeefZoekKlantOp(string naam)
        {
            try
            {
                return tuinRepo.leesZoekKlantOp(naam);
            }
            catch (Exception ex)
            {

                throw new DomeinException("GeefZoekKlantOp", ex);
            }
        }

        public void wijzigOfferte(Offerte offerte, int originaleOfferteId)
        {
            tuinRepo.UpdateOfferte(offerte, originaleOfferteId);

        }


    }
}