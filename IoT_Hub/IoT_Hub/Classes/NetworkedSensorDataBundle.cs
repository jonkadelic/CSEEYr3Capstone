using System;
using System.Collections;
using System.Collections.Generic;

namespace IoT_Hub
{
    public class NetworkedSensorDataBundle : IEnumerable, DeviceDriverPluginSystem.IDeviceDriver
    {
        /// <summary>
        ///     Collection of data items that the bundle represents.
        /// </summary>
		private List<NetworkedSensorDataItem> DataItems;

        /// <summary>
        ///     Default constructor; instantiates DataItems but leaves it empty.
        /// </summary>
        public NetworkedSensorDataBundle()
        {
            DataItems = new List<NetworkedSensorDataItem>();
        }

        /// <summary>
        ///     Accesses a member of the dataset.
        /// </summary>
        /// <param name="Key">
        ///     The Key of the NetworkedSensorDataItem to be found.
        /// </param>
        /// <returns>
        ///     The NetworkedSensorDataItem that has a Key equal to Key.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if Key is not present in the list of NetworkedSensorDataItem.
        /// </exception>
        public NetworkedSensorDataItem this[string Key]
        {
            get
            {
                try
                {
                    int index = DataItems.FindIndex(T => T.Key == Key);
                    if (index == -1)
                        throw new ArgumentOutOfRangeException();
                    else
                        return DataItems[index];
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///     Fetches the enumerator of the DataItems list.
        /// </summary>
        /// <returns>
        ///     DataItems' enumerator.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return DataItems.GetEnumerator();
        }
    }
}
