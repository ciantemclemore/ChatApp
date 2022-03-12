using ChatAppServiceLibrary.DataContracts;
using System.ServiceModel;

namespace ChatAppServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChatManagerService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IChatManagerCallback), SessionMode = SessionMode.Required)]
    public interface IChatManagerService
    {
        [OperationContract]
        bool Login(Client client);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void Logout(string userName);

        [OperationContract]
        bool IsLoggedIn(string userName);
    }
}
