using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.GenericDevice;
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
            foreach (var s in DriverLoader.DriverInterfaces)
            {
                Console.WriteLine(s.Key.Name);
                foreach (string t in s.Key.GetMethods().Select(T => T.Name))
                {
                    Console.WriteLine(t);
                }
                dynamic q = s.Key.GetMethod("GetDevices").Invoke(null, null);
                Console.WriteLine(q.Count);
                foreach (dynamic gd in q)
                {
                    foreach (var r in s.Value)
                    {
                        Console.WriteLine(r.InvokeMember("get_Value", System.Reflection.BindingFlags.InvokeMethod, null, gd, null));
                    }
                }
            }
            Console.Read();
        }
    }
}
