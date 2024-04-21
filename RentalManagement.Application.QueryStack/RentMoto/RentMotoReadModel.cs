namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class RentMotoReadModel
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpectedDate { get; set; }
        public double FineValue { get; set; }
    }
}
