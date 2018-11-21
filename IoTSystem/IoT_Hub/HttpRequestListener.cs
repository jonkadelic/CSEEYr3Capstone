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
            Console.WriteLine(typeof(HttpRequestListener).Name + ": Created new listener thread");
            listenerThread.Start();
            Console.WriteLine(typeof(HttpRequestListener).Name + ": Started listener thread");
        }

        private static void Listen()
        {
            HttpListener listener = new HttpListener();
            Console.WriteLine(typeof(HttpRequestListener).Name + ": Created new listener");
            foreach (string s in GenerateListenerPrefixes())
                listener.Prefixes.Add(s);
            Console.WriteLine(typeof(HttpRequestListener).Name + ": Added listener prefixes");
            listener.Start();
            Console.WriteLine(typeof(HttpRequestListener).Name + ": Started listener");
            while (!endThread)
            {
                Console.WriteLine(typeof(HttpRequestListener).Name + ": Waiting for request...");
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine(typeof(HttpRequestListener).Name + ": Got request!");
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = GetResponseString(request);
                Console.WriteLine(typeof(HttpRequestListener).Name + ": Built response string");
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                Console.WriteLine(typeof(HttpRequestListener).Name + ": Sent response string!");
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
                if (int.TryParse(urlParts[0], out int driverId))
                {
                    d = drivers.Where(x => x.driverId == driverId).First();
                    if (d == null) return "Invalid request.";
                }
                if (urlParts.Length >= 2 && int.TryParse(urlParts[1], out int deviceId))
                {
                    dd = d.Devices.Where(x => x.deviceId == deviceId).First();
                    if (dd == null) return "Invalid request.";
                }
                if (urlParts.Length >= 3)
                {
                    da = dd.basicDevice.DeviceAttributes.Where(x => x.Label == urlParts[2]).First();
                    if (da == null) return "Invalid request.";
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
