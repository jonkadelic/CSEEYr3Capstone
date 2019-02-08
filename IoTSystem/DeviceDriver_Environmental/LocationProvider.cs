using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDriver_Environmental
{
    public static class LocationProvider
    {
        public static double Latitude { get; private set; }

        public static double Longitude { get; private set; }

        public static void ReloadLocation()
        {
            HttpClient client = new HttpClient();
            JToken json;
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.ipdata.co/?api-key=2f73bece30a06ff5a0230076f4bfa71fe8c33c591e37582d14d5772d")
            };

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string jsonDoc = response.Content.ReadAsStringAsync().Result;
                json = JToken.Parse(jsonDoc);
            }
            Latitude = json["latitude"].Value<double>();
            Longitude = json["longitude"].Value<double>();
        }
    }
}
