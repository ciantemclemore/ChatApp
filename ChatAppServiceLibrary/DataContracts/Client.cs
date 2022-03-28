using System;
using System.Runtime.Serialization;

namespace ChatAppServiceLibrary.DataContracts
{
    /// <summary>
    /// Model that represents a client in the chat application.
    /// </summary>
    [DataContract]
    public class Client
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}
