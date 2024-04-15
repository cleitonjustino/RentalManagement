using System.ComponentModel;

namespace RentalManagement.Domain.Enums
{
    public enum PaymentPlans
    {
        [Description("7 dias")]
        Plan7Days,
        [Description("15 dias")]     
        Plan15Days,
        [Description("30 dias")]
        Plan30Days,
    }
}
