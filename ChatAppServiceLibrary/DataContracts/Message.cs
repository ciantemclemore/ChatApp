using System;
using System.Runtime.Serialization;

namespace ChatAppServiceLibrary.DataContracts
{
    /// <summary>
    /// Represents a message that is delivered to a chat room
    /// </summary>
    [DataContract]
    public class Message
    {
        public Message() 
        {
        }

        public Message(Client sender, Client receiver, Guid chatRoomId, string content) 
        {
            Sender = sender;
            Receiver = receiver;  
            ChatRoomId = chatRoomId;
            Content = content;
        }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public Client Sender { get; set; }

        [DataMember]
        public Client Receiver { get; set; }

        [DataMember]
        public string ChatRoomName { get; set; }

        [DataMember]
        public Guid ChatRoomId { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [DataMember]
        public string Content { get; set; }
    }
}
