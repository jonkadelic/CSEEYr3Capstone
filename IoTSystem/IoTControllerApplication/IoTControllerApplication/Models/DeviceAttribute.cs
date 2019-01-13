using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class DeviceAttribute
    {
        public string Label { get; private set; }
        public Type AttributeType { get; private set; }
        public dynamic MinValue { get; set; } = null;
        public dynamic MaxValue { get; set; } = null;
        public IoTDevice Device { get; }

        private dynamic _value;


        public void UpdateValue()
        {
            JToken att;
            using (WebClient client = new WebClient())
            {
                string s = client.DownloadString($"{Services.IoTDataStore.HttpUrl}/{Device.DriverId}/{Device.DeviceId}/{Label}/");
                att = JToken.Parse(s);
            }
            string value = att["value"].Value<string>();
            try
            {
                _value = Convert.ChangeType(value, AttributeType);
            }
            catch (InvalidCastException)
            {
                _value = "ERROR: Attribute is of an unsupported type.";
            }
            catch (Exception)
            {
                _value = "ERROR: Attribute value could not be parsed.";
            }
        }

        public dynamic Value {
            get
            {
                return _value;
            }
            set
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString($"{Services.IoTDataStore.HttpUrl}/{Device.DriverId}/{Device.DeviceId}/{Label}/set?v={value.ToString()}");
                }
            }
        }

        public DeviceAttribute(string label, Type attributeType, IoTDevice device, string minValue = "", string maxValue = "")
        {
            Label = label;
            AttributeType = attributeType;
            Device = device;
            if (minValue != "" && maxValue != "")
            {
                MinValue = minValue;
                MaxValue = maxValue;
            }
            UpdateValue();
        }
    }
}
