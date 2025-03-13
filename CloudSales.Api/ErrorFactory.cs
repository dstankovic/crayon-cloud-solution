using CloudSales.Api.Models;
using CloudSales.Domain.Exceptions;
using System.Net;

namespace CloudSales.Api
{
    public class ErrorFactory
    {
        public ErrorResponse GetErrorResponse(Exception exception)
        {
            return exception switch
            {
                UnauthorizedException => new ErrorResponse((int)HttpStatusCode.Forbidden, "Unauthorized.", string.Empty),
                EntityNotFoundException => new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.", exception.Message),
                _ => new ErrorResponse((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.", exception.Message),
            };
        }
    }
}
