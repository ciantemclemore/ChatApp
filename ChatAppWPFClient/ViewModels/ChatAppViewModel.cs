using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatAppWPFClient.ViewModels
{
    public class ChatAppViewModel : ViewModelBase, IChatManagerServiceCallback, IWindow
    {
        public Client LocalClient { get; set; }
        public Action Close { get ; set; }
        public Action Open { get ; set; }

        private RelayCommand CreateRoomCommand { get; set; }
        public RelayCommand SendPrivateMessageCommand { get; set; }

        private Client _selectedUser = null;
        public Client SelectedUser 
        {
            get => _selectedUser;
            set 
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        public ObservableCollection<Client> OnlineClients 
        {
            get => _clients;
            set 
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ChatAppViewModel(NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            SendPrivateMessageCommand = new RelayCommand(async (o) => await SendMessage(), (o) => !(SelectedUser == null));
        }

        private async Task SendMessage() 
        {
            
        }

        private async Task CreateRoom(object parameter) 
        {
        
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
            OnlineClients = new ObservableCollection<Client>(clients.Where(c => c.Id != LocalClient.Id));
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
