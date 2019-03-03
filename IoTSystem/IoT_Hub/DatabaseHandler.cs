using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_Hub
{
    public static class DatabaseHandler
    {
        private static MongoClient client;
        private static IMongoDatabase db_data;
        private static IMongoCollection<Database.ActionSnapshotPair> db_data_collection;
        private static IMongoDatabase db_routines;
        private static IMongoCollection<Database.Routine> db_routines_collection;
        
        public static void Startup(string s)
        {
            client = new MongoClient(s);

            List<string> macAddresses = NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetPhysicalAddress().ToString()).ToList();
            string macAddress = macAddresses.Where(x => x != "").First();

            db_data = client.GetDatabase("data");
            db_data_collection = db_data.GetCollection<Database.ActionSnapshotPair>(macAddress);
            db_routines = client.GetDatabase("routines");
            db_routines_collection = db_routines.GetCollection<Database.Routine>(macAddress);
            Utility.WriteTimeStamp($"Initialised database handler with URL {s}", typeof(DatabaseHandler));
        }

        public static bool IsDatabaseUp => client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;

        public static void BuildActionSnapshotAndInsert(DeviceDriverPluginSystem.DeviceAttribute attribute, DriverDevice device, dynamic oldValue, dynamic newValue)
        {
            Thread t = new Thread(() => _BuildActionSnapshotAndInsert(attribute, device, oldValue, newValue));
            t.Start();
        }

        private static void _BuildActionSnapshotAndInsert(DeviceDriverPluginSystem.DeviceAttribute attribute, DriverDevice device, dynamic oldValue, dynamic newValue)
        {
            Utility.WriteTimeStamp("Building snapshot...", typeof(DatabaseHandler));
            Database.Action action = new Database.Action(device.Id, attribute.Label, oldValue, newValue);
            List<DriverDevice> driverList = new List<DriverDevice>();
            foreach (Driver d in DriverLoader.Drivers)
                driverList.AddRange(d.Devices);
            driverList.Remove(device);
            Database.Snapshot snapshot = new Database.Snapshot(driverList);
            Database.ActionSnapshotPair pair = new Database.ActionSnapshotPair(action, snapshot);
            Utility.WriteTimeStamp("Snapshot built!", typeof(DatabaseHandler));
            Utility.WriteTimeStamp("Sending to MongoDB", typeof(DatabaseHandler));
            db_data_collection.InsertOne(pair);
            Utility.WriteTimeStamp("Sent to MongoDB", typeof(DatabaseHandler));
        }

        public static List<Database.ActionSnapshotPair> RetrieveAllFromCollection()
        {
            List<Database.ActionSnapshotPair> actionSnapshotPairs = db_data_collection.Find(_ => true).ToList();
            return actionSnapshotPairs;
        }

        public static List<Database.Routine> LoadRoutines()
        {
            return db_routines_collection.Find(_ => true).ToList();
        }
    }
}
