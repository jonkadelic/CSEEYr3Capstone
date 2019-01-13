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
            Console.ReadLine();
        }
    }
}
