using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    class DeviceVariableRangeChecked<VariableType> : DeviceVariable<VariableType> where VariableType : IComparable
    {
        /// <summary>
        ///     The minimum value that the DeviceVariable can be set to.
        /// </summary>
        private readonly VariableType minValue;
        /// <summary>
        ///     The maximum value that the DeviceVariable can be set to.
        /// </summary>
        private readonly VariableType maxValue;

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
        public DeviceVariableRangeChecked(Func<VariableType> get, Action<VariableType> set_NoRangeCheck, string label, VariableType minValue, VariableType maxValue) : base(get, set_NoRangeCheck, label)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
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
            if (IsInValueRange(value))
            {
                base.Set(value);
                return true;
            }
            else return false;
        }

        /// <summary>
        ///     Checks if a value is in the range specified by minValue and maxValue (inclusive).
        /// </summary>
        /// <param name="value">
        ///     The value to be checked.
        /// </param>
        /// <returns>
        ///     Result of comparison - true if in range and false if outside.
        /// </returns>
        private bool IsInValueRange(VariableType value)
        {
            return value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0;
        }
    }
}
