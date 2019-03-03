using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser.Database
{
    public class Attribute
    {
        [BsonElement("_id")]
        public int DatabaseID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public dynamic Value { get; set; }
    }
}
