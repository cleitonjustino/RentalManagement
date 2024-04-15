
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using RentalManagement.Domain.Notification;

namespace RentalManagement.Infrastructure
{
    public class ValidationRequestBehavior<TRequest, Unit> : IPipelineBehavior<TRequest, Unit>
    {
        private readonly IEnumerable<IValidator> _validators;
        private readonly IDomainNotificationContext _notificationContext;

        public ValidationRequestBehavior(IEnumerable<IValidator<TRequest>> validators, IDomainNotificationContext notificationContext)
        {
            _validators = validators;
            _notificationContext = notificationContext;
        }

        public Task<Unit> Handle(TRequest request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
               .Select(v => v.Validate(context))
               .SelectMany(x => x.Errors)
               .Where(f => f != null)
               .ToList();

            return failures.Any() ? Notify(failures) : next();
        }

        private Task<Unit> Notify(IEnumerable<ValidationFailure> failures)
        {
            var result = default(Unit);

            foreach (var failure in failures)
            {
                _notificationContext.NotifyError(failure.ErrorMessage);
            }

            return Task.FromResult(result);
        }
    }
}
