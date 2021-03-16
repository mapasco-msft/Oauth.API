using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Oauth.API.Authentication;
using Oauth.API.Services;

namespace Oauth.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AdminController : ControllerBase
	{

		private readonly ILogger<AdminController> _logger;
		private readonly IClaimsService claimsService;
		public AdminController(ILogger<AdminController> logger, IClaimsService service)
		{
			_logger = logger;
			claimsService = service;
		}

		[HttpGet]
		// Maps to GET http://url/Admin
		// Allow API Users with a JWT or browser with cookie
		[Authorize(Roles = "Admin", AuthenticationSchemes = AzureAuthenticationServiceConfiguration.JWT_SCHEME + "," + AzureAuthenticationServiceConfiguration.COOKIE_SCHEME)]
		public IActionResult Get()
		{
			var name = claimsService.GetUserName(HttpContext.User);
			return Ok($"Welcome {name}");
		}

		[HttpGet("Api")]
		// Maps to GET http://url/Admin/Api
		// Only allow admins with a JWT token
		[Authorize(Roles = "Admin", AuthenticationSchemes = AzureAuthenticationServiceConfiguration.JWT_SCHEME)]
		public IActionResult GetApiOnly()
		{
			var name = claimsService.GetUserName(HttpContext.User);
			return Ok($"Welcome {name}, this route only allows JWT access");
		}
	}
}
