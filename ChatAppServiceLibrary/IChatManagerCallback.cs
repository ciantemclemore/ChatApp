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
        void GetOnlineClients(ObservableCollection<Client> clients);
    }
}
