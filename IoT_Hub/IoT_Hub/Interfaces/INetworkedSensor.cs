using System;
using System.Collections.Generic;

namespace IoT_Hub
{
	/// <summary>
    ///     Interface that represents a device that is capable of reading data about its environment and sending it to the IoT hub.
    /// </summary>
    public interface INetworkedSensor
    {
        /// <summary>
        ///     Fetches data from the device the class implementing INetworkedSensor represents.
        /// </summary>
        /// <returns>
        ///     Data from the device.
        /// </returns>
		NetworkedSensorDataBundle FetchData();
    }
}
