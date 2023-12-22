using AutoMapper;
using EasyTest.BLL.Mappers;
using EasyTest.BLL.Services;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.User;
using EasyTest.Shared.Enums;
using EasyTest.Shared.Helpers;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EasyTest.BLL.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<AuthOptions> _authOptions;

        public AuthServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMapperProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _userManager = A.Fake<UserManager<User>>();
            _authOptions = Options.Create(new AuthOptions
            {
                Issuer = "Issuer",
                Audience = "Audience",
                Key = "FqsadawdJ8coFHGFAQNuzfmksft",
                TokenExpiration = 60
            });
        }

        [Fact]
        public async Task AuthService_Login_WithValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userLoginDto = new UserLoginDto { Email = "user@example.com", Password = "password" };

            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userLoginDto.Email)).Returns(new User { Email = "user@example.com", Name = "User", Role = UserRolesConst.StudentRole });
            A.CallTo(() => _userManager.CheckPasswordAsync(A<User>.Ignored, userLoginDto.Password)).Returns(true);
            A.CallTo(() => _userManager.GetRolesAsync(A<User>.Ignored)).Returns(Task.FromResult<IList<string>>(new List<string> { "student" }));

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.NotNull(result.Data.AccessToken);
        }

        [Fact]
        public async Task AuthService_Login_WithNonExistingUser_ReturnsErrorResponse()
        {
            // Arrange
            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userLoginDto = new UserLoginDto { Email = "nonexistentuser@example.com", Password = "password" };

            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userLoginDto.Email)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("User does not found", result.Message);
        }


        [Fact]
        public async Task AuthService_Login_WithInvalidPassword_ReturnsErrorResponse()
        {
            // Arrange
            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userLoginDto = new UserLoginDto { Email = "user@example.com", Password = "wrongpassword" };

            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userLoginDto.Email)).Returns(new User { Email = "user@example.com", Name = "User" });
            A.CallTo(() => _userManager.CheckPasswordAsync(A<User>.Ignored, userLoginDto.Password)).Returns(false);

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("Provided password is wrong", result.Message);
        }

        [Fact]
        public async Task AuthService_Register_WithAdminRole_ReturnsErrorResponse()
        {
            // Arrange
            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userRegisterDto = new UserRegisterDto { Email = "admin@example.com", Password = "password", Role = UserRoles.Admin };

            // Act
            var result = await authService.Register(userRegisterDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("You cannot register user with role admin", result.Message);
        }

        [Fact]
        public async Task AuthService_Register_WithExistingEmail_ReturnsErrorResponse()
        {
            // Arrange
            var existingUser = new User { Email = "existinguser@example.com", Name = "ExistingUser" };
            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(existingUser.Email)).Returns(existingUser);

            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userRegisterDto = new UserRegisterDto { Email = existingUser.Email, Password = "password", Role = UserRoles.Student };

            // Act
            var result = await authService.Register(userRegisterDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("User with this email already exists", result.Message);
        }

        [Fact]
        public async Task AuthService_Register_WithInvalidRegistration_ReturnsErrorResponse()
        {
            // Arrange
            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(A<string>.Ignored)).Returns(Task.FromResult<User>(null));

            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userRegisterDto = new UserRegisterDto { Email = "newuser@example.com", Password = "password", Role = UserRoles.Student };

            A.CallTo(() => _userManager.CreateAsync(A<User>.Ignored, userRegisterDto.Password)).Returns(IdentityResult.Failed(new IdentityError { Description = "Registration failed" }));

            // Act
            var result = await authService.Register(userRegisterDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("Failed to register user", result.Message);
        }

        [Fact]
        public async Task AuthService_Register_WithInvalidRoleAssociation_ReturnsErrorResponse()
        {
            // Arrange
            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(A<string>.Ignored)).Returns(Task.FromResult<User>(null));
            A.CallTo(() => _userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Success);
            A.CallTo(() => _userManager.AddToRoleAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Failed(new IdentityError { Description = "Role association failed" }));

            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userRegisterDto = new UserRegisterDto { Email = "newuser@example.com", Password = "password", Role = UserRoles.Student };

            // Act
            var result = await authService.Register(userRegisterDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("Failed to associate user with provided role", result.Message);
        }

        [Fact]
        public async Task AuthService_Register_WithValidRegistration_ReturnsSuccessResponse()
        {
            // Arrange
            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(A<string>.Ignored)).Returns(Task.FromResult<User>(null));
            A.CallTo(() => _userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Success);
            A.CallTo(() => _userManager.AddToRoleAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Success);

            var authService = new AuthService(_unitOfWork, _mapper, _userManager, _authOptions);
            var userRegisterDto = new UserRegisterDto { Email = "newuser@example.com", Password = "password", Role = UserRoles.Student };

            // Act
            var result = await authService.Register(userRegisterDto);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.NotNull(result.Data.AccessToken);
            Assert.Equal("You successfully register user", result.Message);
        }
    }
}
