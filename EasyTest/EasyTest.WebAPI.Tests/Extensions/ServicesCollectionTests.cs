using EasyTest.BLL.Interfaces;
using EasyTest.DAL;
using EasyTest.DAL.DbInitializer;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Configuration;

namespace EasyTest.WebAPI.Extensions
{
    public class ServicesCollectionTests
    {
		[Fact]
		public void RegisterCustomServices_Adds_Services_To_Collection()
		{
			var services = new ServiceCollection();
			var config = new Mock<IConfiguration>();

			services.RegisterCustomServices(config.Object);

			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IDbInitializer));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IUnitOfWork));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IAuthService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITestService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IQuestionService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IAnswerService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ISessionService));
		}

		[Fact]
		public void RegisterDatabase_Adds_DbContext_To_Collection()
		{
			var services = new ServiceCollection();
			var config = new Mock<IConfiguration>();

			services.RegisterDatabase(config.Object);

			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ApplicationDbContext));
		}

		[Fact]
		public void RegisterIdentity_Adds_Identity_Options()
		{
			var services = new ServiceCollection();
			var config = new Mock<IConfiguration>();

			services.RegisterIdentity(config.Object);

			var identityOptions = services.BuildServiceProvider().GetService<IOptions<IdentityOptions>>();
			Assert.NotNull(identityOptions);
		}
	}
}
