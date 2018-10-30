﻿using DeviceDriverPluginSystem.GenericDevice;
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
        /// <summary>
        ///     Creates a new Device object with unique ID provided by the JSON data from Lifx.
        /// </summary>
        /// <param name="Id">
        ///     The unique Lifx API of the device.
        /// </param>
        private Device(string Id)
        {
            this.Id = Id;
        }

        /// <summary>
        ///     The Lifx API ID of the device.
        /// </summary>
        private readonly string Id;

        /// <summary>
        ///     The access token for the Lifx API; needs to be made changeable in the future.
        /// </summary>
        private static string AccessToken => "c9a6ca1e3e7752d0fba45dd24db0202a84cd702839a167c7dd1960a5b111e926";

        /// <summary>
        ///     Header for the HTTP request to the Lifx API.
        /// </summary>
        private static string[] HttpHeader => new[] { "Authorization", "Bearer " + AccessToken };

        /// <summary>
        ///     URL for accessing the Lifx HTTP API.
        /// </summary>
        private static string HttpUrl => "https://api.lifx.com/v1/lights/";

        /// <summary>
        ///     Device identifier for the Lifx JSON.
        /// </summary>
        private static string Identifier => "lifx_color_a19";

        /// <summary>
        ///     List of all devices with Identifier equal to "lifx_color_a19".
        /// </summary>
        // TODO: Rewrite as property
        public static List<Device> Devices;

        /// <summary>
        ///     Label of the device as provided by Lifx JSON.
        ///     Cannot be changed by public API so invoking set method does nothing.
        /// </summary>
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

        /// <summary>
        ///     Provided by ILightBulb_Power; allows lightbulb to be turned on and off.
        ///     Acceptable values: true and false
        /// </summary>
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
        /// <summary>
        ///     Provided by ILightBulb_HSL; allows hue to be set as color wheel.
        ///     Interface specifies double but Lifx provides int; however, it is expected that most implementations will use double.
        ///     Acceptable values: 0.0d-360.0d
        /// </summary>
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
        /// <summary>
        ///     Provided by ILightBulb_HSL; allows saturation to be set (how white the light is)
        ///     Acceptable values: 0.0d-1.0d
        /// </summary>
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
        /// <summary>
        ///     Provided by ILightBulb_HSL; allows lightness to be set (how bright the bulb is)
        ///     Analogous to brightness.
        ///     Acceptable values: 0.0d-1.0d (note: setting value to 0.0d also turns off the bulb, setting Powered to false. This is an API thing)
        /// </summary>
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
        /// <summary>
        ///     Provided by ILightBulb_Warmth; allows warmth of the bulb in degrees Kelvin to be set.
        ///     In Lifx API, sets Saturation to 0
        ///     Acceptable values: 1500-9000.
        /// </summary>
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
        /// <summary>
        ///     Fetches the JSON of this specific device from the Lifx servers, using its ID.
        /// </summary>
        /// <returns>
        ///     JSON array of information about the specific device.
        /// </returns>
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

        /// <summary>
        ///     Fetches JSON of all Lifx devices registered to the access token provided.
        /// </summary>
        /// <returns>
        ///     JSON array of information about all devices.
        /// </returns>
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

        /// <summary>
        ///     Retrieves the 
        /// </summary>
        /// <returns></returns>
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
