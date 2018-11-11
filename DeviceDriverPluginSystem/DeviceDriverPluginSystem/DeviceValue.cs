using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceValue<T> : DeviceValue
    {
        public Func<T> Get;
        public Func<bool, T> Set;
        public string Label { get; }

        public DeviceValue(Func<T> get, Func<bool, T> set, string label) : base(typeof(T))
        {
            Get = get;
            Set = set;
            Label = label;
        }
    }

    public class DeviceValue
    {
        public Type ValueType;

        public DeviceValue(Type type)
        {
            ValueType = type;
        }
    }
}
