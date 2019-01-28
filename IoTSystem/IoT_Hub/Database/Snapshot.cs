using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class Snapshot
    {
        [BsonElement("devices")]
        public List<Device> Devices { get; }

        [BsonElement("timestamp")]
        [BsonDateTimeOptions]
        public DateTime Timestamp { get; }

        public Snapshot(List<Device> devices)
        {
            Devices = devices;
            Timestamp = DateTime.Now;
        }

        public Snapshot(List<DriverDevice> devices)
        {
            Devices = devices.ConvertAll(x => new Device(x));
            Timestamp = DateTime.Now;
        }
    }
}
