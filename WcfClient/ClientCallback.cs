using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClient
{
    public class ClientCallback : IChatManagerServiceCallback
    {
        public void GetOnlineClients(Client[] clients)
        {
            //throw new NotImplementedException();
        }

        public void ReceiveMessage(Message message)
        {
            //throw new NotImplementedException();
        }

        public void UpdateAvailableTitles(Title[] titles)
        {
            //throw new NotImplementedException();
        }

        public void UpdateOnlineClients(Client[] clients)
        {
            //throw new NotImplementedException();
        }
    }
}
