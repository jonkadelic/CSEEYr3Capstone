using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;
using System;
using DeviceDriverPluginSystem;

namespace DeviceDriver_Lifx_Color_A19
{
    public class Lightbulb : AbstractBasicDevice
    {
        /// <summary>
        ///     The Lifx API ID of the device.
        /// </summary>
        private readonly string ApiID;

        /// <summary>
        ///     Label of the device as provided by Lifx JSON.
        ///     Cannot be changed by public API so invoking set method does nothing.
        /// </summary>
        public new string Label
        {
            get
            {
                return LightbulbDriver.GetJsonByID(ApiID)["label"].ToString();
            }
        }

        public new string Name => "Color 1000";

        public new string Manufacturer => "Lifx";

        /// <summary>
        ///     Creates a new Device object with unique ID provided by the JSON data from Lifx.
        /// </summary>
        /// <param name="ApiID">
        ///     The unique Lifx API of the device.
        /// </param>
        internal Lightbulb(string ApiID)
        {
            this.ApiID = ApiID;
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceVariable("Powered", IsBulbOn, SetBulbOn);
            AddDeviceVariable("Hue", GetBulbHue, SetBulbHue, new Range<int>(0, 360));
            AddDeviceVariable("Saturation", GetBulbSaturation, SetBulbSaturation, new Range<double>(0d, 1d));
            AddDeviceVariable("Brightness", GetBulbBrightness, SetBulbBrightness, new Range<double>(0d, 1d));
            AddDeviceVariable("Warmth", GetBulbWarmth, SetBulbWarmth, new Range<int>(1500, 9000));
        }

        public void AddDeviceVariable<VariableType>(string label, Func<VariableType> getter, Action<VariableType> setter) where VariableType : IComparable
        {
            DeviceVariables.Add(new DeviceVariable<VariableType>(getter, setter, label));
        }
        public void AddDeviceVariable<VariableType>(string label, Func<VariableType> getter, Action<VariableType> setter, Range<VariableType> valueRange) where VariableType : IComparable
        {
            DeviceVariables.Add(new DeviceVariableRangeChecked<VariableType>(getter, setter, label, valueRange));
        }

        private bool IsBulbOn() => 
            LightbulbDriver.GetJsonByID(ApiID)["power"].ToString() == "on";
        private int GetBulbHue() => 
            LightbulbDriver.GetJsonByID(ApiID)["color"]["hue"].Value<int>();
        private double GetBulbSaturation() => 
            LightbulbDriver.GetJsonByID(ApiID)["color"]["saturation"].Value<double>();
        private double GetBulbBrightness() =>
            LightbulbDriver.GetJsonByID(ApiID)["brightness"].Value<double>();
        private int GetBulbWarmth() =>
            LightbulbDriver.GetJsonByID(ApiID)["color"]["kelvin"].Value<int>();

        private void SetBulbOn(bool newState) => 
            LightbulbDriver.SetDeviceProperty(ApiID, "power", newState ? "on" : "off");
        private void SetBulbHue(int newHue) =>
            LightbulbDriver.SetDeviceProperty(ApiID, "color", "hue:" + newHue.ToString());
        private void SetBulbSaturation(double newSaturation) =>
            LightbulbDriver.SetDeviceProperty(ApiID, "color", "saturation:" + newSaturation.ToString());
        private void SetBulbBrightness(double newBrightness) =>
            LightbulbDriver.SetDeviceProperty(ApiID, "brightness", newBrightness.ToString());
        private void SetBulbWarmth(int newWarmth) =>
            LightbulbDriver.SetDeviceProperty(ApiID, "color", "warmth:" + newWarmth.ToString());
    }
}
