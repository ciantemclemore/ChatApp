﻿using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatAppWPFClient.ViewModels
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class ChatAppViewModel : ViewModelBase, IChatManagerServiceCallback
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
                if (value != null)
                {
                    _chatRoom = value;
                    OnPropertyChanged();

                    if (!ListViewMessages.ContainsKey(_chatRoom.Id))
                    {
                        ListViewMessages.Add(_chatRoom.Id, new ObservableCollection<Message>());
                    }
                    CurrentMessages = ListViewMessages[_chatRoom.Id];

                    if (!_chatRoom.IsPublic || (_chatRoom.IsPublic && !_chatRoom.Clients.Any(c => c.Id == LocalClient.Id)))
                    {
                        IsRoomJoined = false;
                        OnJoinRoomCommand.Execute(null);
                    }
                }
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
        private Dictionary<Guid, ObservableCollection<Message>> ListViewMessages { get; set; } = new Dictionary<Guid, ObservableCollection<Message>>();

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public bool IsRoomJoined { get; set; }

        public string CreatePublicRoomName { get; set; }
        #endregion

        #region View Commands
        public RelayCommand CreatePrivateRoomCommand { get; set; }

        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand CreatePublicRoomCommand { get; set; }

        public RelayCommand OnJoinRoomCommand { get; set; }

        public ICommand NavigateChatAppControl { get; set; }
        #endregion

        #region Fields
        private readonly object _sync = new object();
        private readonly NavigationStore _navigationStore;
        #endregion

        public ChatAppViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            CreatePrivateRoomCommand = new RelayCommand(async (o) => await CreatePrivateRoom(), (o) => (SelectedUser != null) && !PrivateChatRooms.Any(cr => cr.DisplayName.Equals($"{SelectedUser.Name}")));
            CreatePublicRoomCommand = new RelayCommand((o) => CreatePublicRoom());
            SendMessageCommand = new RelayCommand(async (o) => await SendMessage(), (o) => !string.IsNullOrEmpty(MessageText) && SelectedChatRoom != null);
            OnJoinRoomCommand = new RelayCommand(async (o) => await JoinPublicRoom(), (o) => !IsRoomJoined);
        }

        #region Client to Server Method Calls
        private async Task JoinPublicRoom()
        {
            // needs to work for public selection and private
            if (PublicChatRooms.Any() && (!SelectedChatRoom.Clients.Any(c => c.Id == LocalClient.Id) || !SelectedChatRoom.IsPublic))
            {
                await TcpClient.JoinChatRoomAsync(SelectedChatRoom.Id, LocalClient);
                IsRoomJoined = true;
            }
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
            CreatePublicRoomViewModel createPublicRoomViewModel = new CreatePublicRoomViewModel(_navigationStore)
            {
                LocalClient = LocalClient,
                TcpClient = TcpClient,
                ReturnToViewModel = this
            };
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

            MessageText = string.Empty;

            await TcpClient.SendMessageAsync(newMessage);
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        #endregion


        #region Server to Client Callbacks & Helper Methods
        public void ReceiveMessage(Message message)
        {
            // Place the message in neccessary chat room, if it doesn't exist locally, create it
            ChatRoom chatRoom = PrivateChatRooms.Union(PublicChatRooms).FirstOrDefault(cr => cr.Id == message.ChatRoomId);

            if (chatRoom != null)
            {
                chatRoom.Messages.Add(message);
                chatRoom.LastMessage = !string.IsNullOrEmpty(chatRoom.ReceiverTitle) && message.Receiver.Name == LocalClient.Name ? message.Content : string.Empty;
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
                    LastMessage = message.Content
                };

                PrivateChatRooms.Add(newChatRoom);
            }

            // Add the message to the current channel
            if (ListViewMessages.ContainsKey(message.ChatRoomId))
            {
                ListViewMessages[message.ChatRoomId].Add(message);
            }
            else
            {
                ListViewMessages.Add(message.ChatRoomId, new ObservableCollection<Message> { message });
            }
        }

        public void UpdateOnlineClients(List<Client> clients)
        {
            OnlineClients = new ObservableCollection<Client>(clients.Where(c => c.Id != LocalClient.Id));

            // Could use this code if you want to remove private chat rooms of users who logged off
            //var onlineClientName = OnlineClients.Select(c => c.Name);
            //PrivateChatRooms = new ObservableCollection<ChatRoom>(PrivateChatRooms.Where(pcr => onlineClientName.Contains(pcr.DisplayName)));
        }

        public void UpdatePublicChatRooms(List<ChatRoom> chatRooms)
        {
            PublicChatRooms = new ObservableCollection<ChatRoom>(chatRooms);
            RemoveUnusedChatMessages();

            // Must reset the current chat room, using "SelectedValue" property on listview retriggers the call on list reinitialization
            if (!string.IsNullOrEmpty(CreatePublicRoomName)) 
            {
                SelectedChatRoom = PublicChatRooms.FirstOrDefault(pcr => pcr.DisplayName == CreatePublicRoomName);
                CreatePublicRoomName = null;
            }
            //if (SelectedChatRoom != null)
            //{
            //    ChatRoom currentChatRoom = SelectedChatRoom;
            //    SelectedChatRoom = currentChatRoom.IsPublic ? PublicChatRooms.FirstOrDefault(cr => cr.Id == currentChatRoom.Id) : PrivateChatRooms.FirstOrDefault(cr => cr.Id == currentChatRoom.Id);
            //}
        }

        private void RemoveUnusedChatMessages()
        {
            // Make sure to remove any public rooms from list view messags that don't exist
            foreach (Guid chatRoomId in PublicChatRooms.Select(cr => cr.Id))
            {
                if (!ListViewMessages.Keys.Any(key => key == chatRoomId))
                {
                    ListViewMessages.Remove(chatRoomId);
                }
            }
        }
        #endregion
    }
}
