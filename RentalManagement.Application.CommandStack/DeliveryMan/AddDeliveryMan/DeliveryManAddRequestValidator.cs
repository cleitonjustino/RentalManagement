using FluentValidation;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Extensions;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.DeliveryMan.AddDeliveryMan
{
    public sealed class DeliveryManAddRequestValidator : AbstractValidator<DeliveryManAddRequest>
    {
        public DeliveryManAddRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Birthday).NotNull().WithMessage("Birdth day is required");
            RuleFor(x => x.Birthday).Must(birthDay => DateTime.Now.Year - birthDay.Year >= 18).WithMessage("Underage is not allowed");
            RuleFor(x => x.Birthday).LessThanOrEqualTo(DateTime.Now).WithMessage("Data de nascimento não pode ser uma data futura");
            RuleFor(x => x.NumberLicense).NotNull().NotEmpty().WithMessage("NumberLicense is required");
            RuleFor(x => x.Cnpj).NotNull().NotEmpty().WithMessage("Cnpj is required").IsValidCNPJ();
            RuleFor(x => x.TypeLicense).NotNull().NotEmpty().WithMessage("TypeLicense is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email inválido").MaximumLength(LengthFields.Email).WithMessage("Email maior que o permitido").When(x => !x.Email.IsNullOrWhitespace());
        }
    }
}
