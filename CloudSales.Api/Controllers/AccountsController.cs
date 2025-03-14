using Microsoft.AspNetCore.Mvc;
using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Common;
using CloudSales.Api.Extensions;
using CloudSales.Domain.Exceptions;

namespace CloudSales.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountsController(IAccountRepository accountRepository, ISubscriptionService subscriptionService) : ControllerBase
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

    [HttpPost("subscription")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.CreateSubscriptionAsync(customerId, request, cancellationToken);

        return Created();
    }

    [HttpPut("{accountId:int:min(1)}/service/{serviceId:int:min(1)}/quantity")]
    public async Task<IActionResult> UpdateQuantity(int accountId, int serviceId, [FromBody] UpdateQuantityRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.UpdateQuantityAsync(customerId, accountId, serviceId, request.Quantity, cancellationToken);

        return NoContent();
    }

    [HttpPut("{accountId:int:min(1)}/service/{serviceId:int:min(1)}/extend")]
    public async Task<IActionResult> UpdateExpiration(int accountId, int serviceId, [FromBody] UpdateExpirationRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.UpdateExpirationAsync(customerId, accountId, serviceId, request.ValidTo, cancellationToken);

        return NoContent();
    }

    private int GetCustomerIdFromClaims()
    {
        return User.GetIntValueFromClaims(Constants.Auth.Claims.CustomerId) ?? throw new UnknownCustomerIdException("Customer can't be recognized from the request.");
    }
}