using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        Console.WriteLine(r.Get());
                    }
                }
            }
            Console.Read();
        }
    }
}
