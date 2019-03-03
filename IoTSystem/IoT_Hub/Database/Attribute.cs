using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Attribute
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public dynamic Value { get; set; }

        public Attribute(string name, dynamic value)
        {
            Name = name;
            Value = value;
        }
    }
}
