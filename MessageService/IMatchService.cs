
using MessageService.Domain;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{

    [ServiceContract(CallbackContract = typeof(IMatchServiceCallBack), SessionMode = SessionMode.Required)]
    internal interface IMatchService
    {
        [OperationContract(IsOneWay = true)]
        void StartLobby(string username, string code);

        [OperationContract(IsOneWay = true)]
        void AddToLobby(string username, string code);

        [OperationContract(IsOneWay = true)]
        void StartMatch(string code);

        [OperationContract(IsOneWay = true)]
        void DisconnectFromLobby(string username, string code);

        [OperationContract(IsOneWay = true)]
        void DisconnectFromMatch(PlayerServer player);

        [OperationContract(IsOneWay = true)]
        void SetCallbackMatch(string username);

    }

    [ServiceContract]
    public interface IMatchServiceCallBack
    {

        [OperationContract(IsOneWay = true)]
        void UpdateLobby(List<PlayerServer> plyers);

        [OperationContract(IsOneWay = true)]
        void LoadMatch(MatchServer match);


    }
}
