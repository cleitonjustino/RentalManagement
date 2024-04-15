using MediatR;
using RentalManagement.Application.QueryStack.Helpers;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class DeliveryManQuery : IRequest<PagedList<DeliveryManReadModel>>
    {
        public string? Name { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public Expression<Func<Domain.DeliveryMan, bool>> FilterExpression { get; set; }
    }
}
