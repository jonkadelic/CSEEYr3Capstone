using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem.Generics
{
    public class GenericDeviceDriver
    {
        /// <summary>
        ///     List of all devices that the device driver is managing.
        /// </summary>
        public static List<GenericDevice> Devices { get; }

        public static Type DeviceType => typeof(GenericDevice);
    }
}
