using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Property
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public dynamic Value { get; set; }

        public Property(string name, dynamic value)
        {
            Name = name;
            Value = value;
        }
    }
}
