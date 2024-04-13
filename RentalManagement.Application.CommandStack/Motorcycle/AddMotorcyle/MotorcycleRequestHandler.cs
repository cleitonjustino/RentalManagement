using MediatR;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class MotorcycleRequestHandler : IRequestHandler<MotorcycleRequest, MotorcycleResponse>
    {
        private readonly ILogger<MotorcycleRequestHandler> _logger;
        public RentalDbContext _dbContext;

        public MotorcycleRequestHandler(ILogger<MotorcycleRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<MotorcycleResponse> Handle(MotorcycleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var motorcycle = new Motorcycle.Builder()
                   .SetId()
                   .SetPlateNumber(request.PlateNumber)
                   .SetModel(request.Model)
                   .SetYear(request.Year)
                   .Build();

                _dbContext.Motorcycle.Add(motorcycle);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(string.Format(Messages.SuccessSaveBike, motorcycle.Id));

                return new MotorcycleResponse
                {
                    Return = "Sucess",
                    Id = motorcycle.Id
                };             
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.FailedSaveBike,ex.Message));
                throw;
            }
        }
    }
}
