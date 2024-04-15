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

                var existRent = await _dbContext.RentMotorcycle.AnyAsync(r => r.PlateNumber.Equals(moto.PlateNumber));
                if (existRent) throw new ValidationException(new ValidationItem { Message = $"Não é possível excluir a moto {moto.PlateNumber} devido a alugueis vinculoados" });

                _dbContext.Motorcycle.RemoveById(moto.Id);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Exclusão Motorcycle com sucesso");

                return new MotorcycleRemoveResponse
                {
                    Return = "Sucess",
                    Id = moto.Id
                };
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation(string.Format(Messages.Failed, ex.Message));
                return new MotorcycleRemoveResponse
                {
                    Return = $"Error : {ex.Message} ",
                    Id = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Messages.Failed, ex.Message));
                throw;
            }

        }
    }
}
