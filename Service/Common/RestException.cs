using System;
using System.Net;

namespace Service.Common
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RestException(HttpStatusCode statusCode, string message = default) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
