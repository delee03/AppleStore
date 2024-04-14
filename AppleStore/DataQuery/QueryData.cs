using AppleStore.DataAcess;
using Newtonsoft.Json;

namespace AppleStore.DataQuery
{
	public class TokenGoogle
	{
		//[JsonProperty(PropertyName = "error_description")]
		public string error_description { get; set; }
		//[JsonProperty(PropertyName = "name")]
		public string name { get; set; }
		//[JsonProperty(PropertyName = "email")]
		public string email { get; set; }
	}
	public class QueryData
	{
		private static HttpClient httpClient = new HttpClient();
		private readonly ApplicationDbContext? _context;

		public bool loginAdmin(string email, string password)
		{
			if (email.Equals("admin@a.com") && password.Equals("applestore@123")) return true;
			return false;
		}

		public bool loginAdmin(string password)
		{
			if (password.Equals("applestore@123")) return true;
			return false;
		}

		//public List<Product> GetProductsWithType(string search = "", int order_by = 1, int category_id = 0, int page = 1, int limit = 20)
		//{
		//    TokenGoogle json = new TokenGoogle();
		//    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}"))
		//    {
		//        requestMessage.Headers.Add("Accept", "application/json");
		//        HttpResponseMessage response = httpClient.SendAsync(requestMessage).Result;
		//        if (response.IsSuccessStatusCode)
		//        {
		//            var t = JsonConvert.DeserializeObject<TokenGoogle>(response.Content.ReadAsStringAsync().Result);
		//            if (t.error_description == null)
		//            {
		//                json.name = t.name;
		//                json.email = t.email;
		//            }
		//            else json.error_description = t.error_description;
		//        }
		//    }
		//    return json;
		//}

		public static async Task<TokenGoogle> VerifyTokenGoogle(string token)
		{
			TokenGoogle json = new();
			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}"))
			{
				requestMessage.Headers.Add("Accept", "application/json");
				HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
				if (response.IsSuccessStatusCode)
				{
					var t = JsonConvert.DeserializeObject<TokenGoogle>(await response.Content.ReadAsStringAsync());
					if (t.error_description == null)
					{
						json.name = t.name;
						json.email = t.email;
					}
					else json.error_description = t.error_description;
				}
			}
			return json;
		}

		public async Task<string?> LoginWith(string token, string from)
		{
			if (from.ToLower().Equals("google"))
			{
				TokenGoogle data = await VerifyTokenGoogle(token);
				if (data.error_description == null)
				{

				}
			}

			return null;
		}
	}

}