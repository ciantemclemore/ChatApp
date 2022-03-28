using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChatAppServiceLibrary.DataContracts
{
    /// <summary>
    /// ChatRooms represent rooms that users can join and send messages to
    /// </summary>
    [DataContract]
    public class ChatRoom
    {
        public ChatRoom() 
        {
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string ServerName { get; set; }

        [DataMember]
        public string SenderTitle { get; set; }

        [DataMember]
        public string ReceiverTitle { get; set; }

        [DataMember]
        public List<Client> Clients { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public List<Message> Messages { get; set; }

        [DataMember]
        public string LastMessage { get; set; }
    }
}
