using ChatAppServiceLibrary.DataContracts;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
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
