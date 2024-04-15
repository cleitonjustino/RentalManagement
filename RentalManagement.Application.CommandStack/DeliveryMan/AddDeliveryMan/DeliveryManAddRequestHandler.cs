using MediatR;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class DeliveryManAddRequestHandler : IRequestHandler<DeliveryManAddRequest, DeliveryManAddResponse>
    {
        private readonly ILogger<DeliveryManAddRequestHandler> _logger;
        public RentalDbContext _dbContext;

        public DeliveryManAddRequestHandler(ILogger<DeliveryManAddRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<DeliveryManAddResponse> Handle(DeliveryManAddRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var deliveryMan = new Domain.DeliveryMan.Builder()
                   .SetId()
                   .SetImageLicense(string.Empty)
                   .SetDateOfBirth(request.Birthday)
                   .SetNumberLicense(request.NumberLicense)
                   .SetEmail(request.Email)
                   .SetName(request.Name)
                   .SetTypeLicense(request.TypeLicense)
                   .SetCnpj(request.Cnpj)
                   .Build();

                _dbContext.DeliveryMen.Add(deliveryMan);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(string.Format(Messages.SuccessSaveBike, deliveryMan.Id));

                return new DeliveryManAddResponse
                {
                    Return = "Sucess",
                    Id = deliveryMan.Id
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
