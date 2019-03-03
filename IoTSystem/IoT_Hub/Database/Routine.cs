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
        [BsonElement("target_device_id")]
        public string TargetDeviceID { get; set; }

        [BsonElement("target_attribute_name")]
        public string TargetAttribute { get; set; }

        [BsonElement("target_value")]
        public dynamic TargetValue { get; set; }

        [BsonElement("condition_list")]
        public List<RoutineCondition> RoutineConditions { get; set; }
    }
}
