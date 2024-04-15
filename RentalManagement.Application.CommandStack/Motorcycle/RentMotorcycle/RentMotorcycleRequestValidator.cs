using FluentValidation;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.DeliveryMan.AddDeliveryMan
{
    public sealed class RentMotorcycleRequestValidator : AbstractValidator<RentMotorcycleRequest>
    {
        public RentMotorcycleRequestValidator()
        {
            RuleFor(x => x.IdDeliveryMan).NotNull().NotEmpty().WithMessage("Delivery man is required");
            RuleFor(x => x.StartDate).NotNull().WithMessage("StartDate day is required");
            RuleFor(x => x.PaymentPlan).NotNull().WithMessage("PaymentPlan is not allowed");
            RuleFor(x => x.ExpectedDate).NotNull().WithMessage("FinalDate is required");
         }
    }
}
