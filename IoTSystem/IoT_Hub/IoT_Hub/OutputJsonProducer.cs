using Newtonsoft.Json.Linq;
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
            JObject outObject = new JObject();
            outObject.Add("label", hubName);
            outObject.Add("devices", GetAllDevices());
            return outObject.ToString();
        }
        private JArray GetAllDevices()
        {
            JArray devicesArray = new JArray();
            foreach (Driver d in drivers)
            {
                foreach (DriverDevice dd in d.Devices)
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
