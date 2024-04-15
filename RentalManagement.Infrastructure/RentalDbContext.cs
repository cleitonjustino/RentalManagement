using MongoFramework;
using RentalManagement.Domain;

namespace RentalManagement.Infrastructure
{
    public class RentalDbContext : MongoDbContext
    {
        public RentalDbContext(IMongoDbConnection connection) : base(connection)
        {
        }
        public MongoDbSet<Motorcycle> Motorcycle { get; init; }
        public MongoDbSet<User> User { get; init; }
        public MongoDbSet<DeliveryMan> DeliveryMen { get; init; }
        public MongoDbSet<RentMotorcycle> RentMotorcycle { get; init; }
    }
}
