﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServiceLibrary.DataContracts
{
    [DataContract]
    public class ChatRoom
    {
        public ChatRoom() 
        {
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SenderTitle { get; set; }

        [DataMember]
        public string ReceiverTitle { get; set; }

        [DataMember]
        public List<Guid> Clients { get; set; } = new List<Guid>();

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
