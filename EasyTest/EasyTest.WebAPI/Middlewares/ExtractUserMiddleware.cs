using System.IdentityModel.Tokens.Jwt;

namespace EasyTest.WebAPI.Middlewares
{
    public class ExtractUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExtractUserMiddleware> _logger;

        public ExtractUserMiddleware(RequestDelegate next, ILogger<ExtractUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("X-Refresh-Token"))
            {
                var refreshToken = context.Request.Cookies["X-Refresh-Token"];

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(refreshToken);

                        var userEmailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");

                        if (userEmailClaim != null)
                        {
                            context.Items["UserEmail"] = userEmailClaim.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to extract user email from the refresh token.");
                    }
                }
            }

            await _next(context);
        }
    }
}
