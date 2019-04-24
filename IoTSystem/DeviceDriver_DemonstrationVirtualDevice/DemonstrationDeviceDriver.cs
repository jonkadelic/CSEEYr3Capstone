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
    public class DemonstrationDeviceDriver : AbstractBasicDriver
    {
        static HttpClient httpClient = new HttpClient();
        static UdpClient udpClient = new UdpClient();
        public static new Type DeviceType => typeof(DemonstrationDevice);

        public static new List<AbstractBasicDevice> Devices
        {
            get
            {
                List<AbstractBasicDevice> devices = new List<AbstractBasicDevice>
                {
                    new DemonstrationDevice(GetDeviceLocation())
                };
                return devices;
            }
        }


        private static IPAddress GetDeviceLocation()
        {
            udpClient.Client.ReceiveTimeout = 5000;
            byte[] data = Encoding.ASCII.GetBytes("verify");
            udpClient.EnableBroadcast = true;


            udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            string response = "";
            bool gotResponse = false;
            while (gotResponse == false)
            {
                udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
                try
                {
                    response = Encoding.ASCII.GetString(udpClient.Receive(ref endpoint));
                    gotResponse = true;
                }
                catch (SocketException)
                {
                }
                if (response != "device_verified") gotResponse = false;
            }

            return endpoint.Address;
        }

        internal static JToken GetDeviceJson(DemonstrationDevice device)
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
    }
}
