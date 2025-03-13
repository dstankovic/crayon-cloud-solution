using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionsController(ISubscriptionService subscriptionService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequestModel request, CancellationToken cancellationToken)
        {
            // Get the UserId from the current authenticated user
            var customerId = 1;

            await subscriptionService.CreateSubscriptionAsync(customerId, request, cancellationToken);

            return Created();
        }

    }
}
