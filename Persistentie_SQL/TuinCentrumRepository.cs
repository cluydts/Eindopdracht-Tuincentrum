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
        public bool HeeftOfferte_Product(Offerte_Product offerte_product)
        {
            string SQL = "SELECT count(*) from Offerte_Producten WHERE offerteId = @offerteId and productId = @productId and aantalExemplaren = @aantalExemplaren";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@offerteId", offerte_product.OfferteId);
                    cmd.Parameters.AddWithValue("@productId", offerte_product.ProductId);
                    cmd.Parameters.AddWithValue("@aantalExemplaren", offerte_product.aantalExemplaren);
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
            string SQL = "SELECT * from Product";
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
        public List<Offerte> LeesOffertes()
        {
            string SQL = "SELECT * from Offerte";
            List<Offerte> offertes = new List<Offerte>();

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
                        offertes.Add(new Offerte((int)reader["id"], (DateTime)reader["datum"], (int)reader["klantnummer"], (bool)reader["afhaal"], (bool)reader["aanleg"], (int)reader["aantalproducten"]));
                    }

                    return offertes;
                }
                catch (Exception ex)
                {

                    throw new Exception("Leesoffertes", ex);
                }

            }
        }


        public void Schrijfklant(Klant klant)
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
                    cmd.Parameters["@id"].Value = klant.id;
                    cmd.Parameters["@naam"].Value = klant.Naam;
                    cmd.Parameters["@adres"].Value = klant.adres;
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfKlant", ex);
                }
            }
        }
        public void schrijfOfferte(Offerte offerte)
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

                    cmd.Parameters["@id"].Value = offerte.id;
                    cmd.Parameters["@datum"].Value = offerte.Datum;
                    cmd.Parameters["@klantNummer"].Value = offerte.KlantNummer;
                    cmd.Parameters["@afhaal"].Value = offerte.Afhaal;
                    cmd.Parameters["@aanleg"].Value = offerte.Aanleg;
                    cmd.Parameters["@aantalProducten"].Value = offerte.Producten;

                    cmd.ExecuteNonQuery();


                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfOfferte", ex);
                }
            }
        }
        public void SchrijfOfferte_Product(Offerte_Product offerte_product)
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
                    cmd.Parameters["@offerteId"].Value = offerte_product.OfferteId;
                    cmd.Parameters["@productId"].Value = offerte_product.ProductId;
                    cmd.Parameters["@aantalExemplaren"].Value = offerte_product.aantalExemplaren;
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfOfferte_Product", ex);
                }
            }
        }
        public void SchrijfProduct(Product product)
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

                    cmd.Parameters["@id"].Value = product.Id;
                    cmd.Parameters["@nederlandseNaam"].Value = product.Naam;
                    cmd.Parameters["@wetenschappelijkeNaam"].Value = product.WetenschappelijkeNaam;
                    cmd.Parameters["@prijs"].Value = product.prijs;
                    cmd.Parameters["@beschrijving"].Value = product.Beschrijving;

                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfProduct", ex);
                }
            }
        }


        public List<Offerte> LeesOfferteMetProducten()
        {

            string SQL = "select t1.id offerteid, t3.id productid,t3.nederlandseNaam productnaam,t3.wetenschappelijkeNaam wetenschappelijkeNaam,t3.prijs prijs, t3.beschrijving beschrijving, t2.aantalExemplaren aantal from Offerte t1 join Offerte_Producten t2 on t1.id= t2.offerteId join Product t3 on t2.productId=t3.id order by offerteid;";
            List<Offerte> offertes = LeesOffertes();

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

                        foreach (Offerte offerte in offertes)
                        {
                            if (offerte.id == (int)reader["offerteid"])
                            {
                                offerte.VoegProductToe(new Product((int)reader["productid"], (string)reader["productNaam"], (string)reader["wetenschappelijkeNaam"], (double)reader["prijs"], (string)reader["beschrijving"]), (int)reader["aantal"]);
                            }
                            else
                            {
                                offerte.prijs = offerte.BerekenTotalePrijs();
                            }
                        }

                    }

                    return offertes;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesOfferteMetProducten", ex);
                }

            }
        }
        public List<Offerte> LeesZoekOfferteOpStatistieken(string klantNr, string datum, string OffertNr)
        {

            string SQL = $"select id, datum, klantNummer, afhaal, aanleg, aantalProducten from Offerte";

            if (!string.IsNullOrEmpty(klantNr))
            {
                SQL += $" where klantNummer LIKE @klantnummer";
            }
            if (!string.IsNullOrEmpty(OffertNr) && !string.IsNullOrEmpty(klantNr))
            {
                SQL += $" and id LIKE @id";
            }
            else if (!string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))
            {
                SQL += $" where id LIKE @id";
            }
            if (!string.IsNullOrEmpty(datum) && (!string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr)))
            {
                SQL += $" and datum = @datum";
            }
            else if (!string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr)))
            {
                SQL += $" where datum = @datum";
            }

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

                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        offertes.Add(new Offerte((int)reader["id"], (DateTime)reader["datum"], (int)reader["klantnummer"], (bool)reader["afhaal"], (bool)reader["aanleg"], (int)reader["aantalproducten"]));
                    }

                    return offertes;

                }
                catch (Exception ex)
                {

                    throw new Exception("LeesZoekOfferteOpStatistieken", ex);
                }

            }
        }

        public List<Offerte> LeesZoekOfferteMetProductenOpStatistieken(string klantNr, string datum, string OffertNr)
        {
            string SQL = $"select t1.id offerteid, t3.id productid,t3.nederlandseNaam productnaam,t3.wetenschappelijkeNaam wetenschappelijkeNaam,t3.prijs prijs, t3.beschrijving beschrijving, t2.aantalExemplaren aantal " +
                         $"from Offerte t1 " +
                         $"join Offerte_Producten t2 on t1.id= t2.offerteId " +
                         $"join Product t3 on t2.productId=t3.id ";

            List<Offerte> offertes = LeesZoekOfferteOpStatistieken(klantNr, datum, OffertNr);
            if (!string.IsNullOrEmpty(klantNr))
            {
                SQL += $"where t1.klantNummer LIKE @klantnummer ";

            }
            if (!string.IsNullOrEmpty(OffertNr) && !string.IsNullOrEmpty(klantNr))
            {
                SQL += $"and offerteid LIKE @id ";

            }
            else if (!string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr))
            {
                SQL += $"where offerteid LIKE @id ";
            }
            if (!string.IsNullOrEmpty(datum) && (!string.IsNullOrEmpty(OffertNr) || !string.IsNullOrEmpty(klantNr)))
            {
                SQL += $"and t1.datum = @datum ";
            }
            else if (!string.IsNullOrEmpty(datum) && (string.IsNullOrEmpty(OffertNr) && string.IsNullOrEmpty(klantNr)))
            {
                SQL += $"where t1.datum = @datum ";

            }

            SQL += $"order by offerteid;"; ;
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
    }
}
