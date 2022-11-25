
using MessageService.Domain;
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
        void StartLobby(List<PlayerServer> players, MatchServer newMatch);

        [OperationContract]
        void CreatetMatch(MatchServer newMatch);

        [OperationContract]
        void DisconnectFromMatch(MatchServer match);

        [OperationContract]
        void ConnectToMatch(MatchServer match);

        [OperationContract]
        AnswerServer GetAnswer(int idAnswer);

        [OperationContract]
        QuestionServer GetQuestion(int idQuestion);

        [OperationContract]
        MatchServer GetMatch(int idMatch);
    }
}
