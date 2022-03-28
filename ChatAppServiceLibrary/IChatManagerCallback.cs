using ChatAppServiceLibrary.DataContracts;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
    /// <summary>
    /// This callback allows the service to invoke methods on the client
    /// </summary>
    [ServiceContract]
    public interface IChatManagerCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void UpdateOnlineClients(ObservableCollection<Client> clients);

        [OperationContract(IsOneWay = true)]
        void UpdatePublicChatRooms(ObservableCollection<ChatRoom> chatRooms);
    }
}
