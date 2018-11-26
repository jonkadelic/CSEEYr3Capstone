using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class OutputJsonProducer
    {
        private readonly List<Driver> drivers;
        private readonly string hubName;

        public OutputJsonProducer(List<Driver> drivers, string hubName)
        {
            this.drivers = drivers;
            this.hubName = hubName;
        }

        public JObject GetHubInformation()
        {
            Utility.WriteTimeStamp("Building hub information as JSON string", typeof(HttpRequestListener));
            JObject outObject = new JObject
            {
                { "label", hubName },
                { "devices", GetAllDevices() }
            };
            return outObject;
        }
        private JArray GetAllDevices()
        {
            Utility.WriteTimeStamp("Building list of all devices in JSON", typeof(HttpRequestListener));
            Utility.WriteTimeStamp($"Found {drivers.Count} drivers", typeof(HttpRequestListener));
            JArray devicesArray = new JArray();
            foreach (Driver d in drivers)
            {
                List<DriverDevice> ddList = d.Devices;
                Utility.WriteTimeStamp($"Found {ddList.Count} devices in {d.Name}", typeof(HttpRequestListener));
                foreach (DriverDevice dd in ddList)
                    devicesArray.Add(GetDevice(d, dd));
            }
            return devicesArray;
        }
        public JObject GetDevice(Driver driver, DriverDevice device)
        {
            JObject outputObject = new JObject()
            {
                { "label", device.GetConvertedDevice().Label },
                { "name", device.GetConvertedDevice().Name },
                { "manufacturer", device.GetConvertedDevice().Manufacturer },
                { "driverId", driver.driverId },
                { "deviceId", device.deviceId },
                { "attributes", new JArray(device.basicDevice.DeviceAttributes.Select(x => x.Label)) }
            };
            Utility.WriteTimeStamp($"Found {device.basicDevice.DeviceAttributes.Count} device attributes for device in {driver.Name}", typeof(HttpRequestListener));
            return outputObject;
        }
        private JArray GetDeviceAttributes(DriverDevice device)
        {
            return new JArray(device.basicDevice.DeviceAttributes.Select(x => GetDeviceAttribute(x)));
        }
        public JObject GetDeviceAttribute(DeviceAttribute attribute)
        {
            dynamic devVar = attribute;
            JObject jsonDevVar = new JObject
            {
                { "label", devVar.Label },
                { "type", attribute.AttributeType.Name },
                { "value", devVar.Get().ToString() }
            };
            return jsonDevVar;
        }
    }
}
