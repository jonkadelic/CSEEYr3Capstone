using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DevicePropertyRangeChecked<PropertyType> : DeviceProperty<PropertyType> where PropertyType : IComparable
    {
        public Range<PropertyType> propertyRange;

        /// <summary>
        ///     Creates a new instance of DeviceProperty using range checking.
        /// </summary>
        /// <param name="get">
        ///     Delegate with return type T that fetches the appropriate data from the IoT device.
        /// </param>
        /// <param name="set_NoRangeCheck">
        ///     Delegate with return type bool and parameter of type PropertyType that sets the device property value to that parameter, without range checking.
        /// </param>
        /// <param name="label">
        ///     The label of the device property. Should be unique.
        /// </param>
        /// <param name="propertyRange">
        ///     The range of values the property can take.
        /// </param>
        public DevicePropertyRangeChecked(Func<PropertyType> get, Action<PropertyType> set_NoRangeCheck, string label, Range<PropertyType> propertyRange) : base(get, set_NoRangeCheck, label)
        {
            this.propertyRange = propertyRange;
            IsInputRangeChecked = true;
        }

        /// <summary>
        ///     Sets the value of the variable DeviceProperty represents, but only if the new value is within the range of minValue and maxValue.
        ///     Returns true if the value is within range and false otherwise.
        /// </summary>
        /// <param name="value">
        ///     The value to set the variable DeviceProperty represents to.
        /// </param>
        /// <returns>
        ///     Bool to determine if value is within range.
        /// </returns>
        public new bool Set(PropertyType value)
        {
            if (propertyRange.Contains(value))
            {
                base.Set(value);
                return true;
            }
            else return false;
        }
    }
}
