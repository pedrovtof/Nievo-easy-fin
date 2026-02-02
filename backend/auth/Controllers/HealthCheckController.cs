using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet("check")]
        public IActionResult HealthCheck()
        {
            var _response = new { message = "Alive" };
            return Ok(_response);
        }
    }
}