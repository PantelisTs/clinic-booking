
using ClinicBookingApi.Data;
using ClinicBookingApi.Repositories;
using ClinicBookingApi.Services;
using ClinicBookingApi.Security;
using ClinicBookingApi.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IDoctorService, DoctorService>();
			builder.Services.AddScoped<IPatientService, PatientService>();
			builder.Services.AddScoped<IApplicationService, ApplicationService>();
			builder.Services.AddSingleton<IEncryptionUtil, EncryptionUtil>();

			builder.Services.AddRepositories();

			builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());

			var jwtSettings = builder.Configuration.GetSection("Jwt");

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				//options.IncludeErrorDetails = builder.Environment.IsDevelopment();
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = jwtSettings["Issuer"],

					ValidateAudience = true,
					ValidAudience = jwtSettings["Audience"],

					ValidateLifetime = true,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
				};
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowClient", policy =>
				{
					policy.WithOrigins(builder.Configuration["Cors:Origin"]!)
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});

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

			app.UseCors("AllowClient");

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}