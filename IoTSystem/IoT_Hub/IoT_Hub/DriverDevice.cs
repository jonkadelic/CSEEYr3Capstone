using DeviceDriverPluginSystem.Generics;
using System;

namespace IoT_Hub
{
    public class DriverDevice
    {
        private readonly Type deviceType;
        public readonly AbstractBasicDevice basicDevice;

        public DriverDevice(Type deviceType, AbstractBasicDevice basicDevice)
        {
            this.deviceType = deviceType;
            this.basicDevice = basicDevice;
        }

        public dynamic GetConvertedDevice()
        {
            return Convert.ChangeType(basicDevice, deviceType);
        }

    }
}
