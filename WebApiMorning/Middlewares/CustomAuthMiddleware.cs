using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApiMorning.Middlewares
{
    public class CustomAuthMiddleware
    {
        private RequestDelegate _next;
        private ILogger<CustomAuthMiddleware> _logger;
        private readonly string _secretKey;

        public CustomAuthMiddleware(RequestDelegate next,
            ILogger<CustomAuthMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:Secret not found in configuration");
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = ValidateToken(token);
            }
            else
            {
                _logger.LogWarning("Authorization header missing");

            }

            await _next(context);
        }

        private System.Security.Claims.ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                    ValidateIssuer=false,
                    ValidateAudience=false,
                    ClockSkew=TimeSpan.Zero
                };

                var principal=tokenHandler.ValidateToken(token, validationParameters,out _);
                return principal;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                return null;
            }
        }
    }


    public static class CustomAuthMiddlewareExtentions
    {
        public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder) {

            return builder.UseMiddleware<CustomAuthMiddleware>();
        }
    }
}
