using MediatR;
using RentalManagement.Application.QueryStack.Helpers;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class RentMotoQuery : IRequest<List<RentMotoReadModel>>
    {
        public string? PlateNumber { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public DateTimeOffset FinalDate { get; set; }

        public Expression<Func<Domain.RentMotorcycle, bool>> FilterExpression { get; set; }
    }
}
