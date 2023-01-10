using System.Text.Json;

namespace Patronage.Application.CustomExceptionMiddleware
{
    internal class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}