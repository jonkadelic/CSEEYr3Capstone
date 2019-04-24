using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace DeviceDriver_Lifx_Color_A19
{
    public class LightbulbDriver : AbstractBasicDriver
    {
        /// <summary>
        ///     The access token for the Lifx API; needs to be made changeable in the future.
        /// </summary>
        private static readonly string AccessToken = "c9a6ca1e3e7752d0fba45dd24db0202a84cd702839a167c7dd1960a5b111e926";
        /// <summary>
        ///     Header for the HTTP request to the Lifx API.
        /// </summary>
        private static readonly string[] HttpHeader = new[] { "Authorization", "Bearer " + AccessToken };
        /// <summary>
        ///     URL for accessing the Lifx HTTP API.
        /// </summary>
        private static readonly string HttpUrl = "https://api.lifx.com/v1/lights/";
        /// <summary>
        ///     Device identifier for the Lifx JSON.
        /// </summary>
        private static readonly string ProductIdentifier = "lifx_color_a19";

        private static readonly HttpClient client = new HttpClient();

        public static new Type DeviceType => typeof(Lightbulb);

        public static new List<AbstractBasicDevice> Devices
        {
            get
            {
                List<AbstractBasicDevice> Devices = new List<AbstractBasicDevice>();
                JArray json = GetAllDevicesJson();
                foreach (JToken deviceJson in json.Children())
                {
                    if (deviceJson["product"]["identifier"].ToString() == ProductIdentifier)
                    {
                        Devices.Add(new Lightbulb(deviceJson["id"].ToString()));
                    }
                }
                return Devices;
            }
        }

        /// <summary>
        ///     Fetches JSON of all Lifx devices registered to the access token provided.
        /// </summary>
        /// <returns>
        ///     JSON array of information about all devices.
        /// </returns>
        internal static JArray GetAllDevicesJson()
        {
            client.DefaultRequestHeaders.Clear();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl + "all"),
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

        /// <summary>
        ///     Retrieves the JSON of an individual lightbulb by its Lifx ID.
        /// </summary>
        /// <returns>
        ///     JSON of the lightbulb as a JToken.
        /// </returns>
        internal static JToken GetJsonByID(string apiID)
        {
            client.DefaultRequestHeaders.Clear();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl + "id:" + apiID),
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
                return JArray.Parse(json)[0];
            }
        }

        /// <summary>
        ///     Sets the state of a parameter in the Lifx API over the network.
        ///     Acceptable parameters are: power; color; brightness; duration; infrared
        /// </summary>
        /// <param name="state">
        ///     The state to be set.
        /// </param>
        /// <param name="value">
        ///     The value to set the state to.
        /// </param>
        internal static void SetDeviceProperty(string apiID, string state, string value)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HttpHeader[0], HttpHeader[1]);
            HttpResponseMessage response = client.PutAsync(HttpUrl + "id:" + apiID + "/state", new StringContent(state + "=" + value, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
        }
    }
}
