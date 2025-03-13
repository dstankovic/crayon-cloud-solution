using Microsoft.AspNetCore.Mvc;

namespace CloudSales.Api.Controllers
{
    [ApiController]
    [Route("api/dummy")]
    public class DummyController : ControllerBase
    {
        private static readonly int[] value = [1, 2, 3];

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(value);
        }
    }
}
