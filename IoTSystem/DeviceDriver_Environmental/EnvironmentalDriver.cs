using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Timers;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml;

namespace DeviceDriver_Environmental
{
    public class EnvironmentalDriver : AbstractBasicDriver
    {
        private static readonly string AppId = "545ad309c4f5b24bf64d10d1fb12f13b";
        private static string HttpUrl => $"http://api.openweathermap.org/data/2.5/weather?mode=xml&APPID={AppId}&lat={LocationProvider.Latitude}&lon={LocationProvider.Longitude}";

        private static DateTime lastReloadTime = new DateTime(0);

        private static Environmental internalDevice;

        private static readonly HttpClient client = new HttpClient();

        public static double temperature;
        public static int humidity;
        public static double pressure;
        public static double windSpeed;
        public static double windDirection;
        public static int clouds;
        public static int visibility;
        public static int precipitation;

        public static new void Initialize()
        {
            internalDevice = new Environmental();
        }

        public static new Type DeviceType => typeof(Environmental);

        public static new List<AbstractBasicDevice> Devices
        {
            get
            {
                return new List<AbstractBasicDevice> { internalDevice };
            }
        }

        public static void ReloadData()
        {
            if (lastReloadTime.Add(new TimeSpan(0, 15, 0)) > DateTime.Now) return;
            LocationProvider.ReloadLocation();
            string xmlDoc;
            lastReloadTime = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            client.DefaultRequestHeaders.Clear();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(HttpUrl),
                Method = HttpMethod.Get
            };

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                xmlDoc = response.Content.ReadAsStringAsync().Result;
                xml.LoadXml(xmlDoc);
            }

            XmlNode root = xml["current"];
            try
            {
                temperature = double.Parse(root["temperature"].Attributes["value"].Value);
            }
            catch (Exception)
            {
                temperature = -1;
            }
            try
            {
                humidity = int.Parse(root["humidity"].Attributes["value"].Value);
            }
            catch (Exception)
            {
                humidity = -1;
            }
            try
            {
                pressure = double.Parse(root["pressure"].Attributes["value"].Value);
            }
            catch (Exception)
            {
                humidity = -1;
            }
            try
            {
                windSpeed = double.Parse(root["wind"].ChildNodes[0].Attributes["value"].Value);
            }
            catch (Exception)
            {
                windSpeed = -1;
            }
            try
            {
                windDirection = double.Parse(root["wind"].ChildNodes[2].Attributes["value"].Value);
            }
            catch (Exception)
            {
                windDirection = -1;
            }
            try
            {
                clouds = int.Parse(root["clouds"].Attributes["value"].Value);
            }
            catch (Exception)
            {
                clouds = -1;
            }
            try
            {
                visibility = int.Parse(root["visibility"].Attributes["value"].Value);
            }
            catch (Exception)
            {
                visibility = -1;
            }
            try
            {
                string prec = root["precipitation"].Attributes["mode"].Value;
                precipitation = (prec == "no" ? 0 : prec == "rain" ? 1 : 2);
            }
            catch (Exception)
            {
                precipitation = -1;
            }
        }
    }
}
