using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;

namespace RentalManagement.Domain
{
    public class Motorcycle
    {
        public Motorcycle() => DateRegister = DateTimeOffset.Now;

        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string PlateNumber { get; private set; }
        public DateTimeOffset DateRegister { get; private set; }

        public class Builder
        {
            private readonly Motorcycle _entity = new();

            public Builder SetId()
            {
                _entity.Id = Guid.NewGuid();
                return this;
            }

            public Builder SetYear(int year)
            {
                if (year <= LengthFields.Zero || year.ToString().Length < LengthFields.Four)
                    throw new ValidationException(new ValidationItem
                    { Message = Messages.InvalidYear });

                _entity.Year = year;
                return this;
            }

            public Builder SetModel(string model)
            {
                if (string.IsNullOrWhiteSpace(model))
                    throw new ValidationException(new ValidationItem
                    { Message = Messages.InvalidModel });

                _entity.Model = model;
                return this;
            }

            public Builder SetPlateNumber(string plate)
            {
                if (string.IsNullOrWhiteSpace(plate) || plate.Length < LengthFields.Six)
                    throw new ValidationException(new ValidationItem
                    { Message = Messages.InvalidPlateNumber });

                _entity.PlateNumber = plate;
                return this;
            }

            public Motorcycle Build()
              => _entity;
        }
    }
}
