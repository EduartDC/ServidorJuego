using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{
    [ServiceContract(CallbackContract = typeof(INotificationServiceCallback))]
    interface INotificationService
    {
        [OperationContract]
        void SetCallBack(String username);

        [OperationContract]
        void NotificationUsers(string name, string code);
    }

    [ServiceContract]
    public interface INotificationServiceCallback
    {

        [OperationContract(IsOneWay = true)]
        void notification(string username, string code);

        [OperationContract(IsOneWay = true)]
        void LoadLobby(List<PlayerServer> players, string code);

    }
}

