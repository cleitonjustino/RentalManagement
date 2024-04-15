namespace RentalManagement.Domain.Common
{
    public class Plans
    {
        public static Dictionary<int, double> ReturnPlans()
        {
            return new Dictionary<int, double>
            {
                {7, 30.0D},
                {15, 28.0D },
                {30, 22.0D },
                {45, 20.0D },
                {50, 18.0D }
            };
        }
    }
}
