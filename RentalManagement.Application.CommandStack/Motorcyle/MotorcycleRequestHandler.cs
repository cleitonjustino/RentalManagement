using MediatR;
using Microsoft.Extensions.Logging;
using RentalManagement.Domain;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.Motorcyle
{
    public class MotorcycleRequestHandler : IRequestHandler<MotorcycleRequest, MotorcycleResponse>
    {
        private readonly ILogger<MotorcycleRequestHandler> _logger;

        public MotorcycleRequestHandler(ILogger<MotorcycleRequestHandler> logger)
        {
            _logger = logger;
        }

        public async Task<MotorcycleResponse> Handle(MotorcycleRequest request, CancellationToken cancellationToken)
        {
            var motorcycle = new Motorcycle.Builder()
               .SetId()
               .SetPlateNumber(request.PlateNumber)
               .SetModel(request.Model)
               .SetYear(request.Year)
               .Build();

            return new MotorcycleResponse
            {
                Return = "Sucess",
                Id = motorcycle.Id
            };
        }
    }
}
