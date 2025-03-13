using Microsoft.AspNetCore.Mvc;
using CloudSales.Application.Interfaces;

namespace CloudSales.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountsController(IAccountRepository accountRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
    {
        // Get the UserId from the current authenticated user
        var customerId = GetCustomerIdFromClaims();

        var accounts = await accountRepository.GetAccountsModelsAsync(customerId, cancellationToken);

        return Ok(accounts);
    }

    [HttpGet("subscription")]
    public async Task<IActionResult> GetAccountsWithSubscriptions(CancellationToken cancellationToken)
    {
        // Get the UserId from the current authenticated user
        //var customerId = GetCustomerIdFromClaims();
        var customerId = 1;

        var accounts = await accountRepository.GetAccountsWithSubscriptionsModelsAsync(customerId, cancellationToken);

        return Ok(accounts);
    }

    private int GetCustomerIdFromClaims()
    {
        return 1;
        //// Logic to extract UserId from JWT claims (assuming you have authentication set up)
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //return Guid.Parse(userId);
    }
}