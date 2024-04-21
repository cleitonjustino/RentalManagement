using MediatR;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class RentMotoQueryHandler : IRequestHandler<RentMotoQuery, List<RentMotoReadModel>>
    {
        private readonly RentalDbContext _dbContext;
        private readonly ILogger<RentMotoQueryHandler> _logger;

        public RentMotoQueryHandler(RentalDbContext dbContext, ILogger<RentMotoQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<RentMotoReadModel>> Handle(RentMotoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<RentMotoReadModel>();
                var queryResult = _dbContext.RentMotorcycle
               .Where(request.FilterExpression);

                foreach (var entity in await queryResult.ToListAsync(cancellationToken))
                {
                    list.Add(new RentMotoReadModel
                    {
                        Id = entity.Id,
                        FineValue = entity.CalculateFine(request.FinalDate),
                        ExpectedDate = entity.ExpectedDate,
                        StartDate = entity.StartDate,   
                        Plate = entity.PlateNumber
                    });
                }

                _logger.LogInformation($"Query DeliveryMan executed {request.PlateNumber} ");

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Query DeliveryMan Fail", ex.Message));
                throw;
            }
        }
    }
}
