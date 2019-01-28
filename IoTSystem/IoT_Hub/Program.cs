using System;
using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;

namespace IoT_Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            DriverLoader.LoadDrivers();
            ClientBroadcastReceiver.Run();
            HttpRequestListener.Run();
            //if (DatabaseHandler.IsDatabaseUp)
            //    Utility.WriteTimeStamp("Successfully connected to database.", typeof(DatabaseHandler));
            //else
            //    Utility.WriteTimeStamp("Could not connect to database!", typeof(DatabaseHandler));

            Console.ReadLine();
        }
    }
}
