using Newtonsoft.Json;

namespace RentalManagement.Domain.Common
{
    public class ValidationException : Exception
    {
        public ValidationItem[] Errors { get; }

        public ValidationException(params ValidationItem[] errors)
            : base(JsonConvert.SerializeObject(errors.Select((ValidationItem x) => x.Message)))
        {
            Errors = errors;
        }
    }

}
