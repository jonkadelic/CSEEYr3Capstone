using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    public class Driver
    {
        public Driver(Type driverType, Type deviceType)
        {
            this.driverType = driverType;
            this.deviceType = deviceType;
            driverId = driverCounter++;
        }

        private readonly Type driverType;
        private readonly Type deviceType;

        public readonly int driverId;

        private static int driverCounter = 0;
        private int deviceCounter = 0;
        public string Name => driverType.Name;
        public List<DriverDevice> Devices
        {
            get
            {
                deviceCounter = 0;
                return (driverType.GetProperty("Devices").GetValue(null) as List<AbstractBasicDevice>).Select(x => new DriverDevice(deviceType, x, deviceCounter++)).ToList();
            }
        }
    }
}
