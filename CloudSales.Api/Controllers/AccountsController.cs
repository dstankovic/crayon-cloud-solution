using Microsoft.AspNetCore.Mvc;
using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;

namespace CloudSales.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountsController(IAccountRepository accountRepository, ISubscriptionService subscriptionService) : ControllerBase
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

    [HttpPost("subscription")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequestModel request, CancellationToken cancellationToken)
    {
        // Get the UserId from the current authenticated user
        var customerId = 1;

        await subscriptionService.CreateSubscriptionAsync(customerId, request, cancellationToken);

        return Created();
    }

    [HttpPut("{accountId:int:min(1)}/service/{serviceId:int:min(1)}/quantity")]
    public async Task<IActionResult> UpdateQuantity(int accountId, int serviceId, [FromBody] UpdateQuantityRequestModel request, CancellationToken cancellationToken)
    {
        // Get the UserId from the current authenticated user
        var customerId = 1;

        await subscriptionService.UpdateQuantityAsync(customerId, accountId, serviceId, request.Quantity, cancellationToken);

        return NoContent();
    }

    [HttpPut("{accountId:int:min(1)}/service/{serviceId:int:min(1)}/extend")]
    public async Task<IActionResult> UpdateExpiration(int accountId, int serviceId, [FromBody] UpdateExpirationRequestModel request, CancellationToken cancellationToken)
    {
        // Get the UserId from the current authenticated user
        var customerId = 1;

        await subscriptionService.UpdateExpirationAsync(customerId, accountId, serviceId, request.ValidTo, cancellationToken);

        return NoContent();
    }

    private int GetCustomerIdFromClaims()
    {
        return 1;
        //// Logic to extract UserId from JWT claims (assuming you have authentication set up)
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //return Guid.Parse(userId);
    }
}