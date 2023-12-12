using EasyTest.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace EasyTest.WebAPI.Middlewares
{
	public class TokenValidationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AuthOptions _authOptions;

		public TokenValidationMiddleware(RequestDelegate next, IOptions<AuthOptions> authOptions)
		{
			_next = next;
			_authOptions = authOptions.Value;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
			{
				string token = authHeader.ToString().Replace("Bearer ", "");

				if (IsValidToken(token))
				{
					await _next.Invoke(context);
					return;
				}
			}

			var endpoint = context.GetEndpoint();
			if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return;
			}

			await _next.Invoke(context);
		}

		private bool IsValidToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,

				IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_authOptions.Key)),
				ValidIssuer = _authOptions.Issuer,
				ValidAudience = _authOptions.Audience,
			};

			try
			{
				SecurityToken validatedToken;
				tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
