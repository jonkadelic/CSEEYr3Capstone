using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDriver_DemonstrationVirtualDevice
{
    public class DemonstrationDevice : AbstractBasicDevice
    {
        public override string Label => "Demonstration Device";

        public override string Name => "Demonstration Device";

        public override string Manufacturer => "Internal";

        public override string Id => "aaaaaaaaaa";

        public override bool IsReadOnly => true;

        public readonly IPAddress deviceAddress;

        public DemonstrationDevice(IPAddress address)
        {
            deviceAddress = address;
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceProperty("Powered", GetPowered);
            AddDeviceProperty("PowerLevel", GetPowerLevel);
        }

        public void AddDeviceProperty<PropertyType>(string label, Func<PropertyType> getter) where PropertyType : IComparable
        {
            DeviceProperties.Add(new DeviceProperty<PropertyType>(getter, DummySet, label));
        }

        private void DummySet<PropertyType>(PropertyType t)
        {
            return;
        }

        private bool GetPowered() => DemonstrationDeviceDriver.GetDeviceJson(this)["powerState"].Value<bool>();

        private int GetPowerLevel() => DemonstrationDeviceDriver.GetDeviceJson(this)["powerLevel"].Value<int>();
    }
}
