using System;

namespace Web.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }

        public HttpStatusCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode,  string message, Exception inner) : base(message, inner) { }

        public HttpStatusCodeException(string message) : base(message) { }
    }
}
