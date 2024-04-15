using System.ComponentModel;

namespace RentalManagement.Domain.Enums
{
    public enum PaymentPlans
    {
        [Description("7 dias")]
        Plan7Days = 7,
        [Description("15 dias")]     
        Plan15Days = 15,
        [Description("30 dias")]
        Plan30Days = 30,
    }
}
