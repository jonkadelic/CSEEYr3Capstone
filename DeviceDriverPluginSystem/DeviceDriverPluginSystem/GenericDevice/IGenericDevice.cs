using System.Collections.Generic;

namespace DeviceDriverPluginSystem.GenericDevice
{
    public interface IGenericDevice
    {
        /// <summary>
        ///     The name of the device.
        /// </summary>
        string Label { get; set; }
    }
}
