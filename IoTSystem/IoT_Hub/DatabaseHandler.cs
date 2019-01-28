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
        private static IMongoDatabase database;
        private static IMongoCollection<Database.ActionSnapshotPair> collection;
        
        static DatabaseHandler()
        {
            client = new MongoClient("mongodb+srv://admin:admin@joeiotcluster-57die.gcp.mongodb.net/test?retryWrites=true");

            string macAddress = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
            database = client.GetDatabase("data");
            collection = database.GetCollection<Database.ActionSnapshotPair>(macAddress);
            Utility.WriteTimeStamp("Initialised database handler", typeof(DatabaseHandler));
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
            collection.InsertOne(pair);
        }

        public static List<Database.ActionSnapshotPair> RetrieveAllFromCollection()
        {
            List<Database.ActionSnapshotPair> actionSnapshotPairs = collection.Find(_ => true).ToList();
            return actionSnapshotPairs;
        }
    }
}
