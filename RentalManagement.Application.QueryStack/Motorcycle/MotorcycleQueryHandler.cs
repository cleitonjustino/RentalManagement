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

        public async Task<List<MotorcycleReadModel>> Handle(MotorcycleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var queryResult = _dbContext.Motorcycle
               .Where(request.FilterExpression)
               .Select(entity => new MotorcycleReadModel
               {
                   Id = entity.Id,
                   Model = entity.Model,
                   DateRegister = entity.DateRegister,
                   PlateNumber = entity.PlateNumber
               })
               .Skip(request.PageSize * (request.PageNumber - 1))
               .Take(request.PageSize);

                _logger.LogInformation($"Query Motorcycle executed {request.Plate} {request.Id} {request.Model}");

                return await queryResult.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Query Motorcycle Fail", ex.Message));
                throw;
            }
       
        }
    }
}
