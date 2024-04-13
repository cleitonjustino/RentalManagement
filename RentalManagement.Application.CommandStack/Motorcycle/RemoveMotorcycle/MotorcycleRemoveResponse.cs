using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleRemoveResponse
    {
        public Guid Id { get; set; }
        public string Return { get; set; }
    }
}
