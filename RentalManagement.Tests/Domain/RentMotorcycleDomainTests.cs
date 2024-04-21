using RentalManagement.Domain;
using RentalManagement.Domain.Enums;
using Xunit;

namespace RentalManagement.Tests.Domain
{
    public class RentMotorcycleDomainTests
    {
        [Fact]
        public void CalculateFine_ForLatePayment_Plan7Days()
        {
            // Arrange
            var sevenDaysWithOneDayLater = (int)PaymentPlans.Plan7Days * 30 + 50;

            var rentMotorcycle = new RentMotorcycle.Builder()
                .SetPaymentPlan(PaymentPlans.Plan7Days)
                .SetStartDate(DateTimeOffset.Now)
                .SetExpectedDate(DateTimeOffset.Now.AddDays(8))
                .Build();

            // Act
            var fine = rentMotorcycle.CalculateFine(DateTimeOffset.Now.AddDays(9));

            // Assert
            Assert.Equal(sevenDaysWithOneDayLater, fine); 
        }

        [Fact]
        public void CalculateFine_ForLatePayment_Plan15Days()
        {
            // Arrange
            var fifteenDaysWithOneDayLater = (int)PaymentPlans.Plan15Days * 28 + 50;

            var rentMotorcycle = new RentMotorcycle.Builder()
                .SetPaymentPlan(PaymentPlans.Plan15Days)
                .SetStartDate(DateTimeOffset.Now)
                .SetExpectedDate(DateTimeOffset.Now.AddDays(16))
                .Build();

            // Act
            var fine = rentMotorcycle.CalculateFine(DateTimeOffset.Now.AddDays(17));

            // Assert
            Assert.Equal(fifteenDaysWithOneDayLater, fine); 
        }

        [Fact]
        public void CalculateFine_ForExceedingExpectedDate()
        {
            // Arrange
            var thirteenDaysWithOneDayLater = (int)PaymentPlans.Plan30Days * 22 + 50;

            var rentMotorcycle = new RentMotorcycle.Builder()
                .SetPaymentPlan(PaymentPlans.Plan30Days)
                .SetStartDate(DateTimeOffset.Now)
                .SetExpectedDate(DateTimeOffset.Now.AddDays(30))
                .Build();

            // Act
            var fine = rentMotorcycle.CalculateFine(DateTimeOffset.Now.AddDays(32));

            // Assert
            Assert.Equal(thirteenDaysWithOneDayLater, fine); 
        }
    }
}
