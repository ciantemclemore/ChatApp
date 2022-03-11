using ChatAppServiceLibrary;
using System;
using System.ServiceModel;

namespace ChatAppServiceHost
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting ChatApp Service...");

            // The base address that the client will use to make request to the chat app service
            Uri baseAddress = new Uri($"net.tcp://localhost:5001/ChatManagerService");

            NetTcpBinding netTcpBinding = new NetTcpBinding(SecurityMode.None);
            netTcpBinding.MaxConnections = 100;
            netTcpBinding.Name = $"netTcpBinding1";

            // ServiceHost instance creation
            ServiceHost chatServiceHost = new ServiceHost(typeof(ChatManagerService));

            try
            {
                // add the endpoint
                var serviceEndpoint = chatServiceHost.AddServiceEndpoint(typeof(IChatManagerService), netTcpBinding, baseAddress);
                serviceEndpoint.Name = $"netTcp";

                // open the service and block
                chatServiceHost.Open();

                Console.WriteLine($"Tcp service opened on {DateTime.Now}.");

                // allow the user to close the service
                Console.WriteLine("Press <Enter> to terminate the service");
                Console.WriteLine();
                Console.ReadLine();
                chatServiceHost.Close();
            }
            catch (CommunicationException ce) 
            {
                Console.WriteLine($"An exception occurred: {ce.Message}");
                chatServiceHost.Abort();
            }
        }
    }
}
