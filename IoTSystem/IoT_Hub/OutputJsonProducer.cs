using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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

        public string GetHubInformation()
        {
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Building hub information as JSON string");
            JObject outObject = new JObject
            {
                { "label", hubName },
                { "devices", GetAllDevices() }
            };
            return outObject.ToString();
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
                {
                    devicesArray.Add(GetDevice(d, dd));
                }
            }
            return devicesArray;
        }
        private JObject GetDevice(Driver driver, DriverDevice device)
        {
            JObject outputObject = new JObject()
            {
                { "label", device.GetConvertedDevice().Label },
                { "name", device.GetConvertedDevice().Name },
                { "manufacturer", device.GetConvertedDevice().Manufacturer },
                { "deviceId", device.deviceId },
                { "driverId", driver.driverId },
                { "variables", new JArray(GetDeviceVariables(device)) }
            };
            Console.WriteLine($"{typeof(OutputJsonProducer).Name}: Found {device.basicDevice.DeviceAttributes.Count} device attributes for device in {driver.Name}");
            return outputObject;
        }
        private JArray GetDeviceVariables(DriverDevice device)
        {
            JArray outputArray = new JArray();
            for (int i = 0; i < device.basicDevice.DeviceAttributes.Count; i++)
            {
                dynamic devVar = device.basicDevice.DeviceAttributes[i];
                JObject jsonDevVar = new JObject
                {
                    { "label", devVar.Label },
                    { "identifier", i },
                    { "type", device.basicDevice.DeviceAttributes[i].AttributeType.Name },
                    { "value", devVar.Get().ToString() }
                };
                outputArray.Add(jsonDevVar);
            }
            return outputArray;
        }
    }
}
