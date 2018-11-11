using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.GenericDevice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class DriverDevice
    {
        private readonly Type driverType;
        public GenericDevice device;
        public DriverDevice(Type driverType, GenericDevice device)
        {
            this.driverType = driverType;
            this.device = device;
        }
        public List<string> ValueList => device.DeviceValues.Select(x => x.Label).ToList();
        public dynamic GetValue(string name)
        {
            DeviceValue value = device[name];
            Type valueType = typeof(DeviceValue<>).MakeGenericType(value.ValueType);
            dynamic devVal = Convert.ChangeType(value, valueType);
            return devVal.Get();
        }

    }
}
