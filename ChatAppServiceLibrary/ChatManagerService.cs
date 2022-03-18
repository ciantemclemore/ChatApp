using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ChatAppServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatManagerService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ChatManagerService : IChatManagerService
    {
        private readonly Dictionary<Guid, IChatManagerCallback> _clientCallbacks = new Dictionary<Guid, IChatManagerCallback>();
        private readonly ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private readonly Dictionary<Guid, ChatRoom> _chatRooms = new Dictionary<Guid, ChatRoom>();
        private readonly object clientLock = new object();
        private readonly object chatRoomLock = new object();
        private readonly object callbackLock = new object();

        private IChatManagerCallback CurrentCallback => OperationContext.Current.GetCallbackChannel<IChatManagerCallback>();

        private bool IsLoggedIn(Guid userId)
        {
            lock (callbackLock)
            {
                return _clientCallbacks.ContainsKey(userId);
            }
        }

        /// <summary>
        /// Allows a new client to login and begin a session if the client name does not already exist
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Login(Client client)
        {
            lock (clientLock)
            {
                if (!_clients.Any(c => c.Name.Equals(client.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    // this is a new client, add them to the clients list and callback list
                    _clients.Add(client);
                }
                else
                {
                    return false;
                }
            }
            lock (callbackLock)
            {
                if (!_clientCallbacks.ContainsValue(CurrentCallback))
                {
                    _clientCallbacks.Add(client.Id, CurrentCallback);

                    // Now we must update all the clients with the newly connected client
                    foreach (Guid clientKey in _clientCallbacks.Keys)
                    {
                        IChatManagerCallback callback = _clientCallbacks[clientKey];

                        try
                        {
                            callback.UpdateOnlineClients(_clients);
                        }
                        catch (Exception)
                        {
                            _clientCallbacks.Remove(clientKey);
                            return false;
                        }
                    }

                    // Need to also provide the new client with the available rooms
                    foreach (Guid clientKey in _clientCallbacks.Keys)
                    {
                        IChatManagerCallback callback = _clientCallbacks[clientKey];
                        callback.UpdatePublicChatRooms(GetAvailableRooms());
                    }
                }
            }
            return true;
        }
        public void SendMessage(Message message)
        {
            // First, add the message to the chatroom it belongs to
            lock (chatRoomLock)
            {
                _chatRooms[message.ChatRoomId].Messages.Add(message);
            }

            if (message.Receiver != null)
            {
                // send the message to the private room
                lock (callbackLock)
                {
                    bool isReceiverOnline = IsLoggedIn(message.Receiver.Id);
                    IChatManagerCallback senderCallback = _clientCallbacks[message.Sender.Id];
                    IChatManagerCallback receiverCallback = _clientCallbacks[message.Receiver.Id];

                    if (isReceiverOnline)
                    {
                        // send the message to both clients
                        senderCallback.ReceiveMessage(message);
                        receiverCallback.ReceiveMessage(message);
                    }
                    else
                    {
                        // send the message only back to the sender
                        senderCallback.ReceiveMessage(message);
                    }
                }
            }
            else 
            {
                // The message was sent in a public room, send to all clients in that room
                lock (callbackLock)
                {
                    foreach (var callback in _clientCallbacks)
                    {
                        callback.Value.ReceiveMessage(message);
                    }
                }
            }
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }

        public void JoinChatRoom(Guid chatRoomId, Client client)
        {
            lock (chatRoomLock)
            {
                ChatRoom chatRoomToJoin = _chatRooms[chatRoomId];

                // Add the client to the new room
                if (chatRoomToJoin != null)
                {
                    chatRoomToJoin.Clients.Add(client);
                }

                List<ChatRoom> chatRoomsToLeave = _chatRooms.Values.Where(cr => cr.IsPublic && !cr.Name.Equals(chatRoomToJoin?.Name)).ToList();

                // Clients can only be in one public room at a time, remove the client from the previous room
                if (chatRoomsToLeave.Any())
                {
                    foreach (ChatRoom chatRoom in chatRoomsToLeave)
                    {
                        chatRoom.Clients.Remove(client);
                    }
                }

                lock (callbackLock) 
                {
                    foreach (var callback in _clientCallbacks.Values) 
                    {
                        callback.UpdatePublicChatRooms(new ObservableCollection<ChatRoom>(_chatRooms.Values));
                    }
                }
            }
        }

        public ChatRoom GetChatRoom(Guid chatRoomId) 
        {
            lock (chatRoomLock) 
            {
                if (_chatRooms.ContainsKey(chatRoomId))
                {
                    return _chatRooms[chatRoomId];
                }
                else 
                {
                    return null;
                }
            }
        }

        public bool CanCreatePublicChatRoom(Guid chatRoomId, string chatRoomName)
        {
            lock (chatRoomLock)
            {
                return !_chatRooms.Values.Any(cr => cr.Name == chatRoomName);
            }
        }

        private bool CanCreatePrivateChatRoom(string chatRoomName)
        {
            lock (chatRoomLock)
            {
                return !_chatRooms.Values.Any(cr => cr.Name.Equals(chatRoomName));
            }
        }

        public ChatRoom CreateChatRoom(ChatRoomRequest chatRoomRequest) 
        {
            ChatRoom chatRoom;

            lock (chatRoomLock) 
            {
                // allow a user to create a public or private chat room
                // if the room is private and it already exist, just return the id of that room to the user
                // if the room is public, they will can the "can create room" check first
                if (chatRoomRequest.IsPublic)
                {
                    // Create a public room
                    chatRoom = CreateChatRoom(chatRoomRequest);
                }
                else 
                {
                    // Create a private room
                    if (CanCreatePrivateChatRoom(chatRoomRequest.Name))
                    {
                        chatRoom = CreateRoom(chatRoomRequest);
                    }
                    else 
                    {
                        chatRoom = _chatRooms.Values.First(cr => cr.Name.Equals(chatRoomRequest.Name));   
                    }
                }
            }

            return chatRoom; 
        }

        //public void CreatePublicChatRoom(ChatRoomRequest chatRoomRequest, List<Guid> clients)
        //{
        //    lock (chatRoomLock)
        //    {
        //        _ = CreatePublicRoom(chatRoomRequest, clients);

        //        lock (_clientCallbacks)
        //        {
        //            foreach (IChatManagerCallback callback in _clientCallbacks.Values)
        //            {
        //                callback.UpdatePublicChatRooms(new ObservableCollection<ChatRoom>(_chatRooms.Values));
        //            }
        //        }
        //    }
        //}

        private ObservableCollection<ChatRoom> GetAvailableRooms()
        {
            lock (chatRoomLock)
            {
                return new ObservableCollection<ChatRoom>(_chatRooms.Values);
            }
        }

        private ChatRoom CreateRoom(ChatRoomRequest chatRoomRequest) 
        {
            ChatRoom room = new ChatRoom()
            {
                Id = Guid.NewGuid(),
                Clients = new List<Client>(chatRoomRequest.Clients),
                IsPublic = chatRoomRequest.IsPublic,
                Name = chatRoomRequest.Name,
                SenderTitle = !chatRoomRequest.IsPublic ? chatRoomRequest.Clients.First().Name : null,
                ReceiverTitle = !chatRoomRequest.IsPublic ? chatRoomRequest.Clients.Last().Name : null,
                Messages = new List<Message>()
            };

            // Ensure that no one that is reading will get incorrect data         
            _chatRooms.Add(room.Id, room);

            return room;
        }

        //private ChatRoom CreatePublicRoom(ChatRoomRequest chatRoomRequest, List<Guid> clients)
        //{
        //    ChatRoom room = new ChatRoom()
        //    {
        //        Id = chatRoomRequest.ChatRoomId,
        //        Clients = clients,
        //        IsPublic = true,
        //        Name = chatRoomRequest.Name,
        //    };

        //    // Ensure that no one that is reading will get incorrect data         
        //    _chatRooms.Add(room.Id, room);

        //    return room;
        //}
    }
}
