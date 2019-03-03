using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser
{
    public static class DatabaseHandler
    {
        private static MongoClient client;
        private static IMongoDatabase database;

        public static IMongoCollection<Database.ActionSnapshotPair> Collection { get; private set; }

        public static void Startup(string s)
        {
            client = new MongoClient(s);

            List<string> macAddresses = NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetPhysicalAddress().ToString()).ToList();
            string macAddress = macAddresses.Where(x => x != "").First();

            database = client.GetDatabase("data");
            Collection = database.GetCollection<Database.ActionSnapshotPair>(macAddress);
        }

        public static double[][] GetObservation()
        {
            List<double[]> l = new List<double[]>();
            var temp = Collection.AsQueryable().Select(x => x.Snapshot.Devices).ToList();
            List<List<dynamic>> valueList = temp.Select(x => x.SelectMany(y => y.Attributes.Select(z => z.Value)).ToList()).ToList();
            foreach (List<dynamic> values in valueList)
            {
                List<double> doubles = new List<double>();
                foreach(dynamic d in values)
                {
                    if (d is bool)
                        doubles.Add(d ? 1.0 : 0.0);
                    else if (d is int)
                        doubles.Add((double)d);
                    else if (d is double)
                        doubles.Add(d);
                }
                l.Add(doubles.ToArray());
            }
            return l.ToArray();
        }
    }
}
