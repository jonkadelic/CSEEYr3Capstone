using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceValue<T> : DeviceValue
    {
        public Func<T> Get;
        public Func<T, bool> Set;
        public readonly T minValue;
        public readonly T maxValue;

        public DeviceValue(Func<T> get, Func<T, bool> set, string label, T minValue, T maxValue) : base(typeof(T), label)
        {
            Get = get;
            Set = set;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }

    public class DeviceValue
    {
        public Type ValueType;
        public string Label { get; }

        public DeviceValue(Type type, string label)
        {
            ValueType = type;
            Label = label;
        }
    }
}
