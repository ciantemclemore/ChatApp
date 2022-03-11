using System;
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
        public Message(Guid senderId, Guid receiverId, string content) 
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
        }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public Guid SenderId { get; set; }

        [DataMember]
        public Guid? ReceiverId { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [DataMember]
        public string Content { get; set; }
    }
}
