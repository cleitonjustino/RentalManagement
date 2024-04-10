using MediatR;
using System.Linq.Expressions;

namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class MotorcycleQuery: IRequest<List<MotorcycleReadModel>>
    {
        public Guid? Id { get; set; }
        public string Model { get; set; }
        public Expression<Func<Domain.Motorcycle, bool>> FilterExpression { get; set; }
    }
}
