using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Oauth.API.Services;

namespace Oauth.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> _logger;
		private readonly IClaimsService claimsService;

		public UserController(ILogger<UserController> logger, IClaimsService service)
		{
			_logger = logger;
			claimsService = service;
		}

		[HttpGet]
		[Authorize]
		// Maps to GET http://url/User
		// Allows any authenticated user
		public IActionResult Get()
		{
			var name = claimsService.GetUserName(User);
			return Ok($"Welcome, {name}");
		}
	}
}
