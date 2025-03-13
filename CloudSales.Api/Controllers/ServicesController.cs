using CloudSales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServicesController(IServiceRepository serviceRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var services = await serviceRepository.GetServicesModelsAsync(cancellationToken);

            return Ok(services);
        }
    }
}
