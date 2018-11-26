using DeviceDriverPluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace IoT_Hub
{
    public class HttpRequestListener
    {
        private static List<Driver> drivers;
        private static Thread listenerThread;
        private static bool endThread = false;

        public static void Stop()
        {
            endThread = true;
        }
        public static void Run(List<Driver> driverList)
        {
            drivers = driverList;
            listenerThread = new Thread(Listen);
            Utility.WriteTimeStamp("Created new listener thread", typeof(HttpRequestListener));
            listenerThread.Start();
            Utility.WriteTimeStamp("Started listener thread", typeof(HttpRequestListener));
        }

        private static void Listen()
        {
            HttpListener listener = new HttpListener();
            Utility.WriteTimeStamp("Created new listener", typeof(HttpRequestListener));
            foreach (string s in GenerateListenerPrefixes())
                listener.Prefixes.Add(s);
            Utility.WriteTimeStamp("Added listener prefixes", typeof(HttpRequestListener));
            listener.Start();
            Utility.WriteTimeStamp("Started listener", typeof(HttpRequestListener));
            while (!endThread)
            {
                Utility.WriteTimeStamp("Waiting for request...", typeof(HttpRequestListener));
                HttpListenerContext context = listener.GetContext();
                Utility.WriteTimeStamp("Got request!", typeof(HttpRequestListener));
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = GetResponseString(request);
                Utility.WriteTimeStamp("Built response string", typeof(HttpRequestListener));
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                Utility.WriteTimeStamp("Sent response string!", typeof(HttpRequestListener));
                output.Close();
            }
        }

        private static List<string> GenerateListenerPrefixes()
        {
            List<string> outList = new List<string>();
            string baseUrl = "http://+:80/";
            outList.Add(baseUrl + "all/");
            foreach (Driver d in drivers)
            {
                outList.Add(baseUrl + d.driverId.ToString() + "/");
                foreach (DriverDevice dd in d.Devices)
                {
                    outList.Add(baseUrl + d.driverId.ToString() + "/" + dd.deviceId.ToString() + "/");
                    foreach (DeviceAttribute da in dd.basicDevice.DeviceAttributes)
                    {
                        outList.Add(baseUrl + d.driverId.ToString() + "/" + dd.deviceId.ToString() + "/" + da.Label + "/");
                    }
                }
            }
            return outList;
        }

        private static string GetResponseString(HttpListenerRequest request)
        {
            OutputJsonProducer outputJsonProducer = new OutputJsonProducer(drivers, "IoT Hub");
            string[] urlParts = SplitRawUrlIntoParts(request.RawUrl);
            if (urlParts.Length == 1 && urlParts[0] == "all")
            {
                return outputJsonProducer.GetHubInformation().ToString();
            }
            else if (urlParts.Length >= 1)
            {
                Driver d = null;
                DriverDevice dd = null;
                DeviceAttribute da = null;
                try
                {
                    if (int.TryParse(urlParts[0], out int driverId))
                    {
                        d = drivers.Where(x => x.driverId == driverId).First();
                    }
                    if (urlParts.Length >= 2 && int.TryParse(urlParts[1], out int deviceId))
                    {
                        dd = d.Devices.Where(x => x.deviceId == deviceId).First();
                    }
                    if (urlParts.Length >= 3)
                    {
                        da = dd.basicDevice.DeviceAttributes.Where(x => x.Label == urlParts[2]).First();
                    }
                } catch (Exception)
                {
                    return "Invalid request.";
                }
                if (d != null && dd != null && da == null)
                {
                    return outputJsonProducer.GetDevice(d, dd).ToString();
                }
                else if (d != null && dd != null && da != null)
                {
                    return outputJsonProducer.GetDeviceAttribute(da).ToString();
                }
            }
            return "Invalid request.";
        }

        private static string[] SplitRawUrlIntoParts(string rawUrl)
        {
            return rawUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
