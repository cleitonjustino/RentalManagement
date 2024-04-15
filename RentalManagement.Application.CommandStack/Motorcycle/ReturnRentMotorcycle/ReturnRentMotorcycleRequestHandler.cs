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
    public class ReturnRentMotorcycleRequestHandler : IRequestHandler<ReturnRentMotorcycleRequest, ReturnRentMotorcycleResponse>
    {
        private readonly ILogger<ReturnRentMotorcycleRequestHandler> _logger;
        private readonly RentalDbContext _dbContext;

        public ReturnRentMotorcycleRequestHandler(ILogger<ReturnRentMotorcycleRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ReturnRentMotorcycleResponse> Handle(ReturnRentMotorcycleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var plans = Plans.ReturnPlans();
                var rentMoto = _dbContext.RentMotorcycle.FirstOrDefault(x => x.Id == request.IdRent);

                rentMoto?.CalculateFine();
                _dbContext.RentMotorcycle.Update(rentMoto);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new ReturnRentMotorcycleResponse
                {
                    Return = "Sucess",
                    Id = rentMoto.Id
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
