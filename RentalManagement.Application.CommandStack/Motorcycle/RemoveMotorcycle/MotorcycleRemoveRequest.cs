using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleRemoveRequest : IRequest<MotorcycleRemoveResponse>
    {
        public required Guid Id { get; set; }
    }
}
