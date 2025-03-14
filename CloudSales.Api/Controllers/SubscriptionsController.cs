using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/subscription")]
public class SubscriptionsController(ISubscriptionService subscriptionService) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.CreateSubscriptionAsync(customerId, request, cancellationToken);

        return Created();
    }

    [HttpPut("{id:int:min(1)}/quantity")]
    public async Task<IActionResult> UpdateSubscriptionQuantity(int id, UpdateQuantityRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.UpdateQuantityAsync(customerId, id, request.Quantity, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}/extend")]
    public async Task<IActionResult> UpdateSubscriptionExpiration(int id, UpdateExpirationRequestModel request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.UpdateExpirationAsync(customerId, id, request.ValidTo, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}/cancel")]
    public async Task<IActionResult> CancelSubscription(int id, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromClaims();

        await subscriptionService.CancelSubscription(customerId, id, cancellationToken);

        return NoContent();
    }
}
