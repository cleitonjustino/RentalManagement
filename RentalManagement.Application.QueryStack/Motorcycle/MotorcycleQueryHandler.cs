
using MediatR;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class MotorcycleQueryHandler : IRequestHandler<MotorcycleQuery, List<MotorcycleReadModel>>
    {
        private readonly RentalDbContext _dbContext;
        private readonly ILogger<MotorcycleQueryHandler> _logger;

        public MotorcycleQueryHandler(RentalDbContext dbContext, ILogger<MotorcycleQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<List<MotorcycleReadModel>> Handle(MotorcycleQuery request, CancellationToken cancellationToken)
            => _dbContext.Motorcycle
                 .Where(request.FilterExpression)
                 .Select(entity => new MotorcycleReadModel
                 {
                     Id = entity.Id,
                     Model = entity.Model,
                     DateRegister = entity.DateRegister
                 }).ToListAsync(cancellationToken);
    }
}
