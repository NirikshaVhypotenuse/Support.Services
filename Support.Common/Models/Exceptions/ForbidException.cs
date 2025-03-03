namespace Support.Services.Common.Models.Exceptions
{
    public class ForbidException : ApplicationException
    {
        public ErrorResponse<string> ErrorResponse { get; }
        public ForbidException(string message) : base(message)
        {
            ErrorResponse = new ErrorResponse<string>(message, "https://tools.ietf.org/html/rfc7231#section-6.6.1");
        }
    }
}
