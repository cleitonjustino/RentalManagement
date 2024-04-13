using MediatR;
using MongoFramework.Linq;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class MotorcycleRemoveRequestHandler : IRequestHandler<MotorcycleRemoveRequest, MotorcycleRemoveResponse>
    {
        private readonly ILogger<MotorcycleRemoveRequestHandler> _logger;
        public RentalDbContext _dbContext;

        public MotorcycleRemoveRequestHandler(ILogger<MotorcycleRemoveRequestHandler> logger, RentalDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
         
        public async Task<MotorcycleRemoveResponse> Handle(MotorcycleRemoveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var moto = await _dbContext.Motorcycle.FirstOrDefaultAsync(m => m.Id.Equals(request.Id)) ?? throw new ValidationException(new ValidationItem { Message = "Moto não encontrada" });

                _dbContext.Motorcycle.RemoveById(moto.Id);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new MotorcycleRemoveResponse
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
