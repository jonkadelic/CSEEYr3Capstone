using System.Collections.Generic;
using System.Linq;

namespace DeviceDriverPluginSystem.GenericDevice
{
    public class GenericDevice
    {
        /// <summary>
        ///     The name of the device.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     List of all the DeviceValue instances associated with the device driver.
        /// </summary>
        public List<DeviceValue> DeviceValues = new List<DeviceValue>();

        /// <summary>
        ///     A device identifier, unique across all devices and all drivers.
        /// </summary>
        public readonly int ID;

        /// <summary>
        ///     Static ID counter, used for assigning devices a new ID.
        /// </summary>
        private static int idCounter;

        public static List<GenericDevice> Devices = new List<GenericDevice>();

        public static void RefreshDeviceList() { }

        public DeviceValue this[string value]
        {
            get
            {
                return DeviceValues.Find(x => x.Label == value);
            }
        }

        public GenericDevice()
        {
            ID = ++idCounter;
        }
    }
}
