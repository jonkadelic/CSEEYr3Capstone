using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemonstrationVirtualDevice
{
    public class HttpResponder
    {
        private static Thread listenerThread;
        private static bool endThread = false;
        private static HttpListener listener;
        private static AppViewModel model;

        static HttpResponder()
        {
            listener = new HttpListener();
        }

        public static void Stop()
        {
            endThread = true;
        }

        public static void Run(AppViewModel model)
        {
            HttpResponder.model = model;
            listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        private static void Listen()
        {
            GenerateListenerPrefixes();
            listener.Start();
            while (!endThread)
            {
                HttpListenerContext context = WaitForRequestContext(listener);
                SendRequestResponse(context);
            }
        }

        private static void GenerateListenerPrefixes()
        {
            listener.Prefixes.Clear();
            string baseUrl = "http://+:80/";
            listener.Prefixes.Add(baseUrl + "request/");
            listener.Prefixes.Add(baseUrl + "set/bulbR/");
            listener.Prefixes.Add(baseUrl + "set/bulbG/");
            listener.Prefixes.Add(baseUrl + "set/bulbB/");
            listener.Prefixes.Add(baseUrl + "set/bulbPowered/");
            listener.Prefixes.Add(baseUrl + "set/plugPowered/");
        }

        private static HttpListenerContext WaitForRequestContext(HttpListener listener)
        {
            HttpListenerContext context = listener.GetContext();
            return context;
        }

        private static void SendRequestResponse(HttpListenerContext context)
        {
            if (context == null) return;
            HttpListenerResponse response = context.Response;
            string rawUrl = context.Request.RawUrl;
            if (rawUrl.Contains("set"))
            {
                if (rawUrl.Contains("bulbPowered"))
                {
                    bool success = bool.TryParse(rawUrl.Split("/".ToArray()).Last(), out bool result);
                    if (success) model.BulbPowered = result;
                }
                else if (rawUrl.Contains("bulbR"))
                {
                    bool success = byte.TryParse(rawUrl.Split("/".ToArray()).Last(), out byte result);
                    if (success) model.BulbR = result;
                }
                else if (rawUrl.Contains("bulbG"))
                {
                    bool success = byte.TryParse(rawUrl.Split("/".ToArray()).Last(), out byte result);
                    if (success) model.BulbG = result;
                }
                else if (rawUrl.Contains("bulbB"))
                {
                    bool success = byte.TryParse(rawUrl.Split("/".ToArray()).Last(), out byte result);
                    if (success) model.BulbB = result;
                }
                else if (rawUrl.Contains("plugPowered"))
                {
                    bool success = bool.TryParse(rawUrl.Split("/".ToArray()).Last(), out bool result);
                    if (success) model.PlugState = result;
                }
            }
            string responseString = $"{{\"bulbPowered\":\"{model.BulbPowered}\",\"bulbR\":\"{model.BulbR}\",\"bulbG\":\"{model.BulbG}\",\"bulbB\":\"{model.BulbB}\",\"plugPowered\":\"{model.PlugState}\"}}";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
