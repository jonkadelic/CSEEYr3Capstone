using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class DeviceAttribute
    {
        public string Label { get; private set; }
        public Type AttributeType { get; private set; }
        public dynamic Value { get; set; }
        public dynamic MinValue { get; set; } = null;
        public dynamic MaxValue { get; set; } = null;

        public DeviceAttribute(string label, Type attributeType, string value, string minValue = "", string maxValue = "")
        {
            Label = label;
            AttributeType = attributeType;
            try
            {
                Value = Convert.ChangeType(value, AttributeType);
            }
            catch (InvalidCastException)
            {
                Value = "ERROR: Attribute is of an unsupported type.";
            }
            catch (Exception)
            {
                Value = "ERROR: Attribute value could not be parsed.";
            }
            if (minValue != "" && maxValue != "")
            {
                try
                {
                    MinValue = Convert.ChangeType(minValue, AttributeType);
                    MaxValue = Convert.ChangeType(maxValue, AttributeType);
                }
                catch
                {
                    MinValue = null;
                    MaxValue = null;
                }
            }
        }
    }
}
