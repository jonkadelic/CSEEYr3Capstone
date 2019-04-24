using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDriver_DemonstrationVirtualDevice
{
    public class DemonstrationSmartPlugDriver : AbstractBasicDriver
    {
        static HttpClient httpClient = new HttpClient();
        public static new Type DeviceType => typeof(DemonstrationSmartPlug);

        public static new void Initialize()
        {
            IPAddress address = DemonstrationLightBulbDriver.GetDeviceLocation();
            Devices = new List<AbstractBasicDevice>();
            Devices.Add(new DemonstrationSmartPlug(address));
        }

        public static new List<AbstractBasicDevice> Devices { get; private set; }

        internal static JToken GetDeviceJson(DemonstrationSmartPlug device)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://{device.deviceAddress}/request/"),
                Method = HttpMethod.Get
            };

            using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                string json = response.Content.ReadAsStringAsync().Result;
                return JToken.Parse(json);
            }
        }

        internal static void SetDeviceValue(DemonstrationSmartPlug device, string property, dynamic value)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://{device.deviceAddress}/set/{property}/{value}"),
                Method = HttpMethod.Get
            };

            using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
