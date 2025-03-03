using System.Globalization;

namespace Support.Services.Common.Models.Exceptions
{
    public class ApiException : ApplicationException
    {
        public ApiException() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, params object[] args) :
            base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
