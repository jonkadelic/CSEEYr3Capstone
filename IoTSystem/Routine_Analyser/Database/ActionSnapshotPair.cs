using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser.Database
{
    public class ActionSnapshotPair
    {
        [BsonElement("_id")]
        public int DatabaseID { get; set; }

        [BsonElement("action")]
        public Action Action { get; set; }

        [BsonElement("snapshot")]
        public Snapshot Snapshot { get; set; }
    }
}
