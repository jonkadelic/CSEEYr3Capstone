using System.Collections.Generic;

namespace DeviceDriverPluginSystem.Generics
{
    public abstract class AbstractBasicDevice
    {
        /// <summary>
        ///     The name of the device, set by the user.
        /// </summary>
        public abstract string Label { get; }

        /// <summary>
        ///     The name of the product.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     The product manufacturer.
        /// </summary>
        public abstract string Manufacturer { get; }

        /// <summary>
        ///     The device's ID.
        /// </summary>
        public abstract string Id { get; }

        public abstract bool IsReadOnly { get; }

        /// <summary>
        ///     List of all the DeviceVariable instances associated with the device.
        /// </summary>
        public List<DeviceProperty> DeviceProperties { get; } = new List<DeviceProperty>();

        /// <summary>
        ///     Retrieve a DeviceVariable object from DeviceValues by label.
        /// </summary>
        /// <param name="label">
        ///     The name of the DeviceVariable to be found; equivalent to its Label field.
        /// </param>
        /// <returns>
        ///     The DeviceVariable whose label is equal to the supplied value.
        /// </returns>
        public DeviceProperty this[string label] => DeviceProperties.Find(x => x.Label == label);
    }
}
