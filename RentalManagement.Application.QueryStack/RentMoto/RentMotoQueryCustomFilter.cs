using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MongoFramework.Linq;
using RentalManagement.Domain.Extensions;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class RentMotoQueryCustomFilter : IRequestPreProcessor<RentMotoQuery>
    {
        private readonly ILogger<RentMotoQueryCustomFilter> logger;

        public Task Process(RentMotoQuery request, CancellationToken cancellationToken)
        {
            request.FilterExpression = FilterExpression(request);

            return Task.CompletedTask;
        }

        private Expression<Func<Domain.RentMotorcycle, bool>> FilterExpression(RentMotoQuery request)
        {
            var filterExp = PredicateExtensions.Begin<Domain.RentMotorcycle>(true);

            filterExp = FilterByName(request, filterExp);

            return filterExp;
        }


        private Expression<Func<Domain.RentMotorcycle, bool>> FilterByName(RentMotoQuery request, Expression<Func<Domain.RentMotorcycle, bool>> expression)
        {
            if (!string.IsNullOrWhiteSpace(request.PlateNumber))
            {
                expression = expression.AndAlso(x => x.PlateNumber.ToLower() == request.PlateNumber.ToLower());
            }
            return expression;
        }

    }
}
