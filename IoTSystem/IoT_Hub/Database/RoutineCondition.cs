using MongoDB.Bson;
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
        public enum COMPARISON
        {
            EQUAL,
            LESS,
            GREATER
        }

        [BsonElement("_id")]
        public ObjectId RoutineConditionID { get; set; }

        [BsonElement("driver_id")]
        public string DriverID { get; set; }

        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("property_name")]
        public string PropertyName { get; set; }

        [BsonElement("desired_value")]
        public dynamic DesiredValue { get; set; }

        [BsonElement("comparison")]
        public COMPARISON Comparison { get; set; }
    }
}
