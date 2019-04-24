using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IoTControllerApplication.Models
{
    public class DeviceProperty
    {
        public string Label { get; private set; }
        public Type PropertyType { get; private set; }
        public dynamic MinValue { get; set; } = null;
        public dynamic MaxValue { get; set; } = null;
        public IoTDevice Device { get; }

        public async Task<dynamic> GetValueAsync()
        {
            JToken prop;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{Services.DeviceDataStore.HttpUrl}/devices/{Device.DriverId}/{Device.DeviceId}/{Label}/");
                string s = await response.Content.ReadAsStringAsync();
                prop = JToken.Parse(s);
            }
            string value = prop["value"].Value<string>();
            try
            {
                MinValue = prop["minValue"].Value<string>();
                MaxValue = prop["maxValue"].Value<string>();
            }
            catch (Exception) { }
            PropertyType = Type.GetType(prop["type"].Value<string>());
            dynamic _value;
            try
            {
                _value = Convert.ChangeType(value, PropertyType);
            }
            catch (InvalidCastException)
            {
                _value = "ERROR: Property is of an unsupported type.";
            }
            catch (Exception)
            {
                _value = "ERROR: Property value could not be parsed.";
            }
            return _value;
        }

        public async Task SetValueAsync(dynamic value)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.GetStringAsync($"{Services.DeviceDataStore.HttpUrl}/devices/{Device.DriverId}/{Device.DeviceId}/{Label}/set?v={value.ToString()}");
            }
        }

        public DeviceProperty(string label, IoTDevice device)
        {
            Label = label;
            Device = device;
        }
    }
}
