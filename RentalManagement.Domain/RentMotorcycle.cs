using RentalManagement.Domain.Common;
using RentalManagement.Domain.Constants;
using RentalManagement.Domain.Enums;
using System.Numerics;

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
        public double Price { get; private set; }
        public double Ticketed { get; private set; }

        public double CalculateFine()
        {
            var plans = Plans.ReturnPlans();

            double totalDifferenceDays = TotalDifferenceDays();

            if (FinalDate < ExpectedDate)
            {
                ApplyFineForLatePayment(plans, totalDifferenceDays);
            }
            else if (FinalDate > ExpectedDate)
            {
                Ticketed = CalculateFineForExceedingExpectedDate(totalDifferenceDays);
            }

            return Ticketed + Price * (int)PaymentPlan;
        }

        private int TotalDifferenceDays()
        {
            var totalDays = FinalDate.Subtract(StartDate).Days;
            return totalDays - PaymentPlan switch
            {
                PaymentPlans.Plan7Days => 7,
                PaymentPlans.Plan15Days => 15,
                PaymentPlans.Plan30Days => 30,
                _ => 0,
            };
        }

        private void ApplyFineForLatePayment(Dictionary<int, double> plans, double totalDaysLate)
        {
            var percentage = PaymentPlan switch
            {
                PaymentPlans.Plan7Days => 0.20,
                PaymentPlans.Plan15Days => 0.40,
                _ => 0,
            };

            ApplyFineForPaymentPlan(plans, totalDaysLate, percentage);
        }

        private void ApplyFineForPaymentPlan(Dictionary<int, double> plans, double totalDaysLate, double percentage)
        {
            var plan = plans.GetValueOrDefault((int)PaymentPlan);
            double finePercentage = plan * percentage;
            double fineAmount = plan + finePercentage;
            Ticketed = fineAmount * totalDaysLate;
        }

        private double CalculateFineForExceedingExpectedDate(double totalDaysLate)
        {
            const double defaultFinePerDay = 50.0D;
            return totalDaysLate * defaultFinePerDay;
        }

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

                switch (paymentPlan)
                {
                    case PaymentPlans.Plan7Days:
                        _entity.Price = 30D;
                        break;
                    case PaymentPlans.Plan15Days:
                        _entity.Price = 28D;
                        break;
                    case PaymentPlans.Plan30Days:
                        _entity.Price = 22D;
                        break;
                }

                return this;
            }

            public Builder SetStartDate(DateTimeOffset startDate)
            {
                _entity.StartDate = startDate.AddDays(1);
                return this;
            }

            public Builder SetExpectedDate(DateTimeOffset expctedDate)
            {
                _entity.ExpectedDate = expctedDate;
                return this;
            }

            public Builder SetFinalDate(DateTimeOffset dafaFim)
            {
                _entity.FinalDate = dafaFim;
                return this;
            }

            public Builder SetTicketed(DateTimeOffset dafaFim)
            {
                _entity.FinalDate = dafaFim;

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
