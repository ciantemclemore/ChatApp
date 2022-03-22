using System;
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
    }
}
