﻿using DeviceDriverPluginSystem.Generics;
using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;

namespace DeviceDriver_TP_LINK_HS100
{
    public class Plug : AbstractBasicDevice
    {
        private readonly string ApiID;

        public new string Label
        {
            get
            {
                return PlugDriver.GetJsonByID(ApiID)["alias"].ToString();
            }
        }

        internal Plug(string ApiID)
        {
            this.ApiID = ApiID;
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            DeviceVariables.Add(new DeviceVariable<bool>(IsPlugOn, SetPlugOn, "Powered"));
        }

        private bool IsPlugOn()
        {
            return (JToken.Parse(PlugDriver.GetJsonDeviceInfo(ApiID)["result"]["responseData"].ToString())["system"]["get_sysinfo"]["on_time"].Value<int>()) > 0;
        }

        private void SetPlugOn(bool newState) =>
            PlugDriver.SetPoweredState(ApiID, newState);

    }
}
