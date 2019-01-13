using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoTControllerApplication.Services
{
    public static class ServerLocator
    {
        public static IPAddress GetServerLocation()
        {
            UdpClient client = new UdpClient();
            byte[] data = Encoding.ASCII.GetBytes("verify");
            client.EnableBroadcast = true;


            client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            string response = Encoding.ASCII.GetString(client.Receive(ref endpoint));
            if (response == "verified")
                return endpoint.Address;
            else return null;
        }
    }
}
