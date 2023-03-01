using System;
using System.Text.Json;

namespace APIManager
{
    public class APICaller
    {
        private static string API_URL = "https://api-lprgi.natono.biz/api/GetConfig";
        public static string GetJSON()
        {
            var webClient = new System.Net.WebClient();
            webClient.Headers.Add("x-functions-key", "lprgi_api_key_2023");
            string jsonString = webClient.DownloadString(API_URL);
            return jsonString;
        }
    }
}
