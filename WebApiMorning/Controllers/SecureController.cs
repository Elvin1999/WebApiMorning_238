using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiMorning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            dynamic user = HttpContext.Items["User"];
            if (user == null) return Unauthorized("Invalid Token");
            var name = user.Name;
            var fullname = user.Fullname;
            return Ok($"Hello {name}, {fullname} you are authenticated!");
        }
    }
}
