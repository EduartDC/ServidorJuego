
using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback), SessionMode = SessionMode.Required)]
    public interface IGameService
    {

        [OperationContract(IsOneWay = true)]
        void StartRound(MatchServer match);

        [OperationContract(IsOneWay = true)]
        void SetCallbackGame(string userName);

        [OperationContract(IsOneWay = true)]
        void YouTurn(string userName, string code);

        [OperationContract(IsOneWay = true)]
        void SetBoard(MatchServer matchServer, AnswerServer answerServer);

        [OperationContract(IsOneWay = true)]
        void EndMatch(MatchServer match);

        [OperationContract(IsOneWay = true)]
        void AddStrikes(int strikesOne, int strikesTwo, string code);
    }

    [ServiceContract]
    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SetRound(QuestionServer question, List<AnswerServer> answers, MatchServer match);

        [OperationContract(IsOneWay = true)]
        void UpdateMatch(MatchServer match, AnswerServer answerServer);

        [OperationContract(IsOneWay = true)]
        void SetTurn(string userName);

        [OperationContract(IsOneWay = true)]
        void EndTurn(string userName);

        [OperationContract(IsOneWay = true)]
        void ExitMatch(MatchServer match);

        [OperationContract(IsOneWay = true)]
        void SetStrikes(int stikesOne, int strikesTwo);
    }
}
