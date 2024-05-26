using Domein.Interfaces;
using Domein.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistentie_File
{
    public class FileProcessor : IFileProcessor
    {
        public List<Klant> Leesklanten(string filename)
        {
            List<Klant> klanten = new();
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                      
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            klanten.Add(new Klant(int.Parse(parts[0]), parts[1], parts[2]));
                        }
                        else
                        {
                            Console.WriteLine($"Ongeldige invoer in de regel: {line}");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het lezen van het bestand: {ex.Message}");
            }
            return klanten;
        }

        public List<Product> LeesProducten(string filename)
        {
            List<Product> producten = new List<Product>();

            try
            {

                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 5)
                        {
                          
                            producten.Add(new Product(int.Parse(parts[0]), parts[1], parts[2], double.Parse(parts[3]), parts[4]));
                        }
                        else
                        {
                            Console.WriteLine($"Ongeldige invoer in de regel: {line}");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het lezen van het bestand: {ex.Message}");
            }
            return producten;
        }

        public List<Offerte> LeesOffertes(string filename)
        {
            List<Offerte> offertes = new();
            try
            {

                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                       
                        string[] parts = line.Split('|');
                        if (parts.Length == 6)
                        {
                            offertes.Add(new Offerte(int.Parse(parts[0]), DateTime.Parse(parts[1]), int.Parse(parts[2]), bool.Parse(parts[3]), bool.Parse(parts[4]), int.Parse(parts[5])));
                        }
                        else
                        {
                            Console.WriteLine($"Ongeldige invoer in de regel: {line}");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het lezen van het bestand: {ex.Message}");
            }
            return offertes;
        }

        public List<Offerte_Product> LeesOfferte_Producten(string filename)
        {
            List<Offerte_Product> data = new();
            try
            {

                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                       
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            data.Add(new Offerte_Product(int.Parse(parts[0]), int.Parse(parts[1]),  int.Parse(parts[2])));
                        }
                        else
                        {
                            Console.WriteLine($"Ongeldige invoer in de regel: {line}");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het lezen van het bestand: {ex.Message}");
            }
            return data;
        }


    }
}
