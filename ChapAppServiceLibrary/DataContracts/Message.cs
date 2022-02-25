using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChapAppServiceLibrary.DataContracts
{
    [DataContract]
    public class Message
    {
        public Message(string userName, string content, string receiverName = "") 
        {
            UserName = userName;
            Content = content;
            ReceiverName = receiverName;
        }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string ReceiverName { get; set; }

        [DataMember]
        public DateTime Created { get; set; } = DateTime.Now;

        [DataMember]
        public string Content { get; set; }
    }
}
