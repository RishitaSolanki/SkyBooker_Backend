using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SkyBooker.FlightService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var identity = User.Identity;
            return Ok(new { 
                IsAuthenticated = identity.IsAuthenticated,
                Name = identity.Name,
                Claims = claims,
                IsAdmin = User.IsInRole("ADMIN"),
                IsAirlineStaff = User.IsInRole("AIRLINE_STAFF")
            });
        }
    }
}
