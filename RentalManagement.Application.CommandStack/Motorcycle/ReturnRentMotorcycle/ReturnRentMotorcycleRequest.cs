using MediatR;
using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain.Request
{
    public class ReturnRentMotorcycleRequest : IRequest<ReturnRentMotorcycleResponse>
    {
        public required Guid IdRent { get; set; }
        public DateTimeOffset FinalDate { get; set; }
    }
}
