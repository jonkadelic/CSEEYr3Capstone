using System;
using System.Collections;
using System.Collections.Generic;

namespace IoT_Hub
{
    public class NetworkedSensorDataBundle : IEnumerable
    {
        /// <summary>
        ///     Collection of data items that the bundle represents.
        /// </summary>
		private List<NetworkedSensorDataItem> DataItems;

        /// <summary>
        ///     Default constructor; instantiates dataItems but leaves it empty.
        /// </summary>
        public NetworkedSensorDataBundle()
        {
            DataItems = new List<NetworkedSensorDataItem>();
        }

        public NetworkedSensorDataItem this[string Key]
        {
            get
            {
                return DataItems.Find(T => T.Key == Key);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return DataItems.GetEnumerator();
        }
    }
}
