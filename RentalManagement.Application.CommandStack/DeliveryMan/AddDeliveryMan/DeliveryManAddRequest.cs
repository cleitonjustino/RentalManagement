using MediatR;
using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain.Request
{
    public class DeliveryManAddRequest : IRequest<DeliveryManAddResponse>
    {
        public string NumberLicense { get; set; }
        public TypeLicense TypeLicense { get; set; }
        public string ImageLicense { get; set; }
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
    }
}
