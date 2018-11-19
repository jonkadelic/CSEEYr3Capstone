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
            foreach (var s in DriverLoader.Drivers)
            {
                foreach (var q in s.Devices)
                {
                    foreach (dynamic r in q.basicDevice.DeviceVariables)
                    {
                        try
                        {
                            Console.WriteLine(r.Get());
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            Console.ReadLine();
            OutputJsonProducer json = new OutputJsonProducer();
            Console.WriteLine(json.GetOutputJsonString(DriverLoader.Drivers, "IoT Hub"));
            Console.ReadLine();
        }
    }
}
