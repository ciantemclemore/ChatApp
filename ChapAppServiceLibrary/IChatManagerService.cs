using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChapAppServiceLibrary
{
    [ServiceContract]
    public interface IChatManagerService
    {
        [OperationContract]
        bool Login(string userName);

        [OperationContract(IsOneWay = true)]
        void Logout(string userName);

        [OperationContract]
        bool IsLoggedIn(string userName);
    }
}
