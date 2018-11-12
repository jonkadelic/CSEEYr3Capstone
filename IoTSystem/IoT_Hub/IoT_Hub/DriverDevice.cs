using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class DriverDevice
    {
        private readonly Type deviceType;
        public GenericDevice device;
        public DriverDevice(Type deviceType, GenericDevice device)
        {
            this.deviceType = deviceType;
            this.device = device;
        }
        public List<string> ValueList => device.DeviceVariables.Select(x => x.Label).ToList();
        public dynamic GetValue(string name)
        {
            DeviceVariable value = device[name];
            Type valueType = typeof(DeviceVariable<>).MakeGenericType(value.VariableType);
            dynamic devVal = Convert.ChangeType(value, valueType);
            return devVal.Get();
        }

    }
}
