using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.IdentityModel.Protocols.OpenIdConnect;
namespace Oauth.API.Authentication
{
	public static class AzureAuthenticationServiceConfiguration
	{
		public const string JWT_SCHEME = "Bearer";
		public const string COOKIE_SCHEME = "Cookie";
		public const string SP_POLICY = "ServicePrincipal";
		public static void InitAzureOauth(this IServiceCollection services, IConfiguration configuration)
		{
			AzureADOptions azOptions = new AzureADOptions();
			configuration.Bind("AzureAd", azOptions);

			// Add JWT Authentication
			services.AddAuthentication(sharedoptions =>
			{
				sharedoptions.DefaultScheme = JWT_SCHEME;
			})
			.AddJwtBearer(options =>
			{
				options.Authority = azOptions.Instance + azOptions.TenantId;
				options.Audience = azOptions.ClientId;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidAudiences = new string[] { $"api://{azOptions.ClientId}", azOptions.ClientId },
					ValidIssuers = new string[] { $"https://sts.windows.net/{azOptions.TenantId}/", $"https://login.microsoftonline.com/${azOptions.TenantId}/v2.0" }
				};

			});

			// Add cookie based authentication
			services.AddAuthentication(COOKIE_SCHEME)
				.AddAzureAD(options =>
				{
					options.TenantId = azOptions.TenantId;
					options.ClientId = azOptions.ClientId;
					options.Domain = azOptions.Domain;
					options.Instance = azOptions.Instance;
					options.CallbackPath = azOptions.CallbackPath;
				});


			// Add Policies
			services.AddAuthorization(options =>
			{
				// Default policy allows JWT and Cookie Auth
				options.DefaultPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.AddAuthenticationSchemes(JWT_SCHEME, COOKIE_SCHEME)
					.Build();

				// Create a policy based on a claim to identify Service Principal accounts
				options.AddPolicy(SP_POLICY, policy =>
				{
					// Indicates how the client was authenticated. 
					// For a public client, the value is "0". 
					// If client ID and client secret are used, the value is "1". 
					// If a client certificate was used for authentication, the value is "2".
					policy.RequireClaim("appidacr", new[] { "1", "2" });
				});


				// Fallback to require an authenticated user
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});
		}
	}
}