using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatManagerService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ChatManagerService : IChatManagerService
    {
        private readonly Dictionary<Guid, IChatManagerCallback> _clientCallbacks = new Dictionary<Guid, IChatManagerCallback>();
        private readonly ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private readonly ObservableCollection<Title> _titles = new ObservableCollection<Title>();
        private object clientLock = new object();
        private object titleLock = new object();

        private IChatManagerCallback CurrentCallback 
        {
            get { return OperationContext.Current.GetCallbackChannel<IChatManagerCallback>(); }
        }

        public bool CreateTitle(string titleName)
        {
            if (_titles.Any(t => t.Name == titleName))
            {
                return false;
            }
            else 
            {
                lock (titleLock) 
                {
                    Title newTitle = new Title()
                    {
                        Id = Guid.NewGuid(),
                        Clients = new List<Client>(),
                        Messages = new ObservableCollection<Message>(),
                        Name = titleName
                    };

                    _titles.Add(newTitle);
                }

                // now we update each client with the new title
                lock (clientLock) 
                {
                    foreach (Guid clientKey in _clientCallbacks.Keys)
                    {
                        IChatManagerCallback callback = _clientCallbacks[clientKey];

                        try
                        {
                            callback.UpdateAvailableTitles(_titles);
                        }
                        catch (Exception)
                        {
                            _clientCallbacks.Remove(clientKey);
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        public void JoinTitle(Client client, Guid titleId)
        {
        
        }
        public bool IsLoggedIn(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows a new client to login and begin a session if the client name does not already exist
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Login(Client client)
        {
            if (!_clients.Any(c => c.Name.Equals(client.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                !_clientCallbacks.ContainsValue(CurrentCallback)) 
            {
                lock (clientLock) 
                {
                    // this is a new client, add them to the clients list and callback list
                    _clients.Add(client);
                    _clientCallbacks.Add(client.Id, CurrentCallback);

                    // Now we must update all the clients with the newly connected client
                    foreach (Guid clientKey in _clientCallbacks.Keys) 
                    {
                        IChatManagerCallback callback = _clientCallbacks[clientKey];

                        try
                        {
                            callback.UpdateOnlineClients(_clients);
                        }
                        catch(Exception) 
                        {
                            _clientCallbacks.Remove(clientKey);
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }
        public void SendMessage(Message message)
        {
            lock (clientLock) 
            {
                // If we have a valid receiverId, we know the message is a private message
                // else, the message should be broadcasted to all the clients
                if (message.ReceiverId != null) 
                {
                
                }
            }
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }

    }
}
