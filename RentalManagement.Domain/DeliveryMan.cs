using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain
{
    internal class DeliveryMan : User
    {
        public string NumberLicense { get; set; }
        public TypeLicenseEnum TypeLicense { get; set; }
        public string ImageLicense { get; set; }
        public string Cnpj { get; set; }
    }
}
