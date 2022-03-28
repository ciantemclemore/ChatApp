using ChatAppServiceLibrary;
using System;
using System.ServiceModel;

namespace ChatAppServiceHost
{
    /// <summary>
    /// This class contains the Hosted WCF service that the chat
    /// manager application will communicate with.
    /// </summary>
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting ChatApp Service...");

            // ServiceHost instance creation
            // the app.config contains all of the configuration for the host to keep clean code
            ServiceHost chatServiceHost = new ServiceHost(typeof(ChatManagerService));

            try
            {
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
