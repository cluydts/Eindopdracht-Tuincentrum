using Domein.Interfaces;
using Domein.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Persistentie_SQL
{
    public class TuinCentrumRepository : ITuinCentrumRepository
    {
        private string connectionstring;
        public TuinCentrumRepository(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }

        public bool HeeftKlant(Klant klant)
        {
            string SQL = "SELECT count(*) from Klant WHERE id=@id";
            using (SqlConnection conn = new(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@id", klant.id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {

                    throw new Exception("HeeftKlant", ex);
                }
            }
        }
        public bool HeeftKlantOpId(int klantId)
        {
            string SQL = "SELECT count(*) from Klant WHERE id=@id";
            using (SqlConnection conn = new(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@id", klantId);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {

                    throw new Exception("HeeftKlant", ex);
                }
            }
        }
        public bool HeeftOfferte(Offerte offerte)
        {
            string SQL = "SELECT count(*) from Offerte WHERE id=@id";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@id", offerte.id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {

                    throw new Exception("HeeftOfferte", ex);
                }
            }
        }


        public bool HeeftOfferte_Product(string offerteNr, string productId, string aantalProducten)
        {
            string SQL = "SELECT count(*) from Offerte_Producten WHERE offerteId = @offerteId and productId = @productId and aantalExemplaren = @aantalExemplaren";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@offerteId", offerteNr);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@aantalExemplaren", aantalProducten);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {

                    throw new Exception("HeeftOfferte_Product", ex);
                }
            }
        }
        public bool HeeftProduct(Product product)
        {
            string SQL = "SELECT count(*) from Product WHERE id=@id";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@id", product.Id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {

                    throw new Exception("HeeftProduct", ex);
                }
            }
        }

        public List<Klant> LeesKlanten()
        {
            string SQL = "SELECT * from Klant";
            List<Klant> klanten = new List<Klant>();

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        klanten.Add(new Klant((int)reader["id"], (string)reader["naam"], (string)reader["adres"]));
                    }

                    return klanten;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesKlanten", ex);
                }

            }
        }
        public List<Product> LeesProducten()
        {
            string SQL = "SELECT * from Product WHERE NederlandseNaam <> ''";
            List<Product> Producten = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Producten.Add(new Product((int)reader["id"], (string)reader["nederlandseNaam"], (string)reader["wetenschappelijkeNaam"], (double)reader["prijs"], (string)reader["beschrijving"]));
                    }

                    return Producten;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesProducten", ex);
                }

            }
        }


        public void Schrijfklanten(List<Klant> klanten)
        {
            string SQL = "INSERT INTO Klant(id, naam, adres) VALUES(@id, @naam, @adres)";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Transaction = conn.BeginTransaction();

                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@naam", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", System.Data.SqlDbType.NVarChar));
                    foreach (Klant klant in klanten)
                    {
                        cmd.Parameters["@id"].Value = klant.id;
                        cmd.Parameters["@naam"].Value = klant.Naam;
                        cmd.Parameters["@adres"].Value = klant.adres;
                        cmd.ExecuteNonQuery();
                    }


                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfKlant", ex);
                }
            }
        }
        public void schrijfOffertes(List<Offerte> offertes, List<string[]> ProductOfferteData)
        {
            string SQL = "INSERT INTO Offerte(id, datum, klantNummer,afhaal,aanleg,aantalProducten) VALUES(@id, @datum, @klantNummer,@afhaal,@aanleg,@aantalProducten)";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Transaction = conn.BeginTransaction();

                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@datum", System.Data.SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@klantNummer", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@afhaal", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@aanleg", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@aantalProducten", System.Data.SqlDbType.Int));

                    foreach (var offerte in offertes)
                    {
                        cmd.Parameters["@id"].Value = offerte.id;
                        cmd.Parameters["@datum"].Value = offerte.Datum;
                        cmd.Parameters["@klantNummer"].Value = offerte.Klant.id;
                        cmd.Parameters["@afhaal"].Value = offerte.Afhaal;
                        cmd.Parameters["@aanleg"].Value = offerte.Aanleg;
                        cmd.Parameters["@aantalProducten"].Value = offerte.Producten;
                        cmd.ExecuteNonQuery();
                    }


                    cmd.CommandText = "INSERT INTO Offerte_Producten(offerteId, productId, aantalExemplaren) VALUES(@offerteId, @productId, @aantalExemplaren)";



                    cmd.Parameters.Add(new SqlParameter("@offerteId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@productId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@aantalExemplaren", System.Data.SqlDbType.Int));

                    foreach (string[] item in ProductOfferteData)
                    {
                        cmd.Parameters["@offerteId"].Value = int.Parse(item[0]);
                        cmd.Parameters["@productId"].Value = int.Parse(item[1]);
                        cmd.Parameters["@aantalExemplaren"].Value = int.Parse(item[2]);
                        cmd.ExecuteNonQuery();
                    }




                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfOfferte", ex);
                }
            }
        }
        public void SchrijfOfferte_Producten(List<string[]> data)
        {
            string SQL = "INSERT INTO Offerte_producten(offerteId, productId, aantalExemplaren) VALUES(@offerteId, @productId, @aantalExemplaren)";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;

                    cmd.Transaction = conn.BeginTransaction();

                    cmd.Parameters.Add(new SqlParameter("@offerteId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@productId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@aantalExemplaren", System.Data.SqlDbType.Int));

                    foreach (string[] item in data)
                    {
                        cmd.Parameters["@offerteId"].Value = int.Parse(item[0]);
                        cmd.Parameters["@productId"].Value = int.Parse(item[1]);
                        cmd.Parameters["@aantalExemplaren"].Value = int.Parse(item[2]);
                        cmd.ExecuteNonQuery();
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfOfferte_Product", ex);
                }
            }
        }
        public void SchrijfProducten(List<Product> producten)
        {
            string SQL = "INSERT INTO Product(id, nederlandseNaam, wetenschappelijkeNaam, prijs, beschrijving) VALUES(@id, @nederlandseNaam, @wetenschappelijkeNaam, @prijs, @beschrijving)";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;

                    cmd.Transaction = conn.BeginTransaction();

                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@nederlandseNaam", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@wetenschappelijkeNaam", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@prijs", System.Data.SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@beschrijving", System.Data.SqlDbType.NVarChar));

                    foreach (Product product in producten)
                    {
                        cmd.Parameters["@id"].Value = product.Id;
                        cmd.Parameters["@nederlandseNaam"].Value = product.Naam;
                        cmd.Parameters["@wetenschappelijkeNaam"].Value = product.WetenschappelijkeNaam;
                        cmd.Parameters["@prijs"].Value = product.prijs;
                        cmd.Parameters["@beschrijving"].Value = product.Beschrijving;
                        cmd.ExecuteNonQuery();
                    }



                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfProduct", ex);
                }
            }
        }


        public List<Offerte> LeesOffertes()
        {

            // SQL statement om alle offertes op te halen
            string sqlOffertes = @"select t1.id id, t1.datum datum, t1.klantNummer klantNummer, t2.naam klantnaam, t2.adres adres, t1.afhaal afhaal, t1.aanleg aanleg, t1.aantalProducten aantalproducten 
                                from Offerte t1 
                                join Klant t2 on t1.klantNummer = t2.id";

            // SQL statement om alle producten per offerte op te halen
            string sqlProducten = @"
        SELECT t1.id AS offerteid, t3.id AS productid, t3.nederlandseNaam AS productnaam, 
               t3.wetenschappelijkeNaam AS wetenschappelijkeNaam, t3.prijs AS prijs, 
               t3.beschrijving AS beschrijving, t2.aantalExemplaren AS aantal 
        FROM Offerte t1 
        JOIN Offerte_Producten t2 ON t1.id = t2.offerteId 
        JOIN Product t3 ON t2.productId = t3.id 
        ORDER BY offerteid";

            // Dictionary om offertes bij ID op te slaan
            Dictionary<int, Offerte> offertesDict = new Dictionary<int, Offerte>();

            // Eerst alle offertes ophalen
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlOffertes, conn, transaction))
                        {


                            using (IDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Offerte-object aanmaken en toevoegen aan de dictionary
                                    int id = (int)reader["id"];
                                    Offerte offerte = new Offerte(
                                        id,
                                        (DateTime)reader["datum"],
                                        new Klant((int)reader["klantnummer"], (string)reader["klantnaam"], (string)reader["adres"]),
                                        (bool)reader["afhaal"],
                                        (bool)reader["aanleg"],
                                        (int)reader["aantalproducten"]);

                                    offertesDict[id] = offerte;
                                }
                            }



                        }

                        // Vervolgens de producten per offerte ophalen

                        using (SqlCommand cmd = new SqlCommand(sqlProducten, conn, transaction))
                        {

                            conn.Open();
                            using (IDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Het juiste offerte-object vinden en product toevoegen
                                    int offerteId = (int)reader["offerteid"];
                                    if (offertesDict.TryGetValue(offerteId, out Offerte offerte))
                                    {
                                        Product product = new Product(
                                            (int)reader["productid"],
                                            (string)reader["productnaam"],
                                            (string)reader["wetenschappelijkeNaam"],
                                            (double)reader["prijs"],
                                            (string)reader["beschrijving"]);

                                        int aantal = (int)reader["aantal"];
                                        offerte.VoegProductToe(product, aantal);
                                    }
                                }
                            }

                            // De totale prijs voor elke offerte berekenen
                            foreach (var offerte in offertesDict.Values)
                            {
                                offerte.prijs = offerte.BerekenTotalePrijs();
                            }

                            transaction.Commit();

                            // Converteer de dictionary naar een lijst en retourneer deze
                            return offertesDict.Values.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("LeesOffertes", ex);
                    }
                }
            }

        }

        public List<Offerte> LeesZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr, string KlantNaam)
        {

            string SQL = @"select t1.id id, t2.naam klantNaam, t1.datum datum, t1.klantNummer klantNummer, t2.naam klantnaam, t2.adres adres, t1.afhaal afhaal, t1.aanleg aanleg, t1.aantalProducten aantalproducten 
                            from Offerte t1 
                            join Klant t2 on t1.klantNummer= t2.id";

            if (!string.IsNullOrEmpty(klantNr))
            {
                SQL += $" where t1.klantNummer LIKE @klantnummer";
            }
            if (!string.IsNullOrEmpty(OffertNr) && !string.IsNullOrEmpty(klantNr))
            {
                SQL += $" and t1.id LIKE @id";
            }
            else if (!string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))
            {
                SQL += $" where t1.id LIKE @id";
            }
            if (!string.IsNullOrEmpty(datum) && (!string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr)))
            {
                SQL += $" and t1.datum = @datum";
            }
            else if (!string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr)))
            {
                SQL += $" where t1.datum = @datum";
            }
            if ((!string.IsNullOrEmpty(KlantNaam) && (!string.IsNullOrEmpty(datum) || !string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr))))
            {
                SQL += $" and t2.naam LIKE @klantNaam";
            }
            else if (!string.IsNullOrEmpty(KlantNaam) && (string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))))
            {
                SQL += $" where t2.naam LIKE @klantNaam";

            }

            SQL += " order by id";

            List<Offerte> offertes = new List<Offerte>();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue(@"klantnummer", klantNr + '%');
                    cmd.Parameters.AddWithValue(@"id", OffertNr + '%');
                    cmd.Parameters.AddWithValue(@"datum", string.IsNullOrEmpty(datum) ? DBNull.Value : (object)datum);
                    cmd.Parameters.AddWithValue(@"klantNaam", '%' + KlantNaam + '%');


                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        offertes.Add(new Offerte((int)reader["id"], (DateTime)reader["datum"], new Klant((int)reader["klantnummer"], (string)reader["klantnaam"], (string)reader["adres"]), (bool)reader["afhaal"], (bool)reader["aanleg"], (int)reader["aantalproducten"]));
                    }

                    return offertes;

                }
                catch (Exception ex)
                {

                    throw new Exception("LeesZoekOfferteOpStatistieken", ex);
                }

            }
        }

        public List<Offerte> LeesZoekOfferteMetProductenOpStatistieken(string klantNr, string datum, string OffertNr, string KlantNaam)
        {
            string SQL = @"select t2.id offerteid, t1.naam klantNaam , t4.id productid,t4.nederlandseNaam productnaam,t4.wetenschappelijkeNaam wetenschappelijkeNaam,
                                  t4.prijs prijs, t4.beschrijving beschrijving, t3.aantalExemplaren aantal
                            from 
                            Klant t1
                            join Offerte t2 on t1.id=t2.klantNummer
                            join Offerte_Producten t3 on t2.id= t3.offerteId
                            join Product t4 on t3.productId=t4.id ";

            List<Offerte> offertes = LeesZoekOfferteOpStatistieken(klantNr, datum, OffertNr, KlantNaam);
            if (!string.IsNullOrEmpty(klantNr))
            {
                SQL += $"where t2.klantNummer LIKE @klantnummer ";

            }
            if (!string.IsNullOrEmpty(OffertNr) && !string.IsNullOrEmpty(klantNr))
            {
                SQL += $"and t2.id LIKE @id ";

            }
            else if (!string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))
            {
                SQL += $"where t2.id LIKE @id ";
            }
            if (!string.IsNullOrEmpty(datum) && (!string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr)))
            {
                SQL += $"and t2.datum = @datum ";
            }
            else if (!string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr)))
            {
                SQL += $"where t2.datum = @datum ";

            }
            if ( (!string.IsNullOrEmpty(KlantNaam) && (!string.IsNullOrEmpty(datum) || !string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr))))
            {
                SQL += $"and t1.naam LIKE @klantNaam ";
            }
            else if (!string.IsNullOrEmpty(KlantNaam) && (string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))))
            {
                SQL += $"where t1.naam LIKE @klantNaam ";

            }


            SQL += $"order by t2.id;"; ;
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue(@"klantnummer", klantNr + '%');
                    cmd.Parameters.AddWithValue(@"id", OffertNr + '%');
                    cmd.Parameters.AddWithValue(@"datum", string.IsNullOrEmpty(datum) ? DBNull.Value : (object)datum);
                    cmd.Parameters.AddWithValue(@"klantNaam", '%' + KlantNaam + '%');
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        foreach (Offerte offerte in offertes)
                        {
                            if (offerte.id == (int)reader["offerteid"])
                            {
                                offerte.VoegProductToe(new Product((int)reader["productid"], (string)reader["productNaam"], (string)reader["wetenschappelijkeNaam"], (double)reader["prijs"], (string)reader["beschrijving"]), (int)reader["aantal"]);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("LeesZoekOfferteMetProductenOpStatistieken", ex);
                }
            }
            foreach (Offerte offerte in offertes)
            {
                offerte.prijs = Math.Round(offerte.BerekenTotalePrijs(), 2);
            }
            return offertes;

        }
        public List<KlantStatistieken> LeesZoekKlantenOpStatistieken(string klantNr, string naam)
        {

            string SQL = $"SELECT Klant.Naam as Naam, Klant.Adres as Adres, Klant.id as KlantNummer, " +
                         "(SELECT COUNT(*) FROM Offerte " +
                         "WHERE Offerte.Klantnummer = Klant.id) AS AantalOffertes, " +
                         "Offerte.id AS OfferteNummer, " +
                         "ROUND(SUM(ISNULL(Offerte_Producten.aantalExemplaren, 0) * ISNULL(Product.Prijs, 0)),2) AS TotalePrijs " +
                         "FROM Klant " +
                         "LEFT JOIN Offerte ON Klant.id = Offerte.Klantnummer " +
                         "LEFT JOIN Offerte_Producten ON Offerte.id = Offerte_Producten.OfferteId " +
                         "LEFT JOIN Product ON Offerte_Producten.ProductId = Product.id ";
            if (string.IsNullOrEmpty(klantNr) && string.IsNullOrEmpty(naam))
            {

                SQL += "";
            }
            if (!string.IsNullOrEmpty(klantNr) && string.IsNullOrEmpty(naam))
            {
                SQL += $"WHERE Klant.id LIKE @id ";
            }
            else if (string.IsNullOrEmpty(klantNr) && !string.IsNullOrEmpty(naam))
            {
                SQL += $"WHERE Klant.Naam LIKE @naam ";

            }
            else if (!string.IsNullOrEmpty(klantNr) && !string.IsNullOrEmpty(naam))
            {
                SQL += $"WHERE Klant.Naam LIKE @naam and Klant.id LIKE @id ";
            }

            SQL += "GROUP BY Klant.Naam, Klant.Adres, Klant.id, Offerte.id " +
                "ORDER BY klantNummer , OfferteNummer";
            List<KlantStatistieken> KlantStatistieken = new List<KlantStatistieken>();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue(@"id", klantNr + '%');
                    cmd.Parameters.AddWithValue("@naam", "%" + naam + "%");

                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int offerteNummer = reader["OfferteNummer"] == DBNull.Value ? 0 : (int)reader["OfferteNummer"];

                        KlantStatistieken.Add(new KlantStatistieken((string)reader["Naam"], (string)reader["Adres"], (int)reader["Klantnummer"], (int)reader["AantalOffertes"], offerteNummer, Math.Round((double)reader["TotalePrijs"], 2)));
                    }
                    return KlantStatistieken;

                }
                catch (Exception ex)
                {

                    throw new Exception("LeesZoekKlantenOpStatistieken", ex);
                }

            }
        }

        public List<Product> leesZoekProductenOp(string naam)
        {
            string SQL = $"SELECT id, nederlandseNaam, wetenschappelijkeNaam, prijs, beschrijving from Product where nederlandseNaam LIKE @nederlandseNaam ";



            List<Product> producten = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;

                    cmd.Parameters.AddWithValue("@nederlandseNaam", "%" + naam + "%");

                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        producten.Add(new Product((int)reader["id"], (string)reader["nederlandseNaam"], (string)reader["wetenschappelijkeNaam"], (double)reader["prijs"], (string)reader["beschrijving"]));
                    }
                    return producten;

                }
                catch (Exception ex)
                {

                    throw new Exception("leesZoekProductenOp", ex);
                }

            }
        }
        public int LeesAantalOffertes()
        {
            string SQL = "SELECT COUNT(*) aantal from Offerte";
            int aantal = 0;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aantal = (int)reader["aantal"];


                    }

                    return aantal;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesAantalOffertes", ex);
                }
            }
        }
        public List<Klant> leesZoekKlantOp(string naam)
        {
            string SQL = $"select id, naam, adres from Klant  where naam LIKE @naam ";



            List<Klant> klanten = new List<Klant>();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;

                    cmd.Parameters.AddWithValue("@naam", "%" + naam + "%");

                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        klanten.Add(new Klant((int)reader["id"], (string)reader["naam"], (string)reader["adres"]));
                    }
                    return klanten;

                }
                catch (Exception ex)
                {

                    throw new Exception("leesZoekKlantOp", ex);
                }
            }
        }

        public void UpdateOfferte(Offerte offerte, int origineleOfferteId)
        {

            string updateSQL = @"UPDATE Offerte
                         SET id = @offerteId,
                             datum = @datum,
                             klantNummer = @klantNummer,
                             afhaal = @Afhaal,
                             aanleg = @Aanleg,
                             aantalProducten = @aantalProducten
                         WHERE id = @origineleOfferteId;";

            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();

                    // Verwijder bestaande producten
                    cmd.CommandText = "DELETE FROM Offerte_producten WHERE offerteId = @origineleOfferteId";
                    cmd.Parameters.Add(new SqlParameter("@origineleOfferteId", System.Data.SqlDbType.Int));
                    cmd.Parameters["@origineleOfferteId"].Value = origineleOfferteId;
                    cmd.ExecuteNonQuery();

                    // Update de offerte
                    cmd.CommandText = updateSQL;

                    cmd.Parameters.Add(new SqlParameter("@offerteId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@datum", System.Data.SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@klantNummer", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@afhaal", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@aanleg", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@aantalProducten", System.Data.SqlDbType.Int));

                    cmd.Parameters["@offerteId"].Value = offerte.id;
                    cmd.Parameters["@datum"].Value = offerte.Datum;
                    cmd.Parameters["@klantNummer"].Value = offerte.Klant.id;
                    cmd.Parameters["@afhaal"].Value = offerte.Afhaal;
                    cmd.Parameters["@aanleg"].Value = offerte.Aanleg;
                    cmd.Parameters["@aantalProducten"].Value = offerte.Producten;

                    cmd.ExecuteNonQuery();

                    // Voeg nieuwe producten toe
                    cmd.CommandText = "INSERT INTO Offerte_producten(offerteId, productId, aantalExemplaren) VALUES(@offerteId, @productId, @aantalExemplaren)";

                    cmd.Parameters.Add(new SqlParameter("@productId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@aantalExemplaren", System.Data.SqlDbType.Int));
                    foreach (KeyValuePair<Product, int> kvp in offerte.Dproductaantal)
                    {

                        cmd.Parameters["@productId"].Value = kvp.Key.Id;
                        cmd.Parameters["@aantalExemplaren"].Value = kvp.Value;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("UpdateOfferte", ex);
                }
            }
        }

    }
}


