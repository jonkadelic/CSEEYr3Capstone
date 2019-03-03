using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class RoutineCondition
    {
        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("attribute_name")]
        public string AttributeName { get; set; }

        [BsonElement("desired_value")]
        public dynamic DesiredValue { get; set; }
    }
}
