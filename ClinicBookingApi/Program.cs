
using ClinicBookingApi.Data;
using ClinicBookingApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var connString = builder.Configuration.GetConnectionString("DevConnection");

			builder.Services.AddDbContext<ClinicBookingDbContext>(options =>
				options.UseSqlServer(connString));

			builder.Services.AddRepositories();

			builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClinicBooking API v1");
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
