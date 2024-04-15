using MediatR;

namespace RentalManagement.Domain.Request
{
    public class MotorcycleRequest : IRequest<MotorcycleResponse>
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
    }
}
