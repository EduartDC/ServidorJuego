
using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{

    [ServiceContract(CallbackContract = typeof(IMatchServiceCallBack))]
    internal interface IMatchService
    {
        [OperationContract]
        void StartLobby(PlayerServer player, string code);
        [OperationContract]
        void SendInvitation(PlayerServer friend, string code);
        [OperationContract]
        void AddToLobby(PlayerServer friend, string code);

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

    [ServiceContract]
    public interface IMatchServiceCallBack
    {
        [OperationContract(IsOneWay = true)]
        void ShowInvitation(PlayerServer friend, string code);
        [OperationContract(IsOneWay = true)]
        void UpdateLobby(PlayerServer friend, string code);
    }
}
