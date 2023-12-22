using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class MockSchemeProvider : AuthenticationSchemeProvider
	{
		public MockSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
		{
		}

		protected MockSchemeProvider(
			IOptions<AuthenticationOptions> options,
			IDictionary<string, AuthenticationScheme> schemes
		)
			: base(options, schemes)
		{
		}

		public override Task<AuthenticationScheme> GetSchemeAsync(string name)
		{
			AuthenticationScheme mockScheme = new(
				IdentityConstants.ApplicationScheme,
				IdentityConstants.ApplicationScheme,
				typeof(MockAuthenticationHandler)
			);
			return Task.FromResult(mockScheme);
		}
	}

	public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly MockClaimSeed _claimSeed;

		public MockAuthenticationHandler(
			MockClaimSeed claimSeed,
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock
		)
			: base(options, logger, encoder, clock)
		{
			_claimSeed = claimSeed;
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var claimsIdentity = new ClaimsIdentity(_claimSeed.getSeeds(), IdentityConstants.ApplicationScheme);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			var ticket = new AuthenticationTicket(claimsPrincipal, IdentityConstants.ApplicationScheme);
			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
	}

	public class MockClaimSeed
	{
		private readonly IEnumerable<Claim> _seed;

		public MockClaimSeed(IEnumerable<Claim> seed)
		{
			_seed = seed;
		}

		public IEnumerable<Claim> getSeeds() => _seed;
	}
	//public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	//{
	//	public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
	//		ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
	//		: base(options, logger, encoder, clock)
	//	{
	//	}

	//	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	//	{
	//		var identity = new ClaimsIdentity(Array.Empty<Claim>(), "Test");
	//		var principal = new ClaimsPrincipal(identity);
	//		var ticket = new AuthenticationTicket(principal, "TestScheme");

	//		var result = AuthenticateResult.Success(ticket);

	//		return Task.FromResult(result);
	//	}
	//}
}
