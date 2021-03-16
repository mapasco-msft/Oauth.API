using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Oauth.API.Controllers
{
	[ApiController]
	[Route("/")]
	public class UnauthenticatedController : ControllerBase
	{
		private readonly ILogger<UnauthenticatedController> _logger;

		public UnauthenticatedController(ILogger<UnauthenticatedController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[AllowAnonymous]
		// Maps to GET http://url/
		public IActionResult Get()
		{
			return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}
}
