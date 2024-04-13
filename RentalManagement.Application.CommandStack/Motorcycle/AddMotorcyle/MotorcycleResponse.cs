using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleResponse
    {

        public Guid Id { get; set; }
        public string Return { get; set; }
    }
}
