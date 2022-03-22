using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServiceLibrary.DataContracts
{
    [DataContract]
    public class ChatRoomRequest
    {
        [DataMember]
        public string ServerName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public List<Client> Clients { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }

    }
}
