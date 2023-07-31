using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BasketService.Api.Core.Application.Services
{
	public class IdentityService : IIdentityService
	{

		private readonly IHttpContextAccessor httpContextAccessor;

		public IdentityService(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}
			 

		public string GetUserName()
		{
			//Identity Service de claim in name identifiaer içerisine username yi vermiştik burada da alıyoruz.
			return httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
		}
	}
}
