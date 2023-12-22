using EasyTest.DAL.DbInitializer;
using EasyTest.WebAPI.Extensions;
using System.Text.Json.Serialization;

public class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
			{
				c.SchemaFilter<EnumSchemaFilter>();
			});

		builder.Services.RegisterDatabase(builder.Configuration);
		builder.Services.RegisterIdentity(builder.Configuration);
		builder.Services.RegisterServices(builder.Configuration);
		builder.Services.RegisterCustomServices(builder.Configuration);
		builder.Services.AddAuthorization();

		builder.Services.AddJwtAuthentication(builder.Configuration);

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
		SeedDatabase();
		app.Run();

		void SeedDatabase()
		{
			using (var scope = app.Services.CreateScope())
			{
				var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
				dbInitializer.Initialize();
			}
		}
	}
}