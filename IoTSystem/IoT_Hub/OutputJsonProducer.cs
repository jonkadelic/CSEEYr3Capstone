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
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Building hub information as JSON string");
            JObject outObject = new JObject
            {
                { "label", hubName },
                { "devices", GetAllDevices() }
            };
            return outObject;
        }
        private JArray GetAllDevices()
        {
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Building list of all devices in JSON");
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Found {drivers.Count} drivers");
            JArray devicesArray = new JArray();
            foreach (Driver d in drivers)
            {
                List<DriverDevice> ddList = d.Devices;
                Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Found {ddList.Count} devices in {d.Name}");
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
                { "variables", new JArray(device.basicDevice.DeviceAttributes.Select(x => x.Label)) }
            };
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Found {device.basicDevice.DeviceAttributes.Count} device attributes for device in {driver.Name}");
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
