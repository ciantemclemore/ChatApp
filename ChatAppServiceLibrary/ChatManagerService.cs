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
            if (message.Receiver != null)
            {
                // Create an actual room on the server for both the sender and receiver so they can reference it locally
                bool isReceiverOnline = IsLoggedIn(message.Receiver.Id);
                var senderCallback = _clientCallbacks[message.Sender.Id];
                var receiverCallback = _clientCallbacks[message.Receiver.Id];

                // If the user still exists we can send the message
                if (isReceiverOnline)
                {
                    if (CanCreatePrivateChatRoom(message.Sender, message.Receiver))
                    {
                        ChatRoomRequest chatRoomRequest = new ChatRoomRequest()
                        {
                            Name = $"{message.Sender.Id} {message.Receiver.Id}",
                            SenderTitle = message.Sender.Name,
                            ReceiverTitle = message.Receiver.Name,
                        };

                        // Create the chat room for the two clients
                        CreatePrivateChatRoom(chatRoomRequest, message.Sender, message.Receiver);

                        // send the message to both clients
                        senderCallback.ReceiveMessage(message);
                        receiverCallback.ReceiveMessage(message);

                    }
                    // what do you do if you can't create the room?
                }
                else 
                {
                    // send the message only back to the sender
                    senderCallback.ReceiveMessage(message);
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
                List<ChatRoom> chatRoomsToLeave = _chatRooms.Values.Where(cr => cr.IsPublic && !cr.Name.Equals(chatRoomId) && cr.Clients.Any(c => c == client.Id)).ToList();

                // Add the client to the new room
                if (chatRoomToJoin != null)
                {
                    chatRoomToJoin.Clients.Add(client.Id);
                }

                // Clients can only be in one public room at a time, remove the client from the previous room
                if (chatRoomsToLeave.Any())
                {
                    foreach (ChatRoom chatRoom in chatRoomsToLeave)
                    {
                        chatRoom.Clients.Remove(client.Id);
                    }
                }
            }
        }

        public bool CanCreateChatRoom(Guid chatRoomId, string chatRoomName)
        {
            lock (chatRoomLock)
            {
                return !_chatRooms.Values.Any(cr => cr.Name == chatRoomName);
            }
        }

        private bool CanCreatePrivateChatRoom(Client sender, Client receiver)
        {
            lock (chatRoomLock)
            {
                return !_chatRooms.Values.Any(cr => cr.Name.Equals($"{sender.Id} {receiver.Id}") || cr.Name.Equals($"{receiver.Id} {sender.Id}"));
            }
        }

        private void CreatePrivateChatRoom(ChatRoomRequest chatRoomRequest, Client sender, Client receiver)
        {
            lock (chatRoomLock)
            {
                //bool anyRooms = _chatRooms.Values.Any(cr => !cr.IsPublic && cr.Clients.All(id => id == sender.Id || id == receiver.Id));

                //if (!anyRooms)
                //{

                //}
                ChatRoom createdRoom = CreateRoom(chatRoomRequest, new List<Guid>() { sender.Id, receiver.Id }, false);

                lock (callbackLock)
                {
                    IChatManagerCallback senderCallback = _clientCallbacks[sender.Id];
                    IChatManagerCallback receiverCallback = _clientCallbacks[receiver.Id];
                    senderCallback.CreatePrivateRoom(createdRoom);
                    receiverCallback.CreatePrivateRoom(createdRoom);
                }
            }
        }

        public void CreatePublicChatRoom(ChatRoomRequest chatRoomRequest, List<Guid> clients)
        {
            lock (chatRoomLock)
            {
                _ = CreateRoom(chatRoomRequest, clients, true);

                lock (_clientCallbacks)
                {
                    foreach (IChatManagerCallback callback in _clientCallbacks.Values)
                    {
                        callback.UpdatePublicChatRooms(new ObservableCollection<ChatRoom>(_chatRooms.Values));
                    }
                }
            }
        }

        private ObservableCollection<ChatRoom> GetAvailableRooms()
        {
            lock (chatRoomLock)
            {
                return new ObservableCollection<ChatRoom>(_chatRooms.Values);
            }
        }

        private ChatRoom CreateRoom(ChatRoomRequest chatRoomRequest, List<Guid> clients, bool isPublic)
        {
            ChatRoom room = new ChatRoom()
            {
                Id = Guid.NewGuid(),
                Clients = clients,
                IsPublic = isPublic,
                Name = chatRoomRequest.Name,
                SenderTitle = chatRoomRequest.SenderTitle,
                ReceiverTitle = chatRoomRequest.ReceiverTitle
            };

            // Ensure that no one that is reading will get incorrect data         
            _chatRooms.Add(room.Id, room);

            return room;
        }
    }
}
