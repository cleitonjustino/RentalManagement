using MediatR;
using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain.Request
{
    public class RentMotorcycleRequest : IRequest<RentMotorcycleResponse>
    {
        public required Guid IdDeliveryMan { get; set; }
        public PaymentPlans PaymentPlan { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset FinalDate { get; set; }
        public DateTimeOffset ExpectedDate { get; set; }
    }
}
