using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Oauth.API.Services;
using Oauth.API.Authentication;

namespace Oauth.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ServicePrincipalController : ControllerBase
	{

		private readonly ILogger<ServicePrincipalController> _logger;
		private readonly IClaimsService claimsService;
		public ServicePrincipalController(ILogger<ServicePrincipalController> logger, IClaimsService service)
		{
			_logger = logger;
			claimsService = service;
		}

		[HttpGet]
		// Maps to GET http://url/ServicePrincipal
		// Only Allow Service Principals with a JWT (not cookie)
		[Authorize(
			AuthenticationSchemes = AzureAuthenticationServiceConfiguration.JWT_SCHEME, 
			Policy = AzureAuthenticationServiceConfiguration.SP_POLICY
		)]
		public IActionResult Get()
		{
			var name = claimsService.GetAppId(HttpContext.User);
			return Ok($"Welcome {name}");
		}

	}
}
