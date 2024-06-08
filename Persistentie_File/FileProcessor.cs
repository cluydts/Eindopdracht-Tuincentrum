using Domein.Interfaces;
using Domein.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistentie_File
{
    public class FileProcessor : IFileProcessor
    {
        public Dictionary<int, Klant> Leesklanten(string filename)
        {
            Dictionary<int, Klant> klanten = new();
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
                            klanten.Add(int.Parse(parts[0]), new Klant(int.Parse(parts[0]), parts[1], parts[2]));
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

        public Dictionary<int, Product> LeesProducten(string filename)
        {
            Dictionary<int, Product> producten = new();

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

                            producten.Add(int.Parse(parts[0]), new Product(int.Parse(parts[0]), parts[1], parts[2], double.Parse(parts[3]), parts[4]));
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

        public Dictionary<int, Offerte> LeesOffertes(string filename, Dictionary<int, Klant> klanten)
        {
           

            Dictionary<int, Offerte> offertes = new();

            try
            {

                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        string[] parts = line.Split('|');
                        foreach (KeyValuePair<int, Klant> kvp in klanten)
                        {
                            if (kvp.Key == int.Parse(parts[2]))
                            {
                                offertes.Add(int.Parse(parts[0]), new Offerte(int.Parse(parts[0]), DateTime.Parse(parts[1]), kvp.Value, bool.Parse(parts[3]), bool.Parse(parts[4]), int.Parse(parts[5])));
                            }

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

        public List<string[]> LeesOfferte_Producten(string filename)
        {
            List<string[]> data = new();
            int index = 0;
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
                            data.Add(parts);
                            index++;
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
                Console.WriteLine("index: " + index);
            }
            return data;
        }

        public (List<Klant>, List<Product>, List<Offerte>) LeesData(string path)
        {
            Dictionary<int, Product> producten = LeesProducten(path + "\\producten.txt");

            Dictionary<int, Klant> klanten = Leesklanten(path + "\\klanten.txt");

            Dictionary<int, Offerte> offertes = LeesOffertes(path + "\\offertes.txt", klanten);

            List<string[]> productOffertes = LeesOfferte_Producten(path + "\\offerte_producten.txt");

            foreach (string[] OP in productOffertes)
            {
                int offertIdKey = int.Parse(OP[0]);
                Product product = producten[int.Parse(OP[1])];
                int aantal = int.Parse(OP[2]);

                offertes[offertIdKey].VoegProductToe(product, aantal);
            }

            return (klanten.Values.ToList(), producten.Values.ToList(), offertes.Values.ToList());
        }
    }
}
