using Microsoft.AspNetCore.Mvc;
using CloudSales.Application.Interfaces;

namespace CloudSales.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountsController(IAccountRepository accountRepository, ISubscriptionService subscriptionService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        var accounts = await accountRepository.GetAccountsModelsAsync(customerId, cancellationToken);

        return Ok(accounts);
    }

    [HttpGet("subscription")]
    public async Task<IActionResult> GetAccountsWithSubscriptions(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        var accounts = await accountRepository.GetAccountsWithSubscriptionsModelsAsync(customerId, cancellationToken);

        return Ok(accounts);
    }
}