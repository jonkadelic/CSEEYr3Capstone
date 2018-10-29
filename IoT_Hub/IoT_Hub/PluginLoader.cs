using DeviceDriverPluginSystem.GenericDevice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub
{
    class PluginLoader
    {
        public void LoadPlugins()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "plugins/";
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }
            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach(string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }
            Type pluginType = typeof(IGenericDevice);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach(Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach(Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }
            ICollection<IGenericDevice> plugins = new List<IGenericDevice>(pluginTypes.Count);
            foreach(Type type in pluginTypes)
            {
                IGenericDevice plugin = (IGenericDevice)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }
        }
    }
}
