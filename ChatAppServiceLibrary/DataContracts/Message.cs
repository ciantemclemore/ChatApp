﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServiceLibrary.DataContracts
{
    [DataContract]
    public class Message
    {
        public Message() 
        {
        }

        public Message(Guid senderId, Guid chatRoomId, string content) 
        {
            SenderId = senderId;
            ChatRoomId = chatRoomId;
            Content = content;
        }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public Guid SenderId { get; set; }

        [DataMember]
        public Guid ChatRoomId { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [DataMember]
        public string Content { get; set; }
    }
}
