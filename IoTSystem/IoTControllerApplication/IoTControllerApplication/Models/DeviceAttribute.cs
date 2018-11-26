using System;
using System.Collections.Generic;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class DeviceAttribute
    {
        public string Label { get; private set; }
        public Type AttributeType { get; private set; }
        public string Value { get; private set; }

        public DeviceAttribute(string label, Type attributeType, string value)
        {
            Label = label;
            AttributeType = attributeType;
            Value = value;
        }
    }
}
