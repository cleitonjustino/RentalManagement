using MediatR;
using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain.Request
{
    public class DeliveryManAddCnhRequest : IRequest<DeliveryManAddCnhResponse>
    {
        public Guid IdDeliveryMan { get; set; }
        public Stream File { get; set; }
        public string ContentType { get; set; }
    }
}
