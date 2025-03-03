namespace Support.Services.Common.Models.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<ErrorModel>();
        }
        public List<ErrorModel> Errors { get; }
        public ValidationException(string message) : base(message)
        {
            Errors = new List<ErrorModel>();
        }

        public ValidationException(string message, List<ErrorModel> validationErrors) : base(message)
        {
            Errors = validationErrors;
        }
    }
}
