using ChatAppServiceLibrary.DataContracts;
using System;
using System.ServiceModel;

namespace WcfClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new ClientCallback());
            ChatManagerServiceClient client = new ChatManagerServiceClient(context);
            //var proxy = client.ChannelFactory.CreateChannel();

            Client newClient = new Client()
            {
                Id = System.Guid.NewGuid(),
                Name = "Ciante",
                TitleId = null,
                CreatedOn = System.DateTime.Now
            };

            try
            {
                if (client.Login(newClient))
                {
                    System.Console.WriteLine($"Client {newClient.Name} added successfully");
                }
                else
                {
                    System.Console.WriteLine($"Try again, client already exists");
                }

                // close the client
                //proxy = null;
                client.Close();
                Console.ReadLine();
            }
            catch (TimeoutException te)
            {
                Console.WriteLine("The service operation timed out. " + te.Message);
                client.Abort();
                Console.Read();
            }
            catch (CommunicationException ce) 
            {
                Console.WriteLine("There was a communication problem. " + ce.Message);
                client.Abort();
                Console.Read();
            }
        }
    }
}
