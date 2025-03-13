using CloudSales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionsController(IAccountRepository accountRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetLicenses(CancellationToken cancellationToken)
        {
            // Get the UserId from the current authenticated user
            //var customerId = GetCustomerIdFromClaims();
            var customerId = 1;

            var accounts = await accountRepository.GetAccountsWithSubscriptionsModelsAsync(customerId, cancellationToken);

            return Ok(accounts);
        }

    }
}
