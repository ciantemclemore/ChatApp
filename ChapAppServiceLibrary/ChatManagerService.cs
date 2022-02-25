using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChapAppServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatManagerService : IChatManagerService
    {
        //private readonly Dictionary<string, IChatManagerCallback> _users;

        private readonly HashSet<string> _clients;

        public ChatManagerService() 
        {
            _clients = new HashSet<string>();
        }

        public bool IsLoggedIn(string userName)
        {
            return _clients.Contains(userName);
        }

        /// <summary>
        /// Login a new user to the chat application
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool Login(string userName)
        {
            if (!_clients.Contains(userName))
            {
                _clients.Add(userName);

                return true;
            }

            return false;
        }

        public void Logout(string userName)
        {
            if (IsLoggedIn(userName)) 
            {
                _clients.Remove(userName);
            }
        }
    }
}
