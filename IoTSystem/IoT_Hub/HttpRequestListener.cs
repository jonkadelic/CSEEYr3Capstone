using DeviceDriverPluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
            Utility.WriteTimeStamp($"Response string is {responseString}", typeof(HttpRequestListener));
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
            outList.Add(baseUrl + "devices/");
            outList.Add(baseUrl + "routines/");
            outList.Add(baseUrl + "routines/new/");
            foreach (Database.Routine r in DatabaseHandler.LoadRoutines())
            {
                outList.Add(baseUrl + "routines/" + r.RoutineID + "/");
                outList.Add(baseUrl + "routines/" + r.RoutineID + "/delete/");
            }
            foreach (Driver d in DriverLoader.Drivers)
            {
                outList.Add(baseUrl + "devices/" + d.Id + "/");
                foreach (DriverDevice dd in d.Devices)
                {
                    outList.Add(baseUrl + "devices/" + d.Id + "/" + dd.GetDynamicDevice().Id + "/");
                    foreach (DeviceProperty da in dd.DeviceBase.DeviceProperties)
                    {
                        outList.Add(baseUrl + "devices/" + d.Id + "/" + dd.GetDynamicDevice().Id + "/" + da.Label + "/");
                    }
                }
            }
            return outList;
        }

        private static string GetResponseString(HttpListenerRequest request)
        {
            OutputJsonProducer outputJsonProducer = new OutputJsonProducer("IoT Hub");
            string[] urlParts = SplitRawUrlIntoParts(request.RawUrl);
            if (urlParts[0] == "devices")
            {
                if (urlParts.Length == 1)
                {
                    return outputJsonProducer.GetTopLevelDevices().ToString();
                }
                else if (urlParts.Length >= 2)
                {
                    Driver urlDriver = null;
                    DriverDevice urlDriverDevice = null;
                    DeviceProperty urlDeviceProperty = null;
                    string urlNewDevicePropertyValue = null;
                    try
                    {
                        if (urlParts.Length >= 2)
                        {
                            urlDriver = DriverLoader.Drivers.Where(x => x.Id == urlParts[1]).First();
                        }
                        if (urlParts.Length >= 3)
                        {
                            urlDriverDevice = urlDriver.Devices.Where(x => x.GetDynamicDevice().Id == urlParts[2]).First();
                        }
                        if (urlParts.Length >= 4)
                        {
                            urlDeviceProperty = urlDriverDevice.DeviceBase.DeviceProperties.Where(x => x.Label == urlParts[3]).First();
                        }
                        if (urlParts.Length >= 5 && urlParts[4].StartsWith("set?v="))
                        {
                            urlNewDevicePropertyValue = urlParts[4].Substring(6);
                        }
                    }
                    catch (Exception)
                    {
                        return "Invalid request.";
                    }
                    if (urlNewDevicePropertyValue != null)
                    {
                        Utility.WriteTimeStamp($"Setting value of {urlDriverDevice.GetDynamicDevice().Name}'s property {urlDeviceProperty.Label} to {urlNewDevicePropertyValue}", typeof(HttpRequestListener));



                        // Removed since we don't need to save action snapshots any more
                        //DatabaseHandler.BuildActionSnapshotAndInsert(property, urlDriverDevice, property.Get(), val);

                        try
                        {
                            Type attType = urlDeviceProperty.PropertyType;
                            dynamic val = Convert.ChangeType(urlNewDevicePropertyValue, attType);
                            dynamic property = urlDeviceProperty;
                            property.Set(val);
                            return "Success";
                        }
                        catch
                        {
                            return "Error";
                        }
                    }
                    else if (urlDriver != null && urlDriverDevice != null && urlDeviceProperty == null)
                    {
                        Utility.WriteTimeStamp($"Fetching properties of device {urlDriverDevice.GetDynamicDevice().Name}");
                        return outputJsonProducer.GetDevice(urlDriver, urlDriverDevice).ToString();
                    }
                    else if (urlDriver != null && urlDriverDevice != null && urlDeviceProperty != null)
                    {
                        Utility.WriteTimeStamp($"Fetching properties of device {urlDriverDevice.GetDynamicDevice().Name}'s property {urlDeviceProperty.Label}");
                        return outputJsonProducer.GetDeviceProperty(urlDeviceProperty).ToString();
                    }
                }
            }
            else if (urlParts[0] == "routines")
            {
                if (urlParts.Length == 1)
                {
                    return outputJsonProducer.GetRoutinesArray().ToString();
                }
                else if (urlParts.Length == 2)
                {
                    if (urlParts[1] == "new")
                    {
                        JToken jt;
                        string s;

                        try
                        {
                            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                            {
                                s = reader.ReadToEnd();
                                jt = JToken.Parse(s);
                            }
                            Database.Routine routine = new Database.Routine();
                            routine.RoutineName = jt["name"].Value<string>();
                            routine.TargetDeviceID = jt["target"]["deviceId"].Value<string>();
                            routine.TargetDriverID = jt["target"]["driverId"].Value<string>();
                            routine.TargetProperty = jt["target"]["property"].Value<string>();
                            routine.RoutineID = MongoDB.Bson.ObjectId.GenerateNewId();

                            Type valueType = DriverLoader.Drivers.Where(x => x.Id == routine.TargetDriverID).First().Devices.Where(x => x.Id == routine.TargetDeviceID).First().DeviceBase[routine.TargetProperty].PropertyType;
                            routine.TargetValue = Convert.ChangeType(jt["target"]["value"].Value<string>(), valueType);
                            routine.RoutineConditions = new List<Database.RoutineCondition>();
                            foreach (JToken rct in jt["conditions"])
                            {
                                Database.RoutineCondition condition = new Database.RoutineCondition();
                                condition.DriverID = rct["driverId"].Value<string>();
                                condition.DeviceID = rct["deviceId"].Value<string>();
                                condition.PropertyName = rct["property"].Value<string>();
                                condition.RoutineConditionID = MongoDB.Bson.ObjectId.GenerateNewId();
                                Type condValueType = DriverLoader.Drivers.Where(x => x.Id == condition.DriverID).First().Devices.Where(x => x.Id == condition.DeviceID).First().DeviceBase[condition.PropertyName].PropertyType;
                                condition.DesiredValue = Convert.ChangeType(rct["value"].Value<string>(), condValueType);
                                switch (rct["comparison"].Value<string>())
                                {
                                    case "EQUAL":
                                        condition.Comparison = Database.RoutineCondition.COMPARISON.EQUAL;
                                        break;
                                    case "GREATER":
                                        condition.Comparison = Database.RoutineCondition.COMPARISON.GREATER;
                                        break;
                                    case "LESS":
                                        condition.Comparison = Database.RoutineCondition.COMPARISON.LESS;
                                        break;
                                }
                                routine.RoutineConditions.Add(condition);
                            }
                            DatabaseHandler.InsertRoutine(routine);
                            RoutineScheduler.ReloadRoutines();
                        }
                        catch (Exception)
                        {
                            return "Invalid message body.";
                        }
                    }
                    else
                    {
                        try
                        {
                            return outputJsonProducer.GetRoutine(RoutineScheduler.routines.Where(x => x.RoutineID.ToString() == urlParts[1]).First()).ToString();
                        }
                        catch (Exception)
                        {
                            return "No routine matches that ID.";
                        }
                    }
                }
                else if (urlParts.Length == 3)
                {
                    if (urlParts[2] == "delete")
                    {
                        try
                        {
                            Database.Routine r = RoutineScheduler.routines.Where(x => x.RoutineID.ToString() == urlParts[1]).First();
                            DatabaseHandler.DeleteRoutine(r);
                            RoutineScheduler.ReloadRoutines();
                            return "Routine deleted successfully.";
                        }
                        catch (Exception)
                        {
                            return "No routine matches that ID.";
                        }
                    }
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
