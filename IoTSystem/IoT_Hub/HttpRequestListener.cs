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
        private static Thread listenerThread;
        private static bool endThread = false;
        private static HttpListener listener;

        static HttpRequestListener()
        {
            listener = new HttpListener();
        }

        public static void Stop()
        {
            endThread = true;
        }
        public static void Run()
        {
            listenerThread = new Thread(Listen);
            Utility.WriteTimeStamp("Created new listener thread", typeof(HttpRequestListener));
            listenerThread.Start();
            Utility.WriteTimeStamp("Started listener thread", typeof(HttpRequestListener));
        }

        public static void UpdateListenerPrefixes()
        {
            listener.Prefixes.Clear();
            foreach (string s in GenerateListenerPrefixList())
                listener.Prefixes.Add(s);
        }

        private static void Listen()
        {
            Utility.WriteTimeStamp("Created new listener", typeof(HttpRequestListener));
            UpdateListenerPrefixes();
            Utility.WriteTimeStamp("Added listener prefixes", typeof(HttpRequestListener));
            listener.Start();
            Utility.WriteTimeStamp("Started listener", typeof(HttpRequestListener));
            while (!endThread)
            {
                HttpListenerContext context = WaitForRequestContext(listener);
                SendRequestResponse(context);
            }
        }

        private static HttpListenerContext WaitForRequestContext(HttpListener listener)
        {
            Utility.WriteTimeStamp("Waiting for request...", typeof(HttpRequestListener));
            HttpListenerContext context = listener.GetContext();
            Utility.WriteTimeStamp("Got request!", typeof(HttpRequestListener));
            return context;
        }

        private static void SendRequestResponse(HttpListenerContext context)
        {
            if (context == null) return;
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

        private static List<string> GenerateListenerPrefixList()
        {
            List<string> outList = new List<string>();
            string baseUrl = "http://+:80/";
            outList.Add(baseUrl + "all/");
            foreach (Driver d in DriverLoader.Drivers)
            {
                outList.Add(baseUrl + d.Id + "/");
                foreach (DriverDevice dd in d.Devices)
                {
                     outList.Add(baseUrl + d.Id + "/" + dd.GetDynamicDevice().Id + "/");
                    foreach (DeviceAttribute da in dd.DeviceBase.DeviceAttributes)
                    {
                        outList.Add(baseUrl + d.Id + "/" + dd.GetDynamicDevice().Id + "/" + da.Label + "/");
                    }
                }
            }
            return outList;
        }

        private static string GetResponseString(HttpListenerRequest request)
        {
            OutputJsonProducer outputJsonProducer = new OutputJsonProducer(DriverLoader.Drivers, "IoT Hub");
            string[] urlParts = SplitRawUrlIntoParts(request.RawUrl);
            if (urlParts.Length == 1 && urlParts[0] == "all")
            {
                return outputJsonProducer.GetHubInformation().ToString();
            }
            else if (urlParts.Length >= 1)
            {
                Driver urlDriver = null;
                DriverDevice urlDriverDevice = null;
                DeviceAttribute urlDeviceAttribute = null;
                string urlNewDeviceAttributeValue = null;
                try
                {
                    if (urlParts.Length >= 1)
                    {
                        urlDriver = DriverLoader.Drivers.Where(x => x.Id == urlParts[0]).First();
                    }
                    if (urlParts.Length >= 2)
                    {
                        urlDriverDevice = urlDriver.Devices.Where(x => x.GetDynamicDevice().Id == urlParts[1]).First();
                    }
                    if (urlParts.Length >= 3)
                    {
                        urlDeviceAttribute = urlDriverDevice.DeviceBase.DeviceAttributes.Where(x => x.Label == urlParts[2]).First();
                    }
                    if (urlParts.Length >= 4 && urlParts[3].StartsWith("set?v="))
                    {
                        urlNewDeviceAttributeValue = urlParts[3].Substring(6);
                    }
                } catch (Exception)
                {
                    return "Invalid request.";
                }
                if (urlNewDeviceAttributeValue != null)
                {
                    Utility.WriteTimeStamp($"Setting value of {urlDriverDevice.GetDynamicDevice().Name}'s attribute {urlDeviceAttribute.Label} to {urlNewDeviceAttributeValue}", typeof(HttpRequestListener));
                    
                    Type attType = urlDeviceAttribute.AttributeType;
                    dynamic val = Convert.ChangeType(urlNewDeviceAttributeValue, attType);
                    dynamic attribute = urlDeviceAttribute;

                    DatabaseHandler.BuildActionSnapshotAndInsert(attribute, urlDriverDevice, attribute.Get(), val);

                    try
                    {
                        attribute.Set(val);
                        return "Success";
                    }
                    catch
                    {
                        return "Error";
                    }
                }
                else if (urlDriver != null && urlDriverDevice != null && urlDeviceAttribute == null)
                {
                    Utility.WriteTimeStamp($"Fetching properties of device {urlDriverDevice.GetDynamicDevice().Name}");
                    return outputJsonProducer.GetDevice(urlDriver, urlDriverDevice).ToString();
                }
                else if (urlDriver != null && urlDriverDevice != null && urlDeviceAttribute != null)
                {
                    Utility.WriteTimeStamp($"Fetching properties of device {urlDriverDevice.GetDynamicDevice().Name}'s attribute {urlDeviceAttribute.Label}");
                    return outputJsonProducer.GetDeviceAttribute(urlDeviceAttribute).ToString();
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
