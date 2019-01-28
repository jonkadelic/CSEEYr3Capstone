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
        public string Name { get; }

        [BsonElement("value")]
        public dynamic Value { get; }

        public Attribute(string name, dynamic value)
        {
            Name = name;
            Value = value;
        }
    }
}
