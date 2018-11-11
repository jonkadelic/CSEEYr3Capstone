using System.Collections.Generic;

namespace DeviceDriverPluginSystem.GenericDevice
{
    public class GenericDevice
    {
        /// <summary>
        ///     The name of the device.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     A device identifier, unique across all devices and all drivers.
        /// </summary>
        public int ID;

        /// <summary>
        ///     Static ID counter, used for assigning devices a new ID.
        /// </summary>
        private static int idCounter;

        public GenericDevice()
        {
            ID = ++idCounter;
        }
    }
}
