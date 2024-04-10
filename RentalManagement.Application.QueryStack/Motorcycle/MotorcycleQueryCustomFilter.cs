﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoFramework.Linq;
using RentalManagement.Domain.Extensions;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class MotorcycleQueryCustomFilter : IRequestPreProcessor<MotorcycleQuery>
    {
        private readonly ILogger<MotorcycleQueryCustomFilter> logger;

        public Task Process(MotorcycleQuery request, CancellationToken cancellationToken)
        {
            request.FilterExpression = FilterExpression(request);

            return Task.CompletedTask;
        }

        private Expression<Func<Domain.Motorcycle, bool>> FilterExpression(MotorcycleQuery request)
        {
            var filterExp = PredicateExtensions.Begin<Domain.Motorcycle>(true);

            filterExp = FilterById(request, filterExp);
            filterExp = FilterByModel(request, filterExp);

            return filterExp;
        }

        private Expression<Func<Domain.Motorcycle, bool>> FilterByModel(MotorcycleQuery request, Expression<Func<Domain.Motorcycle, bool>> expression)
        {
            if (!string.IsNullOrWhiteSpace(request.Model))
            {               
                expression = expression.AndAlso(x => x.Model.ToLower() == request.Model.ToLower());
            }
            return expression; ;
        }

        private static Expression<Func<Domain.Motorcycle, bool>> FilterById(MotorcycleQuery request,
            Expression<Func<Domain.Motorcycle, bool>> expression)
        {
            if (request.Id is not null && request.Id != Guid.Empty)
            {
                var bytes = GuidConverter.ToBytes(request.Id.Value, GuidRepresentation.PythonLegacy);
                var csuuid = new Guid(bytes);

                expression = expression.AndAlso(x => x.Id == csuuid);
            }
            return expression;
        }
    }
}
