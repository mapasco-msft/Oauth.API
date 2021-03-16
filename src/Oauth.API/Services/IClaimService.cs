using System;
using System.Security.Claims;
namespace Oauth.API.Services
{
	public interface IClaimsService
	{
		string GetUserName(ClaimsPrincipal user);
		string GetAppId(ClaimsPrincipal user);
	}
}