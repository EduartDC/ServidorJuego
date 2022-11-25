
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
    internal interface IGameService
    {

        [OperationContract]
        List<QuestionServer> GetQuestions();

        [OperationContract]
        List<AnswerServer> GetAnswers(QuestionServer question);

        [OperationContract]
        int addPoints(PlayerServer player, int score);

        [OperationContract]
        void UpdateBoard();

        [OperationContract]
        void UpdateStrikes();

    }
}
