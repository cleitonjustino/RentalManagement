using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Enums;

namespace RentalManagement.Domain
{
    public class RentMotorcycle
    {
        public Guid Id { get; private set; }
        public string PlateNumber { get; private set; }
        public PaymentPlans PaymentPlan { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset FinalDate { get; private set; }
        public DateTimeOffset ExpectedDate { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Double Price { get; private set; }

        public class Builder
        {
            private readonly RentMotorcycle _entity = new();

            public Builder SetId()
            {
                _entity.Id = Guid.NewGuid();
                _entity.CreatedAt = DateTimeOffset.Now;
                return this;
            }

            public Builder PlateNumber(string plateNumber)
            {
                _entity.PlateNumber = plateNumber;
                return this;
            }

            public Builder SetPaymentPlan(PaymentPlans paymentPlan)
            {
                _entity.PaymentPlan = paymentPlan;

                if (paymentPlan == PaymentPlans.Plan7Days)
                {
                    _entity.Price = 30D;
                }
                else if (paymentPlan == PaymentPlans.Plan15Days)
                {
                    _entity.Price = 28D;
                }
                else if (paymentPlan == PaymentPlans.Plan30Days)
                {
                    _entity.Price = 22D;
                }

                return this;
            }

            public Builder SetStartDate(DateTimeOffset startDate)
            {
                _entity.StartDate = startDate.AddDays(1);
                return this;
            }

            public Builder SetFinalDate(DateTimeOffset dafaFim)
            {
                _entity.FinalDate = dafaFim;
                //if (paymentPlan == PaymentPlans.Plan7Days)
                //{
                //    _entity.FinalDate = _entity.StartDate.AddDays(7);
                //}
                //else if (paymentPlan == PaymentPlans.Plan15Days)
                //{
                //    _entity.FinalDate = _entity.StartDate.AddDays(15);
                //}
                //else if (paymentPlan == PaymentPlans.Plan30Days)
                //{
                //    _entity.FinalDate = _entity.StartDate.AddDays(30);
                //}
                return this;
            }


            public Builder SetPlateNumber(string plate)
            {
                if (string.IsNullOrWhiteSpace(plate) || plate.Length < LengthFields.Six)
                    throw new ValidationException(new ValidationItem
                    { Message = Messages.InvalidPlateNumber });

                _entity.PlateNumber = plate.ToUpper();
                return this;
            }

            public RentMotorcycle Build()
              => _entity;
        }

    }
}
