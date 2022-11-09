using DataBase;
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
        List<Question> GetQuestions();

        [OperationContract]
        List<Answer> GetAnswers(Question question);

        [OperationContract]
        int addPoints(Player player, int score);

        [OperationContract]
        void UpdateBoard();

        [OperationContract]
        void UpdateStrikes();

    }
}
