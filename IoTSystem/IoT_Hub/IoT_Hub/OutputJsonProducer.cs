using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub
{
    public class OutputJsonProducer
    {
        private List<Driver> drivers;
        private string hubName;

        public string GetOutputJsonString(List<Driver> drivers, string hubName)
        {
            this.drivers = drivers;
            this.hubName = hubName;
            return GetOutputJsonJObject().ToString();
        }

        private JObject GetOutputJsonJObject()
        {
            JObject outputObject = new JObject();
            outputObject = AddHubProperties(outputObject);
            return outputObject;
        }
        private JObject AddHubProperties(JObject obj)
        {
            JObject outputObject = new JObject(obj);
            outputObject.Add("label", new JValue(hubName));
            outputObject.Add("devices", new JArray());
            outputObject = AddHubDevices(outputObject);
            return outputObject;
        }
        private JObject AddHubDevices(JObject obj)
        {
            JObject outputObject = new JObject(obj);
            JArray deviceArray = outputObject["devices"] as JArray;
            foreach (Driver driver in drivers)
            {
                foreach (DriverDevice device in driver.Devices)
                {
                    deviceArray.Add(DriverDeviceToJObject(driver, device));
                }
            }
            return outputObject;
        }
        private JObject DriverDeviceToJObject(Driver driver, DriverDevice device)
        {
            JObject outputObject = new JObject();
            outputObject = PopulateDeviceProperties(outputObject, driver, device);
            return outputObject;
        }
        private JObject PopulateDeviceProperties(JObject obj, Driver driver, DriverDevice device)
        {
            JObject outputObject = new JObject(obj)
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
            for (int i = 0; i < device.basicDevice.DeviceVariables.Count; i++)
            {
                dynamic devVar = device.basicDevice.DeviceVariables[i];
                JObject jsonDevVar = new JObject();
                jsonDevVar.Add("label", devVar.Label);
                jsonDevVar.Add("identifier", i);
                jsonDevVar.Add("type", device.basicDevice.DeviceVariables[i].VariableType.Name);
                jsonDevVar.Add("value", devVar.Get().ToString());

                outputArray.Add(jsonDevVar);
            }
            return outputArray;
        }
    }
}
