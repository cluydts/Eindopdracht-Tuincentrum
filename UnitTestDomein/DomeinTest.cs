using Domein.Exceptions;
using Domein.Models;

namespace UnitTestDomein
{
    public class DomeinTest
    {
        //Offerte offerte1 = new Offerte(100, new DateTime(22 / 05 / 2024), 5, true, false, 5);
        //Offerte offerte2 = new Offerte(200, new DateTime(22 / 06 / 2024), 6, false, true, 10);
        //Offerte offerte3 = new Offerte(300, new DateTime(22 / 07 / 2024), 7, false, false, 15);

        Product product1 = new Product(45, "Product 1", "wetenschappelijk prodcuct 1", 10.0, "beschrijving product 1");
        Product product2 = new Product(50, "Product 2", "wetenschappelijk prodcuct 2", 15.5, "beschrijving product 2");
        Product product3 = new Product(55, "Product 3", "wetenschappelijk prodcuct 3", 14.0, "beschrijving product 3");
        Product product4 = new Product(60, "Product 4", "wetenschappelijk prodcuct 4", 17.9, "beschrijving product 4");
        Product product5 = new Product(65, "Product 5", "wetenschappelijk prodcuct 5", 11.0, "beschrijving product 5");



        [Theory]
        [InlineData(100, 5, 200, 2, 900)] // 5 * 100 + 2 * 200 + 50 (leveringskosten)
        [InlineData(150, 3, 200, 1, 700)] // 3 * 150 + 1 * 200 + 50 (leveringskosten)
        public void BerekenTotalePrijs_MetGeldigeProducten_GeeftCorrectTotaal(double prijs1, int aantal1, double prijs2, int aantal2, double expectedTotal)
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var offerte = new Offerte(1, DateTime.Now, klant, false, false, 0);
            var product1 = new Product(1, "Product1", "Prod1", prijs1, "Beschrijving1");
            var product2 = new Product(2, "Product2", "Prod2", prijs2, "Beschrijving2");
            offerte.VoegProductToe(product1, aantal1);
            offerte.VoegProductToe(product2, aantal2);

            // Act
            double result = offerte.BerekenTotalePrijs();

            // Assert
            Assert.Equal(expectedTotal, result);
        }

        [Theory]
        [InlineData(1500, 2, 2850)] // 2 * 1500 - 150 (korting) + 50 (leveringskosten)
        [InlineData(2500, 1, 2350)] // 1 * 2500 - 125 (korting) - 25 (leveringskosten)
        public void BerekenTotalePrijs_MetKorting_GeeftCorrectTotaal(double prijs, int aantal, double expectedTotal)
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var offerte = new Offerte(1, DateTime.Now, klant, false, false, 0);
            var product = new Product(1, "Product", "Prod", prijs, "Beschrijving");
            offerte.VoegProductToe(product, aantal);

            // Act
            double result = offerte.BerekenTotalePrijs();

            // Assert
            Assert.Equal(expectedTotal, result);
        }

        [Theory]
        [InlineData(3000, 2, 6900)] // 2 * 3000 - 600 (korting) + 50 (leveringskosten) + 900 (aanlegkosten)
        [InlineData(3500, 1, 3850)] // 1 * 3500 - 175 (korting) + 50 (leveringskosten) + 350 (aanlegkosten)
        public void BerekenTotalePrijs_MetAanleg_GeeftCorrectTotaal(double prijs, int aantal, double expectedTotal)
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var offerte = new Offerte(1, DateTime.Now, klant, false, true, 0);
            var product = new Product(1, "Product", "Prod", prijs, "Beschrijving");
            offerte.VoegProductToe(product, aantal);

            // Act
            double result = offerte.BerekenTotalePrijs();

            // Assert
            Assert.Equal(expectedTotal, result);
        }

        [Theory]
        [InlineData(100, 2)]
        [InlineData(200, 1)]
        public void VoegProductToe_MetGeldigProduct_VerhoogtProductCount(double prijs, int aantal)
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var offerte = new Offerte(1, DateTime.Now, klant, false, false, 0);
            var product = new Product(1, "Product", "Prod", prijs, "Beschrijving");

            // Act
            offerte.VoegProductToe(product, aantal);

            // Assert
            Assert.Equal(1, offerte.Producten);
            Assert.True(offerte.Dproductaantal.ContainsKey(product));
        }

        [Theory]
        [InlineData(100, 2)]
        [InlineData(200, 1)]
        public void VerwijderProduct_MetGeldigProduct_VerlaagtProductCount(double prijs, int aantal)
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var product = new Product(1, "Product", "Prod", prijs, "Beschrijving");
            var offerte = new Offerte(1, DateTime.Now, klant, false, false, 0);
            offerte.VoegProductToe(product, aantal);

            // Act
            offerte.VerwijderProduct(product);

            // Assert
            Assert.Equal(0, offerte.Producten);
            Assert.False(offerte.Dproductaantal.ContainsKey(product));
        }

        [Fact]
        public void BerekenTotalePrijs_ZonderProducten_GeeftNul()
        {
            // Arrange
            var klant = new Klant(1, "Jan", "Hoofdstraat 1");
            var offerte = new Offerte(1, DateTime.Now, klant, false, false, 0);

            // Act
            double result = offerte.BerekenTotalePrijs();

            // Assert
            Assert.Equal(0, result);
        }


    }
}