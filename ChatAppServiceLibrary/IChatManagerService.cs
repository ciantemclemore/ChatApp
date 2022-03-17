using ChatAppServiceLibrary.DataContracts;
using System;
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

        [OperationContract(IsOneWay = true)]
        void JoinTitle(Client client, Guid titleId);

        [OperationContract]
        bool CreateChatRoom(string chatRoomName);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Logout(string userName);

        [OperationContract]
        bool IsLoggedIn(string userName);
    }
}
