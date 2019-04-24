using IoTControllerApplication.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IoTControllerApplication.Services
{
    class DeviceDataStore : IDataStore<IoTDevice>
    {
        public static DeviceDataStore DataStore { get; private set; } = new DeviceDataStore();

        List<IoTDevice> devices;

        public static string HttpUrl;

        public DeviceDataStore()
        {
            devices = new List<IoTDevice>();
            HttpUrl = "http://" + ServerLocator.GetServerLocation().ToString();
        }

        public async Task<bool> AddItemAsync(IoTDevice device)
        {
            devices.Add(device);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldDevice = devices.Where((IoTDevice arg) => arg.Id == id).FirstOrDefault();
            devices.Remove(oldDevice);

            return await Task.FromResult(true);
        }

        public async Task<IoTDevice> GetItemAsync(string id)
        {
            return await Task.FromResult(devices.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<IoTDevice>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                JToken jt;
                devices.Clear();
                using (WebClient client = new WebClient())
                {
                    string s = await client.DownloadStringTaskAsync($"{HttpUrl}/devices/");
                    jt = JToken.Parse(s);
                }
                foreach (JToken t in jt["devices"])
                {
                    IoTDevice dev = new IoTDevice(t["label"].Value<string>(), t["name"].Value<string>(), t["manufacturer"].Value<string>(), t["driverId"].Value<string>(), t["deviceId"].Value<string>(), t["readOnly"].Value<bool>());
                    foreach (JValue v in t["properties"])
                    {
                        string propertyLabel = v.Value<string>();
                        dev.Properties.Add(new DeviceProperty(propertyLabel, dev));
                    }
                    devices.Add(dev);
                }
            }
            return await Task.FromResult(devices);
        }

        public async Task<bool> UpdateItemAsync(IoTDevice device)
        {
            JToken jt;
            using (WebClient client = new WebClient())
            {
                string s = client.DownloadString($"{HttpUrl}/devices/");
                jt = JToken.Parse(s);
            }
            JToken t = jt["devices"].Where(x => x["driverId"].Value<string>() == device.DriverId && x["deviceId"].Value<string>() == device.DeviceId).FirstOrDefault();
            IoTDevice dev = new IoTDevice(t["label"].Value<string>(), t["name"].Value<string>(), t["manufacturer"].Value<string>(), t["driverId"].Value<string>(), t["deviceId"].Value<string>(), t["readOnly"].Value<bool>());
            foreach (JValue v in t["properties"])
            {
                string propertyLabel = v.Value<string>();
                JToken att;
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString($"{HttpUrl}/devices/{dev.DriverId}/{dev.DeviceId}/{propertyLabel}/");
                    att = JToken.Parse(s);
                }
                dev.Properties.Add(new DeviceProperty(att["label"].Value<string>(), dev));
            }
            devices.Remove(device);
            devices.Add(dev);
            return await Task.FromResult(true);
        }
    }
}
