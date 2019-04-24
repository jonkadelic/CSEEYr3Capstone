using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace IoT_Hub.Database
{
    public class Device
    {
        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("properties")]
        public List<Property> Properties { get; set; }

        public Device(string deviceId, List<Property> properties)
        {
            DeviceID = deviceId;
            Properties = properties;
        }

        public Device(DriverDevice device)
        {
            Properties = new List<Property>();
            DeviceID = device.Id;
            foreach(DeviceDriverPluginSystem.DeviceProperty property in device.GetDynamicDevice().DeviceProperties)
            {
                dynamic prop = property;
                Property a = new Property(property.Label, prop.Get());
                Properties.Add(a);
            }
        }
    }
}
