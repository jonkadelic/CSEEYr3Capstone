using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;

namespace DeviceDriver_Environmental
{
    public class Environmental : AbstractBasicDevice
    {
        public override string Label => "Environmental";

        public override string Name => "Environmental";

        public override string Manufacturer => "Internal";

        public override string Id => "9999999";

        public override bool IsReadOnly => true;

        internal Environmental()
        {
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceProperty("Time", GetTime);
            AddDeviceProperty("Temperature", GetTemperature);
            AddDeviceProperty("Humidity", GetHumidity);
            AddDeviceProperty("Pressure", GetPressure);
            AddDeviceProperty("WindSpeed", GetWindSpeed);
            AddDeviceProperty("WindDirection", GetWindDirection);
            AddDeviceProperty("Clouds", GetClouds);
            AddDeviceProperty("Visibility", GetVisibility);
            AddDeviceProperty("Precipitation", GetPrecipitation);
        }

        public void AddDeviceProperty<PropertyType>(string label, Func<PropertyType> getter) where PropertyType : IComparable
        {
            DeviceProperties.Add(new DeviceProperty<PropertyType>(getter, DummySet, label));
        }

        private void DummySet<PropertyType>(PropertyType t)
        {
            return;
        }

        private int GetTime()
        {
            return (int)DateTime.Now.TimeOfDay.TotalMinutes;
        }

        private double GetTemperature()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.temperature;
        }

        private int GetHumidity()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.humidity;
        }

        private double GetPressure()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.pressure;
        }

        private double GetWindSpeed()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.windSpeed;
        }

        private double GetWindDirection()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.windDirection;
        }

        private int GetClouds()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.clouds;
        }

        private int GetVisibility()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.visibility;
        }

        private int GetPrecipitation()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.precipitation;
        }
    }
}
