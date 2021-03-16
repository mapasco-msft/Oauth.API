using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


using Oauth.API.Authentication;
using Oauth.API.Services;

namespace Oauth.API
{
	public class Startup
	{
		public Startup(IHostEnvironment env)
		{
			if (env == null)
			{
				throw new ArgumentNullException(nameof(env));
			}
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
				.AddEnvironmentVariables();

			this.Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Extension Method in Authentication/AzureAuthenticationServiceConfiguration.cs
			services.InitAzureOauth(this.Configuration);
			// Provide ClaimsService as the provider for IClaimsService in the dependency graph
			services.AddScoped<IClaimsService, ClaimsService>();
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			// Add Requirement for Authentication and Authorization
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
