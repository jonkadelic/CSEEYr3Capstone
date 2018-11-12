using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceVariableRangeChecked<VariableType> : DeviceVariable<VariableType> where VariableType : IComparable
    {
        private Range<VariableType> ValueRange;

        /// <summary>
        ///     Creates a new instance of DeviceVariable using range checking.
        /// </summary>
        /// <param name="get">
        ///     Delegate with return type T that fetches the appropriate data from the IoT device.
        /// </param>
        /// <param name="set_NoRangeCheck">
        ///     Delegate with return type bool and parameter of type VariableType that sets the device variable value to that parameter, without range checking.
        /// </param>
        /// <param name="label">
        ///     The label of the device variable. Should be unique.
        /// </param>
        /// <param name="minValue">
        ///     The maximum value the variable can take.
        /// </param>
        /// <param name="maxValue">
        ///     The minimum value the variable can take.
        /// </param>
        public DeviceVariableRangeChecked(Func<VariableType> get, Action<VariableType> set_NoRangeCheck, string label, Range<VariableType> valueRange) : base(get, set_NoRangeCheck, label)
        {
            ValueRange = valueRange;
            IsInputRangeChecked = true;
        }

        /// <summary>
        ///     Sets the value of the variable DeviceVariable represents, but only if the new value is within the range of minValue and maxValue.
        ///     Returns true if the value is within range and false otherwise.
        /// </summary>
        /// <param name="value">
        ///     The value to set the variable DeviceVariable represents to.
        /// </param>
        /// <returns>
        ///     Bool to determine if value is within range.
        /// </returns>
        public new bool Set(VariableType value)
        {
            if (ValueRange.Contains(value))
            {
                base.Set(value);
                return true;
            }
            else return false;
        }
    }
}
