using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChatManagerService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IChatManagerCallback), SessionMode = SessionMode.Required)]
    public interface IChatManagerService
    {
        [OperationContract(IsInitiating = true)]
        bool Login(Client client);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract(IsOneWay = false)]
        bool CanCreateChatRoom(Guid chatRoomId, string chatRoomName);
        
        [OperationContract(IsOneWay = true)]
        void CreatePublicChatRoom(ChatRoomRequest chatRoomRequest, List<Guid> clients);

        [OperationContract(IsOneWay = true)]
        void JoinChatRoom(Guid chatRoomId, Client client);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Logout(string userName);
    }
}
