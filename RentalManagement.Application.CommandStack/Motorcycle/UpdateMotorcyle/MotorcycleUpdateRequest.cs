using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleUpdateRequest : IRequest<MotorcycleUpdateResponse>
    {
        public required Guid Id { get; set; }
        public required string PlateNumber { get; set; }

    }
}
