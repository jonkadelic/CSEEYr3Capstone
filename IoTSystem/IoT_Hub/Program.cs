using System;
using System.Collections.Generic;
using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;

namespace IoT_Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbLocale = "mongodb://127.0.0.1:27017/?gssapiServiceName=mongodb";
            List<string> argsList = new List<string>(args);
            int a = argsList.IndexOf("-d");
            int argsPos = a == -1 ? argsList.IndexOf("--database") : a;
            if (argsPos != -1)
            {
                if (argsList.Count - 1 > argsPos) dbLocale = argsList[argsPos + 1];
            }
            DatabaseHandler.Startup(dbLocale);
            DriverLoader.LoadDrivers();
            ClientBroadcastReceiver.Run();
            HttpRequestListener.Run();
            RoutineScheduler.Run();

            Console.ReadLine();
        }
    }
}
