using DeviceDriverPluginSystem.GenericDevice;
using System;
using System.Collections.Generic;

namespace DeviceDriverPluginSystem.GenericSensor
{
	/// <summary>
    ///     Interface that represents a device that is capable of reading data about its environment and sending it to the IoT hub.
    /// </summary>
    public interface IGenericSensor : IGenericDevice
    {
        /// <summary>
        ///     Fetches data from the device the class implementing INetworkedSensor represents.
        /// </summary>
        /// <returns>
        ///     Data from the device as a NetworkedSensorDataBundle.
        /// </returns>
		SensorDataBundle FetchData();
    }
}
