using System;
using System.Net.Http;
using System.Text.Json;

namespace APIManager
{
    public class APICaller
    {
        private static string API_URL = "https://api-lprgi.natono.biz/api/GetConfig";
        public static string GetJSON()
        {
            var webClient = new HttpClient();
            webClient.DefaultRequestHeaders.Add("x-functions-key", "lprgi_api_key_2023");
            string jsonString = webClient.Send(new HttpRequestMessage(HttpMethod.Get, API_URL)).Content.ReadAsStringAsync().Result;
            return jsonString;
        }
    }
}
