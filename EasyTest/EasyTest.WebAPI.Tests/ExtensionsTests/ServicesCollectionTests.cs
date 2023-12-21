using EasyTest.BLL.Interfaces;
using EasyTest.DAL;
using EasyTest.DAL.DbInitializer;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using FakeItEasy;

namespace EasyTest.WebAPI.Extensions
{
    public class ServicesCollectionTests
    {
		[Fact]
		public void ServicesCollection_RegisterCustomServices_Adds_Services_To_Collection()
        {
            // Arrange
            var services = new ServiceCollection();
			var config = A.Fake<IConfiguration>();

            // Act
            services.RegisterCustomServices(config);

            // Assert
            Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IDbInitializer));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IUnitOfWork));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IAuthService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITestService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IQuestionService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(IAnswerService));
			Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ISessionService));
		}

		[Fact]
		public void ServicesCollection_RegisterDatabase_Adds_DbContext_To_Collection()
        {
            // Arrange
            var services = new ServiceCollection();
			var config = A.Fake<IConfiguration>();

            // Act
            services.RegisterDatabase(config);

            // Assert
            Assert.Contains(services, serviceDescriptor => serviceDescriptor.ServiceType == typeof(ApplicationDbContext));
		}

		[Fact]
		public void ServicesCollection_RegisterIdentity_Adds_Identity_Options()
        {
            // Arrange
            var services = new ServiceCollection();
			var config = A.Fake<IConfiguration>();

            // Act
            services.RegisterIdentity(config);
			var identityOptions = services.BuildServiceProvider().GetService<IOptions<IdentityOptions>>();

            // Assert
            Assert.NotNull(identityOptions);
		}
	}
}
