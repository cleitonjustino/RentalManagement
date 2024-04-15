using FluentValidation;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.DeliveryMan.AddDeliveryMan
{
    public sealed class DeliveryManAddCnhRequestValidator : AbstractValidator<DeliveryManAddCnhRequest>
    {
        public DeliveryManAddCnhRequestValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("File is required");

        }
    }
}
