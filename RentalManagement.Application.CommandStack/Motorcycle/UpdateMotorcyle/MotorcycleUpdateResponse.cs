using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleUpdateResponse
    {
        public Guid Id { get; set; }
        public string Return { get; set; }
    }
}
