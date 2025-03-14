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

        [HttpGet("nja")]
        public IActionResult AA()
        {

            var secretKey = "super_secret_dev_key_1234567890_long_enough!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Constants.Auth.Claims.UserId, "1"),
                new Claim(Constants.Auth.Claims.CustomerId, "1"),
            }),
                Expires = DateTime.UtcNow.AddMonths(1),
                Issuer = "cloud-sales-app",
                Audience = "cloud-sales-app",
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated Token: {jwt}");

            return Ok();
        }
    }
}
