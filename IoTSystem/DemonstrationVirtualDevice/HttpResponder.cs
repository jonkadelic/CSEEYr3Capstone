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
            listener.Prefixes.Add("http://+:80/request/");
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
            string responseString = $"{{\"powerState\":\"{model.PoweredValue}\",\"powerLevel\":{model.PowerLevelValue}}}";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
