using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser.Database
{
    public class Device
    {
        [BsonElement("_id")]
        public int DatabaseID { get; set; }

        [BsonElement("device_id")]
        public string DeviceID { get; set; }

        [BsonElement("attributes")]
        public List<Attribute> Attributes { get; set; }
    }
}
