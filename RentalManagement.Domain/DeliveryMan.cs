using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain
{
    public class DeliveryMan : User
    {
        public string NumberLicense { get; private set; }
        public TypeLicense TypeLicense { get; private set; }
        public string IdImageLicense { get; private set; }
        public string Cnpj { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public void SetIdImageLicense(string idImageLicense)
        {
            IdImageLicense = idImageLicense;
        }

        public class Builder
        {
            private readonly DeliveryMan _entity = new();

            public Builder SetId()
            {
                _entity.Id = Guid.NewGuid();
                _entity.CreatedAt = DateTimeOffset.Now;
                return this;
            }

            public Builder SetNumberLicense(string numberLicense)
            {
                _entity.NumberLicense = numberLicense;
                return this;
            }

            public Builder SetTypeLicense(TypeLicense typeLicense)
            {
                _entity.TypeLicense = typeLicense;
                return this;
            }

            public Builder SetImageLicense(string imageLicense)
            {
                _entity.IdImageLicense = imageLicense;
                return this;
            }

            public Builder SetName(string name)
            {
                _entity.Name = name;
                return this;
            }

            public Builder SetEmail(string email)
            {
                _entity.Email = email;
                return this;
            }

            public Builder SetDateOfBirth(DateTime date)
            {
                _entity.Birthdaty = date;
                return this;
            }

            public Builder SetCnpj(string cnpj)
            {
                _entity.Cnpj = cnpj;
                return this;
            }

            public DeliveryMan Build()
                  => _entity;
        }
    }
}
