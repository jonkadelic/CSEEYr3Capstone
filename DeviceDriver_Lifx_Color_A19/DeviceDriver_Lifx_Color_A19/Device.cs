using DeviceDriverPluginSystem.GenericDevice;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using DeviceDriverPluginSystem;
using System.Diagnostics;

namespace DeviceDriver_Lifx_Color_A19
{
    public class Device : GenericDevice
    {
        /// <summary>
        ///     Creates a new Device object with unique ID provided by the JSON data from Lifx.
        /// </summary>
        /// <param name="LifxID">
        ///     The unique Lifx API of the device.
        /// </param>
        private Device(string LifxID)
        {
            this.LifxID = LifxID;
            PopulateDeviceValueList();
        }

        private void PopulateDeviceValueList()
        {
            DeviceValues.Add(new DeviceValue<bool>
            (
                () => GetElementInJson()["power"].ToString() == "on",
                value => SetState("power", value ? "on" : "off"),
                "Powered",
                false,
                true
            ));
            DeviceValues.Add(new DeviceValue<int>
            (
                () => GetElementInJson()["color"]["hue"].Value<int>(),
                (value) => value >= 0 && value <= 360 ? SetState("color", "hue:" + value.ToString()) : false,
                "Hue",
                0,
                360
            ));
            DeviceValues.Add(new DeviceValue<double>
            (
                () => GetElementInJson()["color"]["saturation"].Value<double>(),
                (value) => value >= 0d && value <= 1d ? SetState("color", "saturation:" + value.ToString()) : false,
                "Saturation",
                0d,
                1d
            ));
            DeviceValues.Add(new DeviceValue<double>
            (
                () => GetElementInJson()["brightness"].Value<double>(),
                (value) => value >= 0d && value <= 1d ? SetState("brightness", value.ToString()) : false,
                "Brightness",
                0d,
                1d
            ));
            DeviceValues.Add(new DeviceValue<int>
            (
                () => GetElementInJson()["color"]["kelvin"].Value<int>(),
                (value) => value > 1500 && value <= 9000 ? SetState("color", "warmth:" + value.ToString()) : false,
                "Warmth",
                1500,
                9000
            ));
        }

        /// <summary>
        ///     The Lifx API ID of the device.
        /// </summary>
        private readonly string LifxID;
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
        public new string Label
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

        public new static void RefreshDeviceList()
        {
            Devices = new List<GenericDevice>();
            JArray json = GetAllJson();
            foreach (JToken item in json.Children())
            {
                if (item["product"]["identifier"].ToString() == Identifier)
                {
                    Devices.Add(new Device(item["id"].ToString()));
                }
            }
        }

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
                RequestUri = new Uri(HttpUrl + "id:" + LifxID),
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
            return GetJson().Single(T => T["id"].ToString() == LifxID);
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
        private bool SetState(string state, string value)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(HttpHeader[0], HttpHeader[1]);
            HttpResponseMessage response = client.PutAsync(HttpUrl + "id:" + LifxID + "/state", new StringContent(state + "=" + value, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
            return true;
        }
    }
}
