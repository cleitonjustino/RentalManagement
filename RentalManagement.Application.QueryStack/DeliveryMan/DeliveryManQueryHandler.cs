using MediatR;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Application.QueryStack.Helpers;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class DeliveryManQueryHandler : IRequestHandler<DeliveryManQuery, PagedList<DeliveryManReadModel>>
    {
        private readonly RentalDbContext _dbContext;
        private readonly ILogger<DeliveryManQueryHandler> _logger;

        public DeliveryManQueryHandler(RentalDbContext dbContext, ILogger<DeliveryManQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedList<DeliveryManReadModel>> Handle(DeliveryManQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var queryResult = _dbContext.DeliveryMen
               .Where(request.FilterExpression)
               .Select(entity => new DeliveryManReadModel
               {
                   Id = entity.Id,
                   Name = entity.Name,
                   Cnpj = entity.Cnpj,
               });

                _logger.LogInformation($"Query DeliveryMan executed {request.Name} ");

                return await PagedList<DeliveryManReadModel>.CreateAsync(queryResult, request.PageNumber, request.PageSize, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Query DeliveryMan Fail", ex.Message));
                throw;
            }
        }
    }
}
