using System;
using System.Collections.Generic;

namespace IoT_Hub
{
	/// <summary>
    /// Interface for a networked sensor to implement
    /// </summary>
    public interface INetworkedSensor
    {
		NetworkedSensorDataBundle FetchData();
    }
}
