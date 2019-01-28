using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub.Database
{
    public class ActionSnapshotPair
    {
        [BsonElement("action")]
        public Action Action { get; }

        [BsonElement("snapshot")]
        public Snapshot Snapshot { get; }

        public ActionSnapshotPair(Action action, Snapshot snapshot)
        {
            Action = action;
            Snapshot = snapshot;
        }
    }
}
