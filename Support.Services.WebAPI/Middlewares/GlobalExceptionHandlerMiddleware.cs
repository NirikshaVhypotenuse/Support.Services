using Microsoft.AspNetCore.Diagnostics;
using Support.Services.Common.Models.Exceptions;
using System.Net;
using System.Web;

namespace Support.Services.WebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            (int statusCode, string errorMessage, Dictionary<string, string> validationErrors, ErrorResponse<string> errorResponse) = exception switch
            {
                BadRequestException badRequestException => ((int)HttpStatusCode.BadRequest, badRequestException.Message, badRequestException.ErrorResponse.ValidationErrors, badRequestException.ErrorResponse),
                NotFoundException => ((int)HttpStatusCode.NotFound, exception.Message, null, null),
                ForbidException => ((int)HttpStatusCode.Forbidden, exception.Message, null, null),
                _ => ((int)HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.", null, null)
            };

            _logger.LogError($"An exception occurred during the request: HTTP {statusCode}: {httpContext.Request.Method} {httpContext.Request.Path}" +
                        $"{HttpUtility.UrlDecode(httpContext.Request.QueryString.Value)} - {exception.Message}");


            var error = new ErrorResponse<string>(errorMessage, null);

            //errorResponse.Status = statusCode;
            error.Message = errorMessage;
            error.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            error.Data = null;
            error.ValidationErrors = new Dictionary<string, string>();
            //problemDetails.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id ?? httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
