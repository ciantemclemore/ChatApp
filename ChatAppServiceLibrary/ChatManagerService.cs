using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

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
                if (!_clients.Any(c => c.Name.Equals(client.Name, StringComparison.OrdinalIgnoreCase)))
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
                    IChatManagerCallback receiverCallback;

                    if (isReceiverOnline)
                    {
                        // get the receiver callback since they are online
                        receiverCallback = _clientCallbacks[message.Receiver.Id];
                        
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
            Client client = null;

            lock (clientLock)
            {
                client = _clients.First(c => c.Name.Equals(userName));
                _ = _clients.Remove(client);
            }

            lock (callbackLock)
            {
                _ = _clientCallbacks.Remove(client.Id);

                foreach (var callback in _clientCallbacks)
                {
                    callback.Value.UpdateOnlineClients(_clients);
                }
            }

            lock (chatRoomLock)
            {
                IEnumerable<ChatRoom> chatRooms = _chatRooms.Values.Where(cr => cr.Clients.Any(c => c.Id == client.Id));

                // Remove the logged out user from all the chatrooms they are in
                foreach (var chatRoom in chatRooms)
                {
                    Client clientToRemove = chatRoom.Clients.FirstOrDefault(c => c.Id == client.Id);

                    if (clientToRemove != null)
                    {
                        _ = chatRoom.Clients.Remove(clientToRemove);
                    }
                }
            }
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

                List<ChatRoom> chatRoomsToLeave = _chatRooms.Values.Where(cr => cr.IsPublic && !cr.DisplayName.Equals(chatRoomToJoin?.DisplayName)).ToList();

                // Clients can only be in one public room at a time, remove the client from the previous room
                if (chatRoomsToLeave.Any())
                {
                    foreach (ChatRoom chatRoom in chatRoomsToLeave)
                    {
                        _ = chatRoom.Clients.Remove(client);
                    }
                }

                // Close a chat room if it is completely empty
                foreach (ChatRoom chatRoom in _chatRooms.Values) 
                {
                    if (!chatRoom.Clients.Any()) 
                    {
                        _ = _chatRooms.Remove(chatRoom.Id);
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

        private bool CanCreatePublicChatRoom(string chatRoomName)
        {
            return _chatRooms.Values.Any(cr => cr.DisplayName.Equals(chatRoomName, StringComparison.OrdinalIgnoreCase));
        }

        private bool CanCreatePrivateChatRoom(Client sender, Client receiver)
        {
            return _chatRooms.Values.Any(cr => cr.ServerName.Equals($"{sender.Id} {receiver.Id}") || cr.ServerName.Equals($"{receiver.Id} {sender.Id}"));
        }

        public bool CreatePublicChatRoom(ChatRoomRequest chatRoomRequest)
        {
            lock (chatRoomLock)
            {
                if (!CanCreatePublicChatRoom(chatRoomRequest.DisplayName))
                {
                    _ = CreateRoom(chatRoomRequest);

                    lock (_clientCallbacks)
                    {
                        foreach (var callback in _clientCallbacks.Values)
                        {
                            callback.UpdatePublicChatRooms(new ObservableCollection<ChatRoom>(_chatRooms.Values.Where(cr => cr.IsPublic)));
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ChatRoom CreatePrivateChatRoom(ChatRoomRequest chatRoomRequest)
        {
            ChatRoom chatRoom;

            lock (chatRoomLock)
            {
                // Create a private room
                if (!CanCreatePrivateChatRoom(chatRoomRequest.Clients[0], chatRoomRequest.Clients[1]))
                {
                    chatRoom = CreateRoom(chatRoomRequest);
                }
                else
                {
                    Guid senderId = chatRoomRequest.Clients[0].Id;
                    Guid receiverId = chatRoomRequest.Clients[1].Id;
                    chatRoom = _chatRooms.Values.First(cr => cr.ServerName.Equals($"{senderId} {receiverId}") || cr.ServerName.Equals($"{receiverId} {senderId}"));
                }
            }

            return chatRoom;
        }

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
                DisplayName = chatRoomRequest.DisplayName,
                ServerName = chatRoomRequest.ServerName,
                SenderTitle = !chatRoomRequest.IsPublic ? chatRoomRequest.Clients.First().Name : null,
                ReceiverTitle = !chatRoomRequest.IsPublic ? chatRoomRequest.Clients.Last().Name : null,
                Messages = new List<Message>()
            };

            // Ensure that no one that is reading will get incorrect data         
            _chatRooms.Add(room.Id, room);

            return room;
        }
    }
}
