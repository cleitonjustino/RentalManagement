using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Domain.Extensions;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class DeliveryManQueryCustomFilter : IRequestPreProcessor<DeliveryManQuery>
    {
        private readonly ILogger<DeliveryManQueryCustomFilter> logger;

        public Task Process(DeliveryManQuery request, CancellationToken cancellationToken)
        {
            request.FilterExpression = FilterExpression(request);

            return Task.CompletedTask;
        }

        private Expression<Func<Domain.DeliveryMan, bool>> FilterExpression(DeliveryManQuery request)
        {
            var filterExp = PredicateExtensions.Begin<Domain.DeliveryMan>(true);

            filterExp = FilterByName(request, filterExp);

            return filterExp;
        }


        private Expression<Func<Domain.DeliveryMan, bool>> FilterByName(DeliveryManQuery request, Expression<Func<Domain.DeliveryMan, bool>> expression)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                expression = expression.AndAlso(x => x.Name.ToLower() == request.Name.ToLower());
            }
            return expression;
        }
    }
}
