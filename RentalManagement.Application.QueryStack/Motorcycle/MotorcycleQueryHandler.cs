using MediatR;
using Microsoft.Extensions.Logging;
using RentalManagement.Application.QueryStack.Helpers;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class MotorcycleQueryHandler : IRequestHandler<MotorcycleQuery, PagedList<MotorcycleReadModel>>
    {
        private readonly RentalDbContext _dbContext;
        private readonly ILogger<MotorcycleQueryHandler> _logger;

        public MotorcycleQueryHandler(RentalDbContext dbContext, ILogger<MotorcycleQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedList<MotorcycleReadModel>> Handle(MotorcycleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var queryResult = _dbContext.Motorcycle
               .Where(request.FilterExpression)
               .Select(entity => new MotorcycleReadModel
               {
                   Id = entity.Id,
                   Model = entity.Model,
                   DateRegister = entity.CreatedAt,
                   PlateNumber = entity.PlateNumber
               });

                _logger.LogInformation($"Query Motorcycle executed {request.Plate} {request.Id} {request.Model}");

                return await PagedList<MotorcycleReadModel>.CreateAsync(queryResult, request.PageNumber, request.PageSize, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Query Motorcycle Fail", ex.Message));
                throw;
            }
        }
    }
}
