using DeviceDriverPluginSystem.GenericDevice;
using DeviceDriverPluginSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub
{
    public class DriverLoader
    {
        public static Dictionary<Type, List<Type>> DriverInterfaces;

        public static void LoadDrivers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "plugins/";
            List<Assembly> assemblies = new List<Assembly>();
            DriverInterfaces = new Dictionary<Type, List<Type>>();
            foreach (string dll in Directory.GetFiles(path, "*.DLL"))
            {
                assemblies.Add(Assembly.LoadFile(dll));
                foreach(Assembly assembly in assemblies)
                {
                    foreach(Type type in assembly.GetExportedTypes())
                    {
                        if (type.GetInterfaces().Contains(typeof(IGenericDevice)))
                        {
                            List<Type> valueInterfaces = type.GetInterfaces().Where(T => T.GetInterfaces().Contains(typeof(IDeviceValue))).ToList();
                            DriverInterfaces.Add(type, valueInterfaces);
                        }
                    }
                }
            }
        }
    }
}
