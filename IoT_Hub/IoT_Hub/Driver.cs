using DeviceDriverPluginSystem.GenericDevice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class Driver
    {
        public Driver(Type driverType)
        {
            this.driverType = driverType;
        }

        private readonly Type driverType;
        public string Name => driverType.Name;
        public List<DriverDevice> Devices
        {
            get
            {
                driverType.InvokeMember("RefreshDeviceList", System.Reflection.BindingFlags.InvokeMethod, null, null, null);
                return (driverType.BaseType.GetField("Devices").GetValue(null) as List<GenericDevice>).Select(x => new DriverDevice(driverType, x)).ToList();
            }
        }
    }
}
