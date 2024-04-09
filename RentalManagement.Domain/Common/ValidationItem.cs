namespace RentalManagement.Domain.Common
{
    public class ValidationItem
    {
        public object Arguments { get; set; }
        public string Error { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
    }
}
