//using DataBase;
using DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{
    [ServiceContract]
    internal interface IMatchService
    {
        [OperationContract]
        void StartLobby(List<Player> players, Match newMatch);

        [OperationContract]
        void CreatetMatch(Match newMatch);

        [OperationContract]
        void DisconnectFromMatch(Match match);

        [OperationContract]
        void ConnectToMatch(Match match);
    }
}
