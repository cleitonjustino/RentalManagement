using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;

namespace RentalManagement.Domain
{
    public class Motorcycle
    {     
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string PlateNumber { get; private set; }
        public bool Rented { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset CreatedAt { get; private set; }

        public void ChangePlateNumber(string plate)
        {
            PlateNumber = plate;
        }

        public void SetRented()
        {
            Rented = true;
        }

        public class Builder
        {
            private readonly Motorcycle _entity = new();

            public Builder SetId()
            {
                _entity.Id = Guid.NewGuid();
                _entity.CreatedAt = DateTimeOffset.Now;
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

                _entity.Model = model.ToLower();
                return this;
            }

            public Builder SetPlateNumber(string plate)
            {
                if (string.IsNullOrWhiteSpace(plate) || plate.Length < LengthFields.Six)
                    throw new ValidationException(new ValidationItem
                    { Message = Messages.InvalidPlateNumber });

                _entity.PlateNumber = plate.ToUpper();
                return this;
            }

            public Motorcycle Build()
              => _entity;
        }
    }
}
