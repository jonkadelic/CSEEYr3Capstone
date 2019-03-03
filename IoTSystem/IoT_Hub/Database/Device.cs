using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Device
    {
        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("attributes")]
        public List<Attribute> Attributes { get; set; }

        public Device(string deviceId, List<Attribute> attributes)
        {
            DeviceID = deviceId;
            Attributes = attributes;
        }

        public Device(DriverDevice device)
        {
            Attributes = new List<Attribute>();
            DeviceID = device.Id;
            foreach(DeviceDriverPluginSystem.DeviceAttribute attribute in device.GetDynamicDevice().DeviceAttributes)
            {
                dynamic att = attribute;
                Attribute a = new Attribute(attribute.Label, att.Get());
                Attributes.Add(a);
            }
        }
    }
}
