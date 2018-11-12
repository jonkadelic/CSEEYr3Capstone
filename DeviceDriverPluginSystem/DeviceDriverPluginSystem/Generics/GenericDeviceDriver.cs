using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem.Generics
{
    public static class GenericDeviceDriver
    {
        /// <summary>
        ///     Static ID counter, used for assigning devices a new ID.
        /// </summary>
        private static int uidCounter = 0;

        /// <summary>
        ///     List of all devices that the device driver is managing.
        /// </summary>
        public static List<GenericDevice> Devices { get; } = new List<GenericDevice>();

        /// <summary>
        ///     Creates a new instance of the device this GenericDeviceDriver is managing and adds it to Devices, giving it the correct UID.
        /// </summary>
        public static void CreateDevice()
        {
            Devices.Add(new GenericDevice(uidCounter));
            uidCounter++;
        }
    }
}
