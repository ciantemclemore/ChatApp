using System;
using System.ServiceModel;

namespace ChatAppServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting ChatApp Service...");

            // The base address that the client will use to make request to the chat app service
            Uri baseAddress = new Uri($"http://localhost:5000/ChatApp/");

            // ServiceHost instance creation
            //ServiceHost chatServiceHost = new ServiceHost(typeof(ChatService), baseAddress);

            try
            {
                //chatServiceHost.
            }
            catch (CommunicationException ce) 
            {
                Console.WriteLine($"An exception occurred: {ce.Message}");
            }


            Console.ReadLine();
        }
    }
}
