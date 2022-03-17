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

        private Queue<Message> _messages = new Queue<Message> ();

        public ObservableCollection<ChatRoom> ChatRooms { get; set; } = new ObservableCollection<ChatRoom> ();

        private ChatRoom _chatRoom;
        public ChatRoom SelectedChatRoom 
        {
            get => _chatRoom;
            set 
            {
                _chatRoom = value;
                OnPropertyChanged();
            }
        }

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
            SendPrivateMessageCommand = new RelayCommand(async (o) => await SendMessage(), (o) => (SelectedUser != null) && !ChatRooms.Any(cr => cr.Name.Equals($"{LocalClient.Name} {SelectedUser.Name}")));
        }

        private async Task SendMessage() 
        {
            if (SelectedUser != null) 
            {
                // Create a temp room until the user sends a message
                ChatRoom chatRoom = new ChatRoom()
                {
                    Name = $"{LocalClient.Name} {SelectedUser.Name}"
                };

                ChatRooms.Add(chatRoom);
            }
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

        public void CloseWindow()
        {
            Close?.Invoke();
        }

        public void OpenWindow()
        {
            Open?.Invoke();
        }

        public void UpdatePublicChatRooms(ChatRoom[] chatRooms)
        {
            //throw new NotImplementedException();
        }

        public void CreatePrivateRoom(ChatRoom chatRoom)
        {
            //throw new NotImplementedException();
        }

        public void UpdateOnlineClients(List<Client> clients)
        {
            OnlineClients = new ObservableCollection<Client>(clients.Where(c => c.Id != LocalClient.Id));
        }

        public void UpdatePublicChatRooms(List<ChatRoom> chatRooms)
        {
            //throw new NotImplementedException();
        }
    }
}
