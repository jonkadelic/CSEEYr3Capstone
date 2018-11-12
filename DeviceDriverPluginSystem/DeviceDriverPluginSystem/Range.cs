using System;

namespace DeviceDriverPluginSystem
{
    public class Range<ValueType> where ValueType : IComparable
    {
        public readonly ValueType minValue;
        public readonly ValueType maxValue;

        public Range(ValueType minValue, ValueType maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public bool Contains(ValueType value) => value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0;
    }
}