using DeviceDriverPluginSystem.Generics;
using System;

namespace IoT_Hub
{
    public class DriverDevice
    {
        public readonly int deviceId;

        private readonly Type deviceType;
        public readonly AbstractBasicDevice basicDevice;

        public DriverDevice(Type deviceType, AbstractBasicDevice basicDevice, int deviceId)
        {
            this.deviceType = deviceType;
            this.basicDevice = basicDevice;
            this.deviceId = deviceId;
        }

        public dynamic GetConvertedDevice()
        {
            return Convert.ChangeType(basicDevice, deviceType);
        }

    }
}
