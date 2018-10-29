using DeviceDriverPluginSystem.GenericDevice;
using DeviceDriverPluginSystem.LightBulb;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DeviceDriver_Lifx_Color_A19
{
    public class Device : IGenericDevice, ILightBulb_Power, ILightBulb_HSL, ILightBulb_Warmth
    {

        public Device(string Id)
        {
            this.Id = Id;
        }

        private readonly string Id;

        public static string AccessToken => "c9a6ca1e3e7752d0fba45dd24db0202a84cd702839a167c7dd1960a5b111e926";

        public static string[] HttpHeader => new[] { "Authorization", "Bearer " + AccessToken };

        public static string HttpUrl => "https://api.lifx.com/v1/lights/";

        public static string Identifier => "lifx_color_a19";

        public static List<Device> Devices;

        public string Label
        {
            get
            {
                return GetElementInJson()["label"].ToString();
            }
            set
            {
                return;
            }
        }
        public bool Powered
        {
            get
            {
                return GetElementInJson()["power"].ToString() == "on";
            }
            set
            {
                SetState("power", value ? "on" : "off");
            }
        }
        public double Hue
        {
            get
            {
                return GetElementInJson()["color"]["hue"].Value<double>();
            }
            set
            {
                if (value >= 0d && value <= 360d)
                    SetState("color", "hue:" + ((int) value).ToString());
            }
        }
        public double Saturation
        {
            get
            {
                return GetElementInJson()["color"]["saturation"].Value<double>();
            }
            set
            {
                if (value >= 0.0d && value <= 1.0d)
                    SetState("color", "saturation:" + value.ToString());
            }
        }
        public double Lightness
        {
            get
            {
                return GetElementInJson()["brightness"].Value<double>();
            }
            set
            {
                if (value >= 0.0d && value <= 1.0d)
                    SetState("color", "brightness:" + value.ToString());
            }
        }
        public int Warmth
        {
            get
            {
                return GetElementInJson()["color"]["kelvin"].Value<int>();
            }
            set
            {
                if (value >= 1500 && value <= 9000)
                    SetState("color", "kelvin:" + value.ToString());
            }
        }

        public JArray GetJson()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl + "id:" + Id),
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
                client.Dispose();
                return JArray.Parse(json);
            }
        }

        public static JArray GetAllJson()
        {
            HttpClient client = new HttpClient();
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
                client.Dispose();
                return JArray.Parse(json);
            }
        }

        public JToken GetElementInJson()
        {
            return GetJson().Single(T => T["id"].ToString() == Id);
        }

        private void SetState(string state, string value)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(HttpHeader[0], HttpHeader[1]);
            HttpResponseMessage response = client.PutAsync(HttpUrl + "id:" + Id + "/state", new StringContent(state + "=" + value, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        public static void GetDevices()
        {
            Devices = new List<Device>();
            JArray json = GetAllJson();
            foreach (JToken item in json.Children())
                if (item["product"]["identifier"].ToString() == Identifier)
                    Devices.Add(new Device(item["id"].ToString()));
        }
    }
}
