using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Action
    {
        [BsonElement("device_id")]
        public string DeviceID { get; }

        [BsonElement("property_name")]
        public string PropertyName { get; }

        [BsonElement("old_value")]
        public dynamic OldValue { get; }

        [BsonElement("new_value")]
        public dynamic NewValue { get; }

        public Action(string deviceID, string propertyName, dynamic oldValue, dynamic newValue)
        {
            DeviceID = deviceID;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
