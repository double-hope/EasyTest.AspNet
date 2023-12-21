using EasyTest.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		public readonly IAuthService _authService;
		public readonly IQuestionService _questionService;
		public readonly ISessionService _sessionService;
		public readonly ITestService _testService;
		public WebApplicationFactory<Program> AuthenticatedInstance(params Claim[] claimSeed)
		{
			return WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<IAuthenticationSchemeProvider, MockSchemeProvider>();
					services.AddSingleton<MockClaimSeed>(_ => new(claimSeed));
					services.AddSingleton(_testService);
					services.AddSingleton(_authService);
					services.AddSingleton(_questionService);
					services.AddSingleton(_sessionService);
				});
			});
		}
		public CustomWebApplicationFactory()
        {
            _authService = A.Fake<IAuthService>();
			_questionService = A.Fake<IQuestionService>();
			_sessionService = A.Fake<ISessionService>();
			_testService = A.Fake<ITestService>();
		}

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

			builder.ConfigureTestServices(services =>
            {
				services.AddSingleton(_testService);
                services.AddSingleton(_authService);
                services.AddSingleton(_questionService);
                services.AddSingleton(_sessionService);
			});
        }
    }
}
