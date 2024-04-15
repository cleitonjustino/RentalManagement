using FluentValidation;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.DeliveryMan.AddDeliveryMan
{
    public sealed class ReturnRentMotorcycleRequestValidator : AbstractValidator<ReturnRentMotorcycleRequest>
    {
        public ReturnRentMotorcycleRequestValidator()
        {           
            RuleFor(x => x.FinalDate).NotNull().WithMessage("FinalDate is required");
         }
    }
}
