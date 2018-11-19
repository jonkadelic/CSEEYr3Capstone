﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem
{
    public class DeviceVariable
    {
        /// <summary>
        ///     The type of value DeviceVariable represents; used for conversion to DeviceVariable<VariableType>.
        /// </summary>
        public Type VariableType;
        /// <summary>
        ///     Variable name for printing, e.g. "Powered", "Brightness".
        /// </summary>
        public string Label { get; }

        /// <summary>
        ///     Creates a new instance of DeviceVariable. Only called when invoking constructor for DeviceVariable<VariableType>.
        /// </summary>
        /// <param name="type">
        ///     The type of value DeviceVariable represents.
        /// </param>
        /// <param name="label">
        ///     Name to uniquely identify the variable.
        /// </param>
        protected DeviceVariable(Type type, string label)
        {
            VariableType = type;
            Label = label;
        }
    }

    public class DeviceVariable<VariableType> : DeviceVariable where VariableType : IComparable
    {
        /// <summary>
        ///     Function to get the value of the variable DeviceVariable represents from the device.
        /// </summary>
        public Func<VariableType> Get;
        /// <summary>
        ///     Function to set the value of the variable DeviceVariable represents.
        /// </summary>
        public Action<VariableType> Set;
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
        public DeviceVariable(Func<VariableType> get, Action<VariableType> set, string label) : base(typeof(VariableType), label)
        {
            Get = get;
            Set = set;
            IsInputRangeChecked = false;
        }
    }
}
