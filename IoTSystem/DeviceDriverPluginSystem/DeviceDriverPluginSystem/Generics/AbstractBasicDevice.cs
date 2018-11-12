using System.Collections.Generic;

namespace DeviceDriverPluginSystem.Generics
{
    public abstract class AbstractBasicDevice
    {
        /// <summary>
        ///     Static ID counter, used for assigning devices a new ID.
        /// </summary>
        protected static int uidCounter = 0;

        /// <summary>
        ///     The name of the device.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     A device identifier, unique across all devices and all drivers.
        /// </summary>
        public int UID { get; protected set; }

        /// <summary>
        ///     List of all the DeviceVariable instances associated with the device.
        /// </summary>
        public List<DeviceVariable> DeviceVariables { get; } = new List<DeviceVariable>();

        /// <summary>
        ///     Retrieve a DeviceVariable object from DeviceValues by label.
        /// </summary>
        /// <param name="label">
        ///     The name of the DeviceVariable to be found; equivalent to its Label field.
        /// </param>
        /// <returns>
        ///     The DeviceVariable whose label is equal to the supplied value.
        /// </returns>
        public DeviceVariable this[string label] => DeviceVariables.Find(x => x.Label == label);

        /// <summary>
        ///     Creates a new GenericDevice and assigns a numerical identifier to it.
        /// </summary>
        /// <param name="ID">
        ///     A numerical identifier for the class.
        /// </param>
        public AbstractBasicDevice()
        {
            UID = uidCounter++;
        }
    }
}
