using DeviceDriverPluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            listenerThread.Start();
        }

        private static void Listen()
        {
            HttpListener listener = new HttpListener();
            foreach (string s in GenerateListenerPrefixes())
                listener.Prefixes.Add(s);
            listener.Start();
            while (!endThread)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = GetResponseString(request);
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        private static List<string> GenerateListenerPrefixes()
        {
            List<string> outList = new List<string>();
            string baseUrl = "http://+:8080/";
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
            string[] urlParts = SplitRawUrlIntoParts(request.RawUrl);
            if (urlParts.Length == 1 && urlParts[0] == "all")
            {
                OutputJsonProducer outputJsonProducer = new OutputJsonProducer(drivers, "IoT Hub");
                return outputJsonProducer.GetHubInformation();
            }
            else return "Invalid request.";
        }

        private static string[] SplitRawUrlIntoParts(string rawUrl)
        {
            return rawUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
