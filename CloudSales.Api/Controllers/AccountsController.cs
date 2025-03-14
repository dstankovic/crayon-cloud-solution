using Microsoft.AspNetCore.Mvc;
using CloudSales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CloudSales.Api.Controllers;

[Authorize]
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