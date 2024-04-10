using System.Linq.Expressions;

namespace RentalManagement.Domain.Extensions
{
    public class SubstituteParameterVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Sub { get; set; } = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node) =>
            Sub.TryGetValue(node, out var newValue) ? newValue : node;
    }
}
