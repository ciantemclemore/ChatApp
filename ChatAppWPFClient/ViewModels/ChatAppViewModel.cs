using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatAppWPFClient.ViewModels
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class ChatAppViewModel : ViewModelBase, IChatManagerServiceCallback, IWindow
    {
        public Client LocalClient { get; set; }
        public ChatManagerServiceClient TcpClient { get; set; }
        public Action Close { get ; set; }
        public Action Open { get ; set; }
        
        private ObservableCollection<Message> _currentMessages = new ObservableCollection<Message>();
        public ObservableCollection<Message> CurrentMessages 
        {
            get => _currentMessages;
            set 
            {
                _currentMessages = value;
                OnPropertyChanged();
            }
        }

        Dictionary<Guid, ObservableCollection<Message>> ListViewMessages { get; set; } = new Dictionary<Guid, ObservableCollection<Message>>();

        public ObservableCollection<ChatRoom> ChatRooms { get; set; } = new ObservableCollection<ChatRoom> ();

        private ChatRoom _chatRoom;
        public ChatRoom SelectedChatRoom 
        {
            get => _chatRoom;
            set 
            {
                _chatRoom = value;
                OnPropertyChanged();
                if (!ListViewMessages.ContainsKey(_chatRoom.Id))
                {
                    ListViewMessages.Add(_chatRoom.Id, new ObservableCollection<Message>());
                }
                CurrentMessages = ListViewMessages[_chatRoom.Id];
            }
        }

        private RelayCommand CreateRoomCommand { get; set; }
        public RelayCommand CreatePrivateRoomCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

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

        private string _messageText;
        public string MessageText 
        {
            get => _messageText;
            set 
            {
                _messageText = value;
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

        private readonly object _sync = new object();
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ChatAppViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            CreatePrivateRoomCommand = new RelayCommand(async (o) => await CreateLocalPrivateRoom(), (o) => (SelectedUser != null) && !ChatRooms.Any(cr => cr.Name.Equals($"{SelectedUser.Name}")));
            SendMessageCommand = new RelayCommand(async (o) => await SendMessage(), (o) => !string.IsNullOrEmpty(MessageText) && SelectedChatRoom != null);
        }

        private async Task CreateLocalPrivateRoom() 
        {
            ChatRoom chatRoom = await TcpClient.CreateChatRoomAsync(new ChatRoomRequest() 
            {
                Clients = new List<Client>() { LocalClient, SelectedUser },
                IsPublic = false,
                Name = SelectedUser.Name
            });

            ChatRooms.Add(chatRoom);
            SelectedChatRoom = chatRoom;
            SelectedUser = null;
        }

        private async Task SendMessage() 
        {
            Message newMessage;

            // Determine if it is a public or private room before sending the message
            if (!SelectedChatRoom.IsPublic)
            {
                Client receiver = SelectedChatRoom.Clients.FirstOrDefault(c => c.Id != LocalClient.Id);

                // Create a message for two users in a private room
                newMessage = new Message()
                {
                    Id = Guid.NewGuid(),
                    ChatRoomId = SelectedChatRoom.Id,
                    ChatRoomName = SelectedChatRoom.Name,
                    Content = MessageText,
                    Receiver = receiver,
                    Sender = LocalClient,
                    TimeStamp = DateTime.Now
                };
            }
            else 
            {
                // Create a message for all users in a public room
                newMessage = new Message()
                {
                    Id = Guid.NewGuid(),
                    ChatRoomId = SelectedChatRoom.Id,
                    ChatRoomName = SelectedChatRoom.Name,
                    Content = MessageText,
                    Receiver = null,
                    Sender = LocalClient,
                    TimeStamp = DateTime.Now
                };
            }

            await TcpClient.SendMessageAsync(newMessage);
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
            lock (_sync)
            {
                // Place the message in neccessary chat room, if it doesn't exist locally, create it
                ChatRoom chatRoom = ChatRooms.FirstOrDefault(cr => cr.Id == message.ChatRoomId);

                if (chatRoom != null)
                {
                    chatRoom.Messages.Add(message);
                }
                else 
                {
                    ChatRoom newChatRoom = new ChatRoom()
                    {
                        Id = message.ChatRoomId,
                        Clients = new List<Client>() { message.Sender, message.Receiver },
                        IsPublic = false,
                        Messages = new List<Message>() { message },
                        Name = message.Sender.Name,
                        ReceiverTitle = message.Receiver.Name,
                        SenderTitle = message.Sender.Name,
                    };

                    ChatRooms.Add(newChatRoom); 
                    SelectedChatRoom = newChatRoom;
                }

                CurrentMessages.Add(message);
            }
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
            //OnlineClients = new ObservableCollection<Client>(clients.Where(c => c.Id != LocalClient.Id));
            foreach (var client in clients) 
            {
                if (client.Id != LocalClient.Id && !OnlineClients.Any(c => c.Id == client.Id)) 
                {
                    OnlineClients.Add(client);
                }
            }
        }

        public void UpdatePublicChatRooms(List<ChatRoom> chatRooms)
        {
            //throw new NotImplementedException();
        }
    }
}
