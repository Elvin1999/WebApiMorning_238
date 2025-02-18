using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiMorning.Dtos;
using WebApiMorning.Repositories.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiMorning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IStudentRepository studentRepository, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _configuration = configuration;
        }

        // POST api/<AccountController>
        [HttpPost("SignIn")]
        public async Task<ActionResult<string>> Post([FromBody] SignInDto dto)
        {
            var student = await _studentRepository.Get(s => s.Username == dto.Username &&
            s.Password == dto.Password);
            if (student != null)
            {
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, dto.Username),new Claim("Fullname",student.Fullname) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var tokenData = tokenHandler.WriteToken(token);
                return tokenData;
            }
            return "";
        }
    }
}
