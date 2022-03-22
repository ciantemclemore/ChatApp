using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ChatAppWPFClient.ViewModels
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class ChatAppViewModel : ViewModelBase, IChatManagerServiceCallback, IWindow
    {
        #region Client/Connection Members
        public Client LocalClient { get; set; }

        public ChatManagerServiceClient TcpClient { get; set; }
        #endregion

        #region Full View Properties
        private ObservableCollection<Message> _currentMessages = new ObservableCollection<Message>();
        private ObservableCollection<ChatRoom> _privateChatRooms = new ObservableCollection<ChatRoom>();
        private ObservableCollection<ChatRoom> _publicChatRooms = new ObservableCollection<ChatRoom>();
        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private ChatRoom _chatRoom;
        private Client _selectedUser;
        private string _messageText;

        public ObservableCollection<Message> CurrentMessages
        {
            get => _currentMessages;
            set
            {
                _currentMessages = value;
                OnPropertyChanged();
            }
        }

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
        public Client SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Client> OnlineClients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChatRoom> PublicChatRooms
        {
            get => _publicChatRooms;
            set
            {
                _publicChatRooms = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChatRoom> PrivateChatRooms
        {
            get => _privateChatRooms;
            set
            {
                _privateChatRooms = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region View Model Properties
        Dictionary<Guid, ObservableCollection<Message>> ListViewMessages { get; set; } = new Dictionary<Guid, ObservableCollection<Message>>();

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        #endregion

        #region View Commands
        public RelayCommand CreatePrivateRoomCommand { get; set; }

        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand CreatePublicRoomCommand { get; set; }

        public ICommand NavigateChatAppControl { get; set; }
        #endregion

        #region Fields
        private readonly object _sync = new object();
        private readonly NavigationStore _navigationStore;
        #endregion

        public Action Close { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public ChatAppViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            CreatePrivateRoomCommand = new RelayCommand(async (o) => await CreatePrivateRoom(), (o) => (SelectedUser != null) && !PrivateChatRooms.Any(cr => cr.DisplayName.Equals($"{SelectedUser.Name}")));
            CreatePublicRoomCommand = new RelayCommand((o) => CreatePublicRoom());
            SendMessageCommand = new RelayCommand(async (o) => await SendMessage(), (o) => !string.IsNullOrEmpty(MessageText) && SelectedChatRoom != null);
        }

        private async Task CreatePrivateRoom()
        {
            ChatRoom chatRoom = await TcpClient.CreatePrivateChatRoomAsync(new ChatRoomRequest()
            {
                Clients = new List<Client>() { LocalClient, SelectedUser },
                IsPublic = false,
                ServerName = $"{LocalClient.Id} {SelectedUser.Id}",
                DisplayName = SelectedUser.Name
            });

            if (!PrivateChatRooms.Any(cr => cr.Id == chatRoom.Id))
            {
                PrivateChatRooms.Add(chatRoom);
            }

            SelectedChatRoom = chatRoom;
            SelectedUser = null;
        }

        private void CreatePublicRoom()
        {
            CreatePublicRoomViewModel createPublicRoomViewModel = new CreatePublicRoomViewModel(_navigationStore);
            createPublicRoomViewModel.LocalClient = LocalClient;
            createPublicRoomViewModel.TcpClient = TcpClient;
            createPublicRoomViewModel.ReturnToViewModel = this;
            NavigateChatAppControl = new NavigateCommand<CreatePublicRoomViewModel>(_navigationStore, () => createPublicRoomViewModel);
            NavigateChatAppControl.Execute(null);
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
                    ChatRoomName = SelectedChatRoom.DisplayName,
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
                    ChatRoomName = SelectedChatRoom.DisplayName,
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


        public void ReceiveMessage(Message message)
        {
            // TODO: Have a little bug to fix, private messages go to the channel you're selected on instead of the correct channel
            lock (_sync)
            {
                // Place the message in neccessary chat room, if it doesn't exist locally, create it
                ChatRoom chatRoom = PrivateChatRooms.Union(PublicChatRooms).FirstOrDefault(cr => cr.Id == message.ChatRoomId);

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
                        DisplayName = message.Sender.Name,
                        ReceiverTitle = message.Receiver.Name,
                        SenderTitle = message.Sender.Name,
                    };

                    PrivateChatRooms.Add(newChatRoom);
                    SelectedChatRoom = newChatRoom;
                }

                CurrentMessages.Add(message);
            }
        }

        public void UpdateOnlineClients(List<Client> clients)
        {
            OnlineClients = new ObservableCollection<Client>(clients.Where(c => c.Id != LocalClient.Id));
        }

        public void UpdatePublicChatRooms(List<ChatRoom> chatRooms)
        {
            PublicChatRooms = new ObservableCollection<ChatRoom>(chatRooms);
        }
    }
}
