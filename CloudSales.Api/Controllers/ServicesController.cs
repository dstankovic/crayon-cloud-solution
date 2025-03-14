using CloudSales.Application.Interfaces;
using CloudSales.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/service")]
    public class ServicesController(IServiceRepository serviceRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetServices(CancellationToken cancellationToken)
        {
            var services = await serviceRepository.GetServicesModelsAsync(cancellationToken);

            return Ok(services);
        }
    }
}
