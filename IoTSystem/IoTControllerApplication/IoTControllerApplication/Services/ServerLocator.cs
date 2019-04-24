using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoTControllerApplication.Services
{
    public static class ServerLocator
    {
        static UdpClient client = new UdpClient();
        public static IPAddress GetServerLocation()
        {
            client.Client.ReceiveTimeout = 5000;
            byte[] data = Encoding.ASCII.GetBytes("verify");
            client.EnableBroadcast = true;


            client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            string response = "";
            bool gotResponse = false;
            while (gotResponse == false)
            {
                Send(data);
                try
                {
                    response = Encoding.ASCII.GetString(client.Receive(ref endpoint));
                    gotResponse = true;
                }
                catch (SocketException)
                {
                }
            }

            if (response == "verified")
                return endpoint.Address;
            else return null;
        }

        static void Send(byte[] data)
        {
            client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
        }
    }
}
