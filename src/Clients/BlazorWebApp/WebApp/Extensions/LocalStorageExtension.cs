using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace WebApp.Extensions
{
	public static class LocalStorageExtension
	{

		public static string GetUsername(this ISyncLocalStorageService localStorageService)
		{
			return localStorageService.GetItem<string>("username");
		}

		public static async Task<string> GetUsername(this ILocalStorageService localStorageService)
		{
			return await localStorageService.GetItemAsync<string>("username");
		}

		public static void SetUsername(this ISyncLocalStorageService localStorageService, string value)
		{
			localStorageService.SetItem("username", value);
		}

		public static async Task SetUsername(this ILocalStorageService localStorageService, string value)
		{
			await localStorageService.SetItemAsync("username", value);
		}



		public static string GetToken(this ISyncLocalStorageService localStorageService)
		{
			return localStorageService.GetItem<string>("token");
		}

		public static async Task<string> GetToken(this ILocalStorageService localStorageService)
		{
			return await localStorageService.GetItemAsync<string>("username");
		}

		public static void SetToken(this ISyncLocalStorageService localStorageService, string value)
		{
			localStorageService.SetItem("token", value);
		}

		public static async Task SetToken(this ILocalStorageService localStorageService, string value)
		{
			await localStorageService.SetItemAsync("token", value);
		}

	}
}
