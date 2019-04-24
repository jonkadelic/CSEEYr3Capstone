using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Routine
    {
        [BsonElement("routine_name")]
        public string RoutineName { get; set; }

        [BsonElement("_id")]
        public ObjectId RoutineID { get; set; }

        [BsonElement("target_device_id")]
        public string TargetDeviceID { get; set; }

        [BsonElement("target_driver_id")]
        public string TargetDriverID { get; set; }

        [BsonElement("target_property_name")]
        public string TargetProperty { get; set; }

        [BsonElement("target_value")]
        public dynamic TargetValue { get; set; }

        [BsonElement("condition_list")]
        public List<RoutineCondition> RoutineConditions { get; set; }
    }
}
