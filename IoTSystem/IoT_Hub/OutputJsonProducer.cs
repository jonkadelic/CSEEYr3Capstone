using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class OutputJsonProducer
    {
        private readonly string hubName;

        public OutputJsonProducer(string hubName)
        {
            this.hubName = hubName;
        }
        public JObject GetTopLevelDevices()
        {
            Utility.WriteTimeStamp("Building hub information as JSON string", typeof(OutputJsonProducer));
            JObject outObject = new JObject
            {
                { "label", hubName },
                { "devices", GetDevicesArray() }
            };
            return outObject;
        }
        private JArray GetDevicesArray()
        {
            Utility.WriteTimeStamp("Building list of all devices in JSON", typeof(OutputJsonProducer));
            JArray devicesArray = new JArray();
            foreach (Driver d in DriverLoader.Drivers)
            {
                List<DriverDevice> ddList = d.Devices;
                Utility.WriteTimeStamp($"Found {ddList.Count} devices in {d.Name}", typeof(OutputJsonProducer));
                foreach (DriverDevice dd in ddList)
                {
                    devicesArray.Add(GetDevice(d, dd));
                }
            }
            return devicesArray;
        }
        public JObject GetDevice(Driver driver, DriverDevice device, bool propertyValues = false)
        {
            JObject outputObject = new JObject()
            {
                { "label", device.GetDynamicDevice().Label },
                { "name", device.GetDynamicDevice().Name },
                { "manufacturer", device.GetDynamicDevice().Manufacturer },
                { "driverId", driver.Id },
                { "deviceId", device.GetDynamicDevice().Id },
                { "readOnly", device.DeviceBase.IsReadOnly },
                { "properties", propertyValues ? new JArray(device.DeviceBase.DeviceProperties.Select(x => GetDeviceProperty(x))) : new JArray(device.DeviceBase.DeviceProperties.Select(x => x.Label)) }
            };
            Utility.WriteTimeStamp($"Found {device.DeviceBase.DeviceProperties.Count} device properties for {device.GetDynamicDevice().Name} in {driver.Name}", typeof(OutputJsonProducer));
            return outputObject;
        }
        private JArray GetDeviceProperties(DriverDevice device)
        {
            return new JArray(device.DeviceBase.DeviceProperties.Select(x => GetDeviceProperty(x)));
        }
        public JObject GetDeviceProperty(DeviceProperty property)
        {
            dynamic devVar = property;
            JObject jsonDevVar = new JObject
            {
                { "label", devVar.Label },
                { "type", property.PropertyType.FullName },
                { "value", devVar.Get().ToString() }
            };
            try
            {
                jsonDevVar.Add("minValue", devVar.propertyRange.minValue.ToString());
                jsonDevVar.Add("maxValue", devVar.propertyRange.maxValue.ToString());
            }
            catch (Exception) { }
            return jsonDevVar;
        }


        public JArray GetRoutinesArray()
        {
            JArray outArray = new JArray();
            foreach(Database.Routine r in RoutineScheduler.routines)
                outArray.Add(GetRoutine(r));
            return outArray;
        }

        public JObject GetRoutine(Database.Routine r)
        {
            JObject outObject = new JObject()
            {
                { "id", r.RoutineID.ToString() },
                { "name", r.RoutineName },
                { "target", new JObject()
                    {
                        { "deviceId", r.TargetDeviceID },
                        { "driverId", r.TargetDriverID },
                        { "property", r.TargetProperty },
                        { "value", r.TargetValue }
                    }
                },
                { "conditions", GetRoutineConditions(r.RoutineConditions) }
            };
            return outObject;
        }

        public JArray GetRoutineConditions(List<Database.RoutineCondition> conditions)
        {
            JArray array = new JArray();
            foreach (Database.RoutineCondition rc in conditions)
            {
                array.Add(GetRoutineCondition(rc));
            }
            return array;
        }

        public JObject GetRoutineCondition(Database.RoutineCondition c)
        {
            JObject outObject = new JObject()
            {
                { "id", c.RoutineConditionID.ToString() },
                { "deviceId", c.DeviceID },
                { "driverId", c.DriverID },
                { "property", c.PropertyName },
                { "value", c.DesiredValue },
                { "comparison", c.Comparison.ToString("g") } // format specifier "g" means to provide the literal name of the enum member
            };
            return outObject;
        }
    }
}
