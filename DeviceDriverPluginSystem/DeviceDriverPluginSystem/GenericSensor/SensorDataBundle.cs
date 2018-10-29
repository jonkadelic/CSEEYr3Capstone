using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeviceDriverPluginSystem.GenericSensor
{
    public class SensorDataBundle : IEnumerable
    {
        /// <summary>
        ///     Collection of data items that the bundle represents.
        /// </summary>
		private List<SensorDataItem> DataItems;

        /// <summary>
        ///     Creates a List of NetworkedSensorDataItem from an IEnumerable of NetworkedSensorDataItem.
        /// </summary>
        public SensorDataBundle(IEnumerable<SensorDataItem> DataItems) 
        {
            this.DataItems = DataItems.ToList();
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
        public SensorDataItem this[string Key]
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
