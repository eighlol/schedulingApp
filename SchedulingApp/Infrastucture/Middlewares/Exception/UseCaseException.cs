using System.Diagnostics.CodeAnalysis;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SchedulingApp.Infrastucture.Middleware.Exception
{
    [ExcludeFromCodeCoverage]
    public class UseCaseException : System.Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ContentType { get; set; } = @"text/plain";

        public UseCaseException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public UseCaseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public UseCaseException(HttpStatusCode statusCode, System.Exception inner) : this(statusCode, inner.ToString()) { }

        public UseCaseException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            ContentType = @"application/json";
        }
    }
}
