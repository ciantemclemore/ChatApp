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

        public ChatRoom(string roomName = "") 
        {
            Name = roomName;
        }


        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Client> Clients { get; set; } = new List<Client>();

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
