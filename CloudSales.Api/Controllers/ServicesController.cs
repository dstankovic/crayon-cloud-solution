using CloudSales.Application.Interfaces;
using CloudSales.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudSales.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/service")]
    public class ServicesController(IServiceRepository serviceRepository) : BaseApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetServices(CancellationToken cancellationToken)
        {
            var services = await serviceRepository.GetServicesModelsAsync(cancellationToken);

            return Ok(services);
        }
    }
}
