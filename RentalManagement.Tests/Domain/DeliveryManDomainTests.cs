using RentalManagement.Domain.Enums;
using RentalManagement.Domain;
using Xunit;

namespace RentalManagement.Tests.Domain
{
    public class DeliveryManDomainTests
    {
        [Fact]
        public void SetIdImageLicense_Should_Set_IdImageLicense()
        {
            // Arrange
            var deliveryMan = new DeliveryMan.Builder()
                .SetId()
                .SetName("John Doe")
                .SetNumberLicense("ABC123")
                .SetTypeLicense(TypeLicense.A)
                .SetCnpj("123456789")
                .SetDateOfBirth(new DateTime(1990, 1, 1))
                .Build();

            // Act
            deliveryMan.SetIdImageLicense("image123");

            // Assert
            Assert.Equal("image123", deliveryMan.IdImageLicense);
        }

    }
}
