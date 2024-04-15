using MediatR;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Request;
using RentalManagement.Infrastructure;
using RentalManagement.Infrastructure.ExternalServices.Storage;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class DeliveryManAddCnhRequestHandler : IRequestHandler<DeliveryManAddCnhRequest, DeliveryManAddCnhResponse>
    {
        private readonly ILogger<DeliveryManAddCnhRequestHandler> _logger;
        public RentalDbContext _dbContext;
        private readonly IStorageService _storageService;

        public DeliveryManAddCnhRequestHandler(ILogger<DeliveryManAddCnhRequestHandler> logger, RentalDbContext dbContext, IStorageService storageService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<DeliveryManAddCnhResponse> Handle(DeliveryManAddCnhRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var deliveryMan = await _dbContext.DeliveryMen.FirstOrDefaultAsync(x => x.Id == request.IdDeliveryMan) ?? throw new ValidationException(new ValidationItem { Message = "Falha ao buscar entregador informado" }); 

                var idImage = Guid.NewGuid();

                await _storageService.UploadFileAsync(idImage.ToString(), request.File, request.ContentType);
                deliveryMan.SetIdImageLicense(idImage.ToString());

                _dbContext.DeliveryMen.Update(deliveryMan);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation(string.Format(Messages.Success, idImage));

                return new DeliveryManAddCnhResponse
                {
                    Return = "Sucess",
                    Id = idImage
                };
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation(string.Format(Messages.Failed, ex.Message));

                return new DeliveryManAddCnhResponse
                {
                    Return = $"Error : {ex.Message}",
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
