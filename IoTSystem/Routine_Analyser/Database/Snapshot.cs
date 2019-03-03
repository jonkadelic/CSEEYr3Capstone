using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser.Database
{
    public class Snapshot
    {
        [BsonElement("_id")]
        public int DatabaseID { get; set; }

        [BsonElement("devices")]
        public List<Device> Devices { get; set; }

        [BsonElement("timestamp")]
        [BsonDateTimeOptions]
        public DateTime Timestamp { get; set; }
    }
}
