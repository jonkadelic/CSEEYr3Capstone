using DeviceDriverPluginSystem.Generics;
using System;

namespace IoT_Hub
{
    /// <summary>
    ///     Models an individual IoT device whose driver is provided through the plugin architecture.
    /// </summary>
    public class DriverDevice
    {
        /// <summary>
        ///     Unique ID for an instance of DriverDevice.
        /// </summary>
        public string Id => DeviceBase.Id;
        /// <summary>
        ///     The AbstractBasicDevice this DriverDevice wraps.
        /// </summary>
        public AbstractBasicDevice DeviceBase { get; }
        /// <summary>
        ///     The Type that implements AbstractBasicDevice, provided by a plugin.
        /// </summary>
        private readonly Type deviceType;

        public DriverDevice(Type deviceType, AbstractBasicDevice basicDevice)
        {
            this.deviceType = deviceType;
            DeviceBase = basicDevice;
        }

        /// <summary>
        ///     Converts the AbstractBasicDevice stored in DeviceBase to an object of the type stored by deviceType.
        /// </summary>
        /// <returns>
        ///     An object of type deviceType.
        /// </returns>
        public dynamic GetDynamicDevice()
        {
            return Convert.ChangeType(DeviceBase, deviceType);
        }

    }
}
