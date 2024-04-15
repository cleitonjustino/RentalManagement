using MediatR;
using MongoFramework.Linq;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;
using RentalManagement.Domain.Enums;
using RentalManagement.Domain;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class RentMotorcycleRequestHandler : IRequestHandler<RentMotorcycleRequest, RentMotorcycleResponse>
    {
        private readonly ILogger<RentMotorcycleRequestHandler> _logger;
        private readonly RentalDbContext _dbContext;

        public RentMotorcycleRequestHandler(ILogger<RentMotorcycleRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<RentMotorcycleResponse> Handle(RentMotorcycleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var moto = await TakeMotoAvailableAsync(cancellationToken);

                await CheckLicenseIsAllowed();

                var rentMoto = new RentMotorcycle.Builder()
                    .PlateNumber(moto.PlateNumber)
                    .SetStartDate(request.StartDate)
                    .SetExpectedDate(request.ExpectedDate)
                    .SetPaymentPlan(request.PaymentPlan)
                    .Build();

                _dbContext.RentMotorcycle.Add(rentMoto);
                _dbContext.Motorcycle.Update(moto);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new RentMotorcycleResponse
                {
                    Return = "Sucess",
                    Id = moto.Id
                };
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation(string.Format(Messages.Failed, ex.Message));
                return new RentMotorcycleResponse
                {
                    Return = $"Error : {ex.Message} ",
                    Id = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.FailedSaveBike, ex.Message));
                throw;
            }
        }

        private async Task CheckLicenseIsAllowed()
        {
            var qualifiedDriver = await _dbContext.DeliveryMen.AnyAsync(d => d.TypeLicense.Equals(TypeLicense.A) || d.TypeLicense.Equals(TypeLicense.AB));

            if (!qualifiedDriver)
                throw new ValidationException(new ValidationItem { Message = "Somente entregadores habilitados na categoria A podem efetuar uma locação" });
        }

        private async Task<Motorcycle> TakeMotoAvailableAsync(CancellationToken cancellationToken)
        {
            var queryMoto = _dbContext.Motorcycle.Take(10).Where(m => m.Rented == false);
            var moto = await _dbContext.Motorcycle.FirstOrDefaultAsync(cancellationToken) ?? throw new ValidationException(new ValidationItem { Message = "Não existem motos disponíveis no momento" });
            moto.SetRented();
            return moto;
        }
    }
}
