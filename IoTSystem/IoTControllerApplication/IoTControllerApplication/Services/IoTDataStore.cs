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
    class IoTDataStore : IDataStore<IoTDevice>
    {
        List<IoTDevice> devices;

        public static string HttpUrl = "http://192.168.2.85";

        public IoTDataStore()
        {
            devices = new List<IoTDevice>();
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
                    string s = await client.DownloadStringTaskAsync($"{HttpUrl}/all/");
                    jt = JToken.Parse(s);
                }
                foreach (JToken t in jt["devices"])
                {
                    IoTDevice dev = new IoTDevice(t["label"].Value<string>(), t["name"].Value<string>(), t["manufacturer"].Value<string>(), t["driverId"].Value<string>(), t["deviceId"].Value<string>());
                    foreach (JValue v in t["attributes"])
                    {
                        string attributeLabel = v.Value<string>();
                        JToken att;
                        using (WebClient client = new WebClient())
                        {
                            string s = await client.DownloadStringTaskAsync($"{HttpUrl}/{dev.DriverId}/{dev.DeviceId}/{attributeLabel}/");
                            att = JToken.Parse(s);
                        }
                        string minValue = "", maxValue = "";
                        try
                        {
                            minValue = att["minValue"].Value<string>();
                            maxValue = att["maxValue"].Value<string>();
                        }
                        catch (Exception) { }
                        dev.Attributes.Add(new DeviceAttribute(att["label"].Value<string>(), Type.GetType(att["type"].Value<string>()), dev, minValue, maxValue));
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
                string s = client.DownloadString(HttpUrl + "/all/");
                jt = JToken.Parse(s);
            }
            JToken t = jt["devices"].Where(x => x["driverId"].Value<string>() == device.DriverId && x["deviceId"].Value<string>() == device.DeviceId).FirstOrDefault();
            IoTDevice dev = new IoTDevice(t["label"].Value<string>(), t["name"].Value<string>(), t["manufacturer"].Value<string>(), t["driverId"].Value<string>(), t["deviceId"].Value<string>());
            foreach (JValue v in t["attributes"])
            {
                string attributeLabel = v.Value<string>();
                JToken att;
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString($"{HttpUrl}/{dev.DriverId}/{dev.DeviceId}/{attributeLabel}/");
                    att = JToken.Parse(s);
                }
                dev.Attributes.Add(new DeviceAttribute(att["label"].Value<string>(), Type.GetType(att["type"].Value<string>()), dev));
            }
            devices.Remove(device);
            devices.Add(dev);
            return await Task.FromResult(true);
        }
    }
}
