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
            foreach (var s in DriverLoader.Drivers)
            {
                s.InvokeMember("RefreshDeviceList", System.Reflection.BindingFlags.InvokeMethod, null, null, null);
                foreach (var q in s.BaseType.GetField("Devices").GetValue(null) as List<GenericDevice>)
                {
                    foreach (var r in q.DeviceValues)
                    {
                        Type valueType = typeof(DeviceValue<>).MakeGenericType(r.ValueType);
                        dynamic devVal = Convert.ChangeType(r, valueType);
                        Console.WriteLine(devVal.Get());
                    }
                }
            }
            Console.Read();
        }
    }
}
