using DeviceDriverPluginSystem.GenericDevice;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using DeviceDriver_Lifx_Color_A19.Interfaces;
using DeviceDriverPluginSystem;

namespace DeviceDriver_Lifx_Color_A19
{
    public class Device : IGenericDevice, IPowered, IHue, ISaturation, IBrightness, IWarmth
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

        public static List<IGenericDevice> GetDevices()
        { 
            List<IGenericDevice> Devices = new List<IGenericDevice>();
            JArray json = GetAllJson();
            foreach (JToken item in json.Children())
                if (item["product"]["identifier"].ToString() == Identifier)
                    Devices.Add(new Device(item["id"].ToString()));
            return Devices;
        }

        string IPowered.Name => "Powered";

        string IHue.Name => "Hue";

        string ISaturation.Name => "Saturation";

        string IBrightness.Name => "Brightness";

        string IWarmth.Name => "Warmth";

        /// <summary>
        ///     Allows lightbulb to be turned on and off.
        ///     Acceptable values: true/false
        /// </summary>
        bool IPowered.Value
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
        ///     Allows hue to be set as color wheel.
        ///     Acceptable values: 0-360
        /// </summary>
        int IHue.Value
        {
            get
            {
                return GetElementInJson()["color"]["hue"].Value<int>();
            }
            set
            {
                if (value >= ((IHue) this).MinValue && value <= ((IHue)this).MaxValue)
                    SetState("color", "hue:" + value.ToString());
            }
        }
        /// <summary>
        ///     Allows saturation to be set (how white the light is)
        ///     Acceptable values: 0.0d-1.0d
        /// </summary>
        double ISaturation.Value
        {
            get
            {
                return GetElementInJson()["color"]["saturation"].Value<double>();
            }
            set
            {
                if (value >= ((ISaturation)this).MinValue && value <= ((ISaturation)this).MaxValue)
                    SetState("color", "saturation:" + value.ToString());
            }
        }
        /// <summary>
        ///     Allows lightness to be set (how bright the bulb is)
        ///     Acceptable values: 0.0d-1.0d (note: setting value to 0.0d also turns off the bulb, setting Powered to false. This is an API thing)
        /// </summary>
        double IBrightness.Value
        {
            get
            {
                return GetElementInJson()["brightness"].Value<double>();
            }
            set
            {
                if (value >= ((IBrightness)this).MinValue && value <= ((IBrightness)this).MaxValue)
                    SetState("color", "brightness:" + value.ToString());
            }
        }
        /// <summary>
        ///     Allows warmth of the bulb in degrees Kelvin to be set.
        ///     In Lifx API, sets Saturation to 0.
        ///     Acceptable values: 1500-9000.
        /// </summary>
        int IWarmth.Value
        {
            get
            {
                return GetElementInJson()["color"]["kelvin"].Value<int>();
            }
            set
            {
                if (value >= ((IWarmth)this).MinValue && value <= ((IWarmth)this).MaxValue)
                    SetState("color", "kelvin:" + value.ToString());
            }
        }

        bool IPowered.MaxValue => true;

        bool IPowered.MinValue => false;

        int IHue.MaxValue => 360;

        int IHue.MinValue => 0;

        double ISaturation.MaxValue => 1.0d;

        double ISaturation.MinValue => 0.0d;

        double IBrightness.MaxValue => 1.0d;

        double IBrightness.MinValue => 0.0d;

        int IWarmth.MaxValue => 9000;

        int IWarmth.MinValue => 1500;

        /// <summary>
        ///     Fetches the JSON of this specific device from the Lifx servers, using its ID.
        /// </summary>
        /// <returns>
        ///     JSON array of information about the specific device.
        /// </returns>
        private JArray GetJson()
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
        private static JArray GetAllJson()
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
        ///     Retrieves the JSON of an individual lightbulb by its Lifx ID.
        /// </summary>
        /// <returns>
        ///     JSON of the lightbulb as a JToken.
        /// </returns>
        private JToken GetElementInJson()
        {
            return GetJson().Single(T => T["id"].ToString() == Id);
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
        private void SetState(string state, string value)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(HttpHeader[0], HttpHeader[1]);
            HttpResponseMessage response = client.PutAsync(HttpUrl + "id:" + Id + "/state", new StringContent(state + "=" + value, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
