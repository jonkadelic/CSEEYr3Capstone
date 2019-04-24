using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceProperty
    {
        /// <summary>
        ///     The type of value DeviceProperty represents; used for conversion to DeviceProperty<PropertyType>.
        /// </summary>
        public Type PropertyType;
        /// <summary>
        ///     Variable name for printing, e.g. "Powered", "Brightness".
        /// </summary>
        public string Label { get; }

        /// <summary>
        ///     Creates a new instance of DeviceProperty. Only called when invoking constructor for DeviceProperty<PropertyType>.
        /// </summary>
        /// <param name="type">
        ///     The type of value DeviceProperty represents.
        /// </param>
        /// <param name="label">
        ///     Name to uniquely identify the property.
        /// </param>
        protected DeviceProperty(Type type, string label)
        {
            PropertyType = type;
            Label = label;
        }
    }

    public class DeviceProperty<PropertyType> : DeviceProperty where PropertyType : IComparable
    {
        /// <summary>
        ///     Function to get the value of the variable DeviceVariable represents from the device.
        /// </summary>
        private Func<PropertyType> _Get;

        private PropertyType cached;
        private DateTime lastRequest = DateTime.Now;

        public PropertyType Get()
        {
            if (lastRequest.Add(new TimeSpan(0, 0, 30)) < DateTime.Now)
                return _Get();
            else
                return cached;
        }
        /// <summary>
        ///     Function to set the value of the variable DeviceVariable represents.
        /// </summary>
        public Action<PropertyType> Set;
        /// <summary>
        ///     Flag to determine if the variable should be range checked.
        /// </summary>
        public bool IsInputRangeChecked { get; protected set; }

        /// <summary>
        ///     Creates a new instance of DeviceVariable that does not use range checking.
        /// </summary>
        /// <param name="get">
        ///     Delegate that gets the value of the variable DeviceVariable represents from the IoT device.
        /// </param>
        /// <param name="set">
        ///     Delegate that sets the value of the variable DeviceVariable represents.
        /// </param>
        /// <param name="label">
        ///     Label to identify the DeviceVariable. Should be unique.
        /// </param>
        public DeviceProperty(Func<PropertyType> get, Action<PropertyType> set, string label) : base(typeof(PropertyType), label)
        {
            _Get = get;
            Set = set;
            cached = _Get();
            IsInputRangeChecked = false;
        }


    }
}
