using RentalManagement.Domain;
using Xunit;

namespace RentalManagement.Tests.Domain
{
    public class MotorcycleDomainTests
    {
        [Fact]
        public void ChangePlateNumber_Should_Set_New_PlateNumber()
        {
            // Arrange
            var motorcycle = new Motorcycle.Builder()
                .SetId()
                .SetYear(2020)
                .SetModel("Suzuki")
                .SetPlateNumber("ABC123")
                .Build();

            // Act
            motorcycle.ChangePlateNumber("XYZ789");

            // Assert
            Assert.Equal("XYZ789", motorcycle.PlateNumber);
        }

        [Fact]
        public void SetRented_Should_Set_Rented_To_True()
        {
            // Arrange
            var motorcycle = new Motorcycle.Builder()
                .SetId()
                .SetYear(2020)
                .SetModel("Suzuki")
                .SetPlateNumber("ABC123")
                .Build();

            // Act
            motorcycle.SetRented();

            // Assert
            Assert.True(motorcycle.Rented);
        }

        [Theory]
        [InlineData(2020)]
        [InlineData(2019)]
        [InlineData(1990)]
        public void Builder_SetYear_Should_Set_Year(int year)
        {
            // Arrange & Act
            var motorcycle = new Motorcycle.Builder()
                .SetId()
                .SetModel("Suzuki")
                .SetPlateNumber("ABC123")
                .SetYear(year)
                .Build();

            // Assert
            Assert.Equal(year, motorcycle.Year);
        }
    }
}
