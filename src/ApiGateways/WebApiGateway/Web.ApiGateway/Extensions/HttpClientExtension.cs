using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Web.ApiGateway.Extensions
{
	public static class HttpClientExtension
	{
		public static async Task<TResult> PostGetResponseAsync<TResult, TValue>(this HttpClient client, String url, TValue value)
		{
			var httpRes = await client.PostAsJsonAsync(url, value);

			return httpRes.IsSuccessStatusCode ? await httpRes.Content.ReadFromJsonAsync<TResult>() : default;
			  
		}

		public static async Task PostAsync<TValue>(this HttpClient client, String url, TValue value)
		{
			await client.PostAsJsonAsync(url, value);
		}

		public static async Task<T> GetResponseAsync<T>(this HttpClient client, String url)
		{
			return await client.GetFromJsonAsync<T>(url);
		}
	}
}
