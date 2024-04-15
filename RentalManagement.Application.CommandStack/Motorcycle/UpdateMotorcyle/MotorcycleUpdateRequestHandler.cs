using MediatR;
using MongoFramework.Linq;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class MotorcycleUpdateRequestHandler : IRequestHandler<MotorcycleUpdateRequest, MotorcycleUpdateResponse>
    {
        private readonly ILogger<MotorcycleUpdateRequestHandler> _logger;
        public RentalDbContext _dbContext;

        public MotorcycleUpdateRequestHandler(ILogger<MotorcycleUpdateRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
         
        public async Task<MotorcycleUpdateResponse> Handle(MotorcycleUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var moto = await _dbContext.Motorcycle.FirstOrDefaultAsync(m => m.Id.Equals(request.Id), cancellationToken: cancellationToken) ?? throw new ValidationException(new ValidationItem { Message = "Moto não encontrada" });

                moto.ChangePlateNumber(request.PlateNumber);

                _dbContext.Motorcycle.Update(moto);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new MotorcycleUpdateResponse
                {
                    Return = "Sucess",
                    Id = moto.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.FailedSaveBike, ex.Message));
                throw;
            }

        }
    }
}
