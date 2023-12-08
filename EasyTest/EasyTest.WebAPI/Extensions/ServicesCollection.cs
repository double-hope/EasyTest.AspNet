using EasyTest.BLL.Interfaces;
using EasyTest.BLL.Services;
using EasyTest.DAL;
using EasyTest.DAL.DbInitializer;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using EasyTest.BLL.Mappers;
using EasyTest.Shared.Helpers;

namespace EasyTest.WebAPI.Extensions
{
    public static class ServicesCollection
    {
        public static void RegisterCustomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ISessionService, SessionService>();
		}
        public static void RegisterServices(this IServiceCollection services, IConfiguration config)
        {
			services.Configure<AuthOptions>(config.GetSection("Jwt"));
			services.AddAutoMapper(conf =>
            {
                conf.AddProfiles(
                    new List<Profile>()
                    {
                        new TestMapperProfile(),
                        new UserMapperProfile(),
                        new QuestionMapperProfile(),
                        new AnswerMapperProfile(),
                        new SessionMapperProfile(),
                        new SessionAnswerMapperProfile(),
					});
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
				options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void RegisterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }
        public static void RegisterIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
				options.Password.RequireDigit = true;
				    options.Password.RequireLowercase = true;
				    options.Password.RequireUppercase = true;
				    options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }
    }
}
