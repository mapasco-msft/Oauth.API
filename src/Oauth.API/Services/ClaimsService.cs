using System;
using System.Security.Claims;
namespace Oauth.API.Services
{
	public class ClaimsService : IClaimsService
	{
		public string GetUserName(ClaimsPrincipal user)
		{
			var name = user.FindFirst(claim => claim.Type == "name");
			return name?.Value ?? throw new NullReferenceException();
		}
		public string GetAppId(ClaimsPrincipal user)
		{
			var name = user.FindFirst(claim => claim.Type == "appid");
			return name?.Value ?? throw new NullReferenceException();
		}

	}
}