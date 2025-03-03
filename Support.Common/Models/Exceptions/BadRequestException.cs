namespace Support.Services.Common.Models.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public ErrorResponse<string> ErrorResponse { get; }
        public BadRequestException(string message, Dictionary<string, string> validationErrors = null) : base(message)
        {
            ErrorResponse = new ErrorResponse<string>(message, "https://tools.ietf.org/html/rfc7231#section-6.6.1", validationErrors);
        }
    }
}
