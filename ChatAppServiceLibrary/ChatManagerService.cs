using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatManagerService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ChatManagerService : IChatManagerService
    {

        private readonly Dictionary<Guid, Title> _titles;
        private readonly Dictionary<Guid, IChatManagerCallback> _clientCallbacks;
        private readonly ObservableCollection<Client> _clients;


        public ChatManagerService()
        {
            _titles = new Dictionary<Guid, Title>();
            _clients = new ObservableCollection<Client>();
            _clientCallbacks = new Dictionary<Guid, IChatManagerCallback>();
        }

        public bool IsLoggedIn(string userName)
        {
            throw new NotImplementedException();
        }

        public bool Login(string userName)
        {
            throw new NotImplementedException();
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
