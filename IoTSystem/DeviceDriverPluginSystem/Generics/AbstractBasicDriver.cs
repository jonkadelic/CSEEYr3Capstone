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

        /// <summary>
        ///     The class that this driver manages.
        /// </summary>
        public static Type DeviceType => typeof(AbstractBasicDevice);

        /// <summary>
        ///     An initialisation routine.
        /// </summary>
        public static void Initialize()
        {
            return;
        }
    }
}
