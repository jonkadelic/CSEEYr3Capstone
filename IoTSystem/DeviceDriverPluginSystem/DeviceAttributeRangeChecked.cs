using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceAttributeRangeChecked<AttributeType> : DeviceAttribute<AttributeType> where AttributeType : IComparable
    {
        private Range<AttributeType> attributeRange;

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
        /// <param name="attributeRange">
        ///     The range of values the attribute can take.
        /// </param>
        public DeviceAttributeRangeChecked(Func<AttributeType> get, Action<AttributeType> set_NoRangeCheck, string label, Range<AttributeType> attributeRange) : base(get, set_NoRangeCheck, label)
        {
            this.attributeRange = attributeRange;
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
        public new bool Set(AttributeType value)
        {
            if (attributeRange.Contains(value))
            {
                base.Set(value);
                return true;
            }
            else return false;
        }
    }
}
