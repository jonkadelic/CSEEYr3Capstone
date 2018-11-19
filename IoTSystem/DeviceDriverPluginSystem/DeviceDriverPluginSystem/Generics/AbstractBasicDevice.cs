using System.Collections.Generic;

namespace DeviceDriverPluginSystem.Generics
{
    public abstract class AbstractBasicDevice
    {
        /// <summary>
        ///     The name of the device.
        /// </summary>
        public string Label { get; set; }

        public string Name { get; }

        public string Manufacturer { get; }

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
    }
}
