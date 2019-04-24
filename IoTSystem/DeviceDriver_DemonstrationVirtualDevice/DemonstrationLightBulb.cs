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
    public class DemonstrationLightBulb : AbstractBasicDevice
    {
        public override string Label => "Simulated Light Bulb";

        public override string Name => "Simulated Light Bulb";

        public override string Manufacturer => "Joseph Brown";

        public override string Id => "abcd";

        public override bool IsReadOnly => false;

        public readonly IPAddress deviceAddress;

        public DemonstrationLightBulb(IPAddress address)
        {
            deviceAddress = address;
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceProperty("Powered", GetBulbPowered, SetBulbPowered);
            AddDeviceProperty("Red", GetBulbR, SetBulbR, new Range<int>(0, 255));
            AddDeviceProperty("Green", GetBulbG, SetBulbG, new Range<int>(0, 255));
            AddDeviceProperty("Blue", GetBulbB, SetBulbB, new Range<int>(0, 255));
        }

        internal void AddDeviceProperty<VariableType>(string label, Func<VariableType> getter, Action<VariableType> setter) where VariableType : IComparable
        {
            DeviceProperties.Add(new DeviceProperty<VariableType>(getter, setter, label));
        }
        internal void AddDeviceProperty<VariableType>(string label, Func<VariableType> getter, Action<VariableType> setter, Range<VariableType> valueRange) where VariableType : IComparable
        {
            DeviceProperties.Add(new DevicePropertyRangeChecked<VariableType>(getter, setter, label, valueRange));
        }

        private bool GetBulbPowered() => DemonstrationLightBulbDriver.GetDeviceJson(this)["bulbPowered"].Value<bool>();
        private int GetBulbR() => DemonstrationLightBulbDriver.GetDeviceJson(this)["bulbR"].Value<int>();
        private int GetBulbG() => DemonstrationLightBulbDriver.GetDeviceJson(this)["bulbG"].Value<int>();
        private int GetBulbB() => DemonstrationLightBulbDriver.GetDeviceJson(this)["bulbB"].Value<int>();

        private void SetBulbPowered(bool value) => DemonstrationLightBulbDriver.SetDeviceValue(this, "bulbPowered", value);
        private void SetBulbR(int value) => DemonstrationLightBulbDriver.SetDeviceValue(this, "bulbR", value);
        private void SetBulbG(int value) => DemonstrationLightBulbDriver.SetDeviceValue(this, "bulbG", value);
        private void SetBulbB(int value) => DemonstrationLightBulbDriver.SetDeviceValue(this, "bulbB", value);

    }
}
