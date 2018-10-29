using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace DeviceDriver_Lifx_Color_A19
{
    public class MainClass : IGenericActuator
    {

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static string AccessToken => "c9a6ca1e3e7752d0fba45dd24db0202a84cd702839a167c7dd1960a5b111e926";

        public static string[] HttpHeader => new[] {"Authorization", "Bearer " + AccessToken };

        public static string HttpUrl => "https://api.lifx.com/v1/lights/all";

        public static JArray GetAllDevices()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl),
                Method = HttpMethod.Get,
                Headers =
                {
                    { HttpHeader[0], HttpHeader[1] }
                }
            };

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;
                return JArray.Parse(json);
            }
        }
    }
}
