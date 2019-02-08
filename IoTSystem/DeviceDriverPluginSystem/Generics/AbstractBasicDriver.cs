using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem.Generics
{
    public abstract class AbstractBasicDriver
    {
        /// <summary>
        ///     List of all devices that the device driver is managing.
        /// </summary>
        public static List<AbstractBasicDevice> Devices { get; }

        public static Type DeviceType => typeof(AbstractBasicDevice);

        public static void Initialize()
        {
            return;
        }
    }
}
