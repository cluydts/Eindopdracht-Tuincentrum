using Domein.Exceptions;
using Domein.Models;

namespace UnitTestDomein
{
    public class DomeinTest
    {
        Offerte offerte1 = new Offerte(100, new DateTime(22 / 05 / 2024), 5, true, false, 5);
        Offerte offerte2 = new Offerte(200, new DateTime(22 / 06 / 2024), 6, false, true, 10);
        Offerte offerte3 = new Offerte(300, new DateTime(22 / 07 / 2024), 7, false, false, 15);

        Product product1 = new Product(45, "Product 1", "wetenschappelijk prodcuct 1", 10.0, "beschrijving product 1");
        Product product2 = new Product(50, "Product 2", "wetenschappelijk prodcuct 2", 15.5, "beschrijving product 2");
        Product product3 = new Product(55, "Product 3", "wetenschappelijk prodcuct 3", 14.0, "beschrijving product 3");
        Product product4 = new Product(60, "Product 4", "wetenschappelijk prodcuct 4", 17.9, "beschrijving product 4");
        Product product5 = new Product(65, "Product 5", "wetenschappelijk prodcuct 5", 11.0, "beschrijving product 5");

        [Theory]
        [InlineData(100)]
        [InlineData(1)]
        public void Offerte_ZouAltijdEenklantNummerMoetenHebben_Valid(int klantnummer)
        {
            // Act
            offerte1.KlantNummer = klantnummer;

            // Assert
            Assert.Equal(klantnummer, offerte1.KlantNummer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Offerte_ZouAltijdEenklantNummerMoetenHebben_Invalid(int klantnummer)
        {
            var ex = Assert.Throws<DomeinException>(() => offerte1.KlantNummer = klantnummer);
            Assert.Equal("klantnummer", ex.Message);

        }

        [Fact]
        public void CalculateTotalPrice_ShouldReturnCorrectTotal()
        {
            // Arrange

            offerte1.VoegProductToe(product1, 2);
            offerte1.VoegProductToe(product2, 2);
            offerte1.VoegProductToe(product3, 1);


            // Act
            double totalPrice = offerte1.BerekenTotalePrijs();

            // Assert
            Assert.Equal(40.0, totalPrice);
        }

        [Fact]
        public void AddProduct_InvalidProduct_ShouldThrowException()
        {
            // Arrange
            

            // Act & Assert
            Assert.Throws<ArgumentException>(() => offerte1.VoegProductToe(product1, 7));
        }

        [Fact]
        public void RemoveProduct_ValidProduct_ShouldRemoveProduct()
        {
            offerte2.VoegProductToe(product4, 3);

            // Act
            offerte2.Dproductaantal.Remove(product5);

            // Assert
            Assert.DoesNotContain(product, offer.Products);
        }
    }
}