using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClinicBookingApi.Helpers
{
	public class AuthorizeOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var authAttributes = context.MethodInfo
				.GetCustomAttributes(true)
				.OfType<AuthorizeAttribute>()
				.Distinct();

			if (authAttributes.Any())
			{
				operation.Security = new List<OpenApiSecurityRequirement>();

				var roles = context.MethodInfo.GetCustomAttributes(true)
					.OfType<AuthorizeAttribute>()
					.Where(attr => !string.IsNullOrEmpty(attr.Roles))
					.SelectMany(attr => attr.Roles!.Split(','))
					.Select(r => r.Trim());

				operation.Security.Add(new OpenApiSecurityRequirement
				{
					[new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, context.Document)] = roles.ToList()
				});
			}
		}
	}
}