using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChatAppServiceLibrary.DataContracts
{
    /// <summary>
    /// The information required to create a chat room
    /// </summary>
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
