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
    public class DemonstrationSmartPlug : AbstractBasicDevice
    {
        public override string Label => "Simulated Smart Plug";

        public override string Name => "Simulated Smart Plug";

        public override string Manufacturer => "Joseph Brown";

        public override string Id => "efgh";

        public override bool IsReadOnly => false;

        public readonly IPAddress deviceAddress;

        public DemonstrationSmartPlug(IPAddress address)
        {
            deviceAddress = address;
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceProperty("Powered", GetPlugPowered, SetPlugPowered);
        }

        public void AddDeviceProperty<PropertyType>(string label, Func<PropertyType> getter, Action<PropertyType> setter) where PropertyType : IComparable
        {
            DeviceProperties.Add(new DeviceProperty<PropertyType>(getter, setter, label));
        }

        private bool GetPlugPowered() => DemonstrationSmartPlugDriver.GetDeviceJson(this)["plugPowered"].Value<bool>();

        private void SetPlugPowered(bool value) => DemonstrationSmartPlugDriver.SetDeviceValue(this, "plugPowered", value);

    }
}
