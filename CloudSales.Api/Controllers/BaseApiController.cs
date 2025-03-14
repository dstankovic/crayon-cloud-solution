using CloudSales.Api.Extensions;
using CloudSales.Domain.Common;
using CloudSales.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        protected int GetCustomerIdFromClaims()
        {
            return User.GetIntValueFromClaims(Constants.Auth.Claims.CustomerId) ?? throw new UnknownCustomerIdException("Customer can't be recognized from the request.");
        }
    }
}
