using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppWPFClient
{
    public class MainViewModel : IChatManagerServiceCallback
    {
        public Client LocalClient { get; set; }

        private void InputDialog_OnLoginButtonClicked(LoginWindow inputDialog)
        {
            

            //// create a client
            //Client newClient = new Client()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = inputDialog.Answer,
            //    CreatedOn = DateTime.Now,
            //    TitleId = null
            //};

            //while (!client.Login(newClient)) 
            //{
            //    inputDialog.lblQuestion.Content = "Try again, login name already exists:";
            //}

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
