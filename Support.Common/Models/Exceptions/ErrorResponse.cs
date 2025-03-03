namespace Support.Services.Common.Models.Exceptions
{
    public class ErrorResponse<T>
    {
        public ErrorResponse()
        { }
        public ErrorResponse(T data, string? message = null)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ErrorResponse(string message, string type, Dictionary<string, string> validationErrors)
        {
            Success = false;
            Message = message;
            Type = type;
            ValidationErrors = validationErrors;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; }
        public T? Data { get; set; }
        public string Type { get; set; }
    }
    public class ErrorModel
    {
        public string? PropertyName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
