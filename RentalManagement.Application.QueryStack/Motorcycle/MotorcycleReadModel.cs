namespace RentalManagement.Application.QueryStack.Motorcycle
{
    public class MotorcycleReadModel
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public DateTimeOffset DateRegister { get; set; }
    }
}
