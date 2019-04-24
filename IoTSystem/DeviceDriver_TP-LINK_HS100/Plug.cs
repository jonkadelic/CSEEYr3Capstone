using DeviceDriverPluginSystem.Generics;
using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;

namespace DeviceDriver_TP_LINK_HS100
{
    public class Plug : AbstractBasicDevice
    {
        private readonly string ApiID;

        public override string Label
        {
            get
            {
                return PlugDriver.GetJsonByID(ApiID)["alias"].ToString();
            }
        }
   
        public override string Name => "Smart Wi-Fi Plug with Energy Monitoring";

        public override string Manufacturer => "TP-LINK";

        public override string Id => ApiID;

        public override bool IsReadOnly => false;

        internal Plug(string ApiID)
        {
            this.ApiID = ApiID;
            PopulateDeviceProperties();
        }

        private void PopulateDeviceProperties()
        {
            DeviceProperties.Add(new DeviceProperty<bool>(IsPlugOn, SetPlugOn, "Powered"));
        }

        private bool IsPlugOn()
        {
            try
            {
                return (JToken.Parse(PlugDriver.GetDeviceInfoJson(ApiID)["result"]["responseData"].ToString())["system"]["get_sysinfo"]["on_time"].Value<int>()) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetPlugOn(bool newState) =>
            PlugDriver.SetPoweredState(ApiID, newState);

    }
}
