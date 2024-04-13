using MongoFramework;
using RentalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalManagement.Infrastructure
{
    public class RentalDbContext : MongoDbContext
    {
        public RentalDbContext(IMongoDbConnection connection) : base(connection)
        {
        }
        public MongoDbSet<Motorcycle> Motorcycle { get; init; }
        public MongoDbSet<User> User { get; init; }
    }

}
