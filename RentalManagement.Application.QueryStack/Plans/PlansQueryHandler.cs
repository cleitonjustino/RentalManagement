using MediatR;
using Microsoft.Extensions.Logging;
using RentalManagement.Application.QueryStack.Motorcycle;
using RentalManagement.Domain.Enums;
using RentalManagement.Domain.Extensions;
using System.ComponentModel;

namespace RentalManagement.Application.QueryStack.Plans
{
    public class PlansQueryHandler : IRequestHandler<PlansQuery, List<string>>
    {
        private readonly ILogger<MotorcycleQueryHandler> _logger;

        public PlansQueryHandler(ILogger<MotorcycleQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<List<string>> Handle(PlansQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Query Plans executed");

                var lista = Enum.GetValues(typeof(PaymentPlans))
                            .Cast<PaymentPlans>()
                            .Select(v => v.GetDescription())
                            .ToList();
                return Task.FromResult(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Query Plans Fail", ex.Message));
                throw;
            }
        }
    }
}
