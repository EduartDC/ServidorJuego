
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
        void UpdateBoard(MatchServer match, string username);

        [OperationContract(IsOneWay = true)]
        void StartRaund(string username, string code);

        [OperationContract(IsOneWay = true)]
        void SetCallbackGame(string username);

        [OperationContract(IsOneWay = true)]
        void YouTurn(string username, string code);
    }

    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SetRound(QuestionServer question, List<AnswerServer> answers);

        [OperationContract(IsOneWay = true)]
        void UpdateMatch(MatchServer match);

        [OperationContract(IsOneWay = true)]
        void SetTurn();

        [OperationContract(IsOneWay = true)]
        void ExitMatch();
    }
}
