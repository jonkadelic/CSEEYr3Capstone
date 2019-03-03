using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser.Database
{
    public class Action
    {
        [BsonElement("_id")]
        public int DatabaseID { get; set; }

        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("property_name")]
        public string PropertyName { get; set; }

        [BsonElement("old_value")]
        public dynamic OldValue { get; set; }

        [BsonElement("new_value")]
        public dynamic NewValue { get; set; }
    }
}
