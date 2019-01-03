using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDriver_TP_LINK_HS100
{
    public class PlugDriver : AbstractBasicDriver
    {
        private static readonly string AccessToken = "762d7865-B52EPmdI2vYOwy6YodTmwxA";

        private static readonly string[] HttpHeader = new[] { "Content-Type", "application/json" };

        private static readonly string HttpUrl = "https://eu-wap.tplinkcloud.com/?token=" + AccessToken;

        private static readonly string ProductIdentifier = "IOT.SMARTPLUGSWITCH";

        private static readonly HttpClient client = new HttpClient();

        public static new Type DeviceType => typeof(Plug);

        public static new List<AbstractBasicDevice> Devices
        {
            get
            {
                List<AbstractBasicDevice> Devices = new List<AbstractBasicDevice>();
                JArray json = GetAllDevicesJson();
                foreach (JToken deviceJson in json.Children())
                {
                    if (deviceJson["deviceType"].ToString() == ProductIdentifier)
                    {
                        Devices.Add(new Plug(deviceJson["deviceId"].ToString()));
                    }
                }
                return Devices;
            }
        }

        public static JArray GetAllDevicesJson()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl),
                Method = HttpMethod.Post,
                Content = new StringContent("{\"method\":\"getDeviceList\"}", Encoding.UTF8, "application/json")
            };

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;
                JToken jt = JToken.Parse(json);
                return jt["result"]["deviceList"] as JArray;
            }
        }

        public static JToken GetJsonDeviceInfo(string apiID)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl),
                Method = HttpMethod.Post,
                Content = new StringContent("{\"method\":\"passthrough\", \"params\": {\"deviceId\": \"" + apiID + "\", \"requestData\": \"{\\\"system\\\":{\\\"get_sysinfo\\\":null}}\" }}", Encoding.UTF8, "application/json")
            };

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;
                return JToken.Parse(json);
            }
        }

        internal static JToken GetJsonByID(string apiID)
        {
            JArray ja = GetAllDevicesJson();
            return ja.Where(x => x["deviceId"].ToString() == apiID).First();
        }

        internal static void SetPoweredState(string apiID, bool state)
        {
            string messageContent = "{\"method\":\"passthrough\", \"params\": {\"deviceId\": \"" + apiID + "\", \"requestData\": \"{\\\"system\\\":{\\\"set_relay_state\\\":{\\\"state\\\":" + (state ? "1" : "0") + "}}}\" }}";
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl),
                Method = HttpMethod.Post,
                Content = new StringContent(messageContent, Encoding.UTF8, "application/json")
            };
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
