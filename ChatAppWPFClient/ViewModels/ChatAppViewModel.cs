using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppWPFClient.ViewModels
{
    public class ChatAppViewModel : ViewModelBase, IChatManagerServiceCallback, IWindow
    {
        public Client LocalClient { get; set; }
        public Action Close { get ; set; }
        public Action Open { get ; set; }

        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ChatAppViewModel(NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void InputDialog_OnLoginButtonClicked()
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

        public void CloseWindow()
        {
            Close?.Invoke();
        }

        public void OpenWindow()
        {
            Open?.Invoke();
        }
    }
}
