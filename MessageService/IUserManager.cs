




//using DataBase;
using DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MessageService
{

    [ServiceContract]
    interface IUserManager
    {

        [OperationContract]
        int AddPlayer(Player player);

        [OperationContract]
        int ValidatePlayer(Player player);

        [OperationContract]
        int ValidateEmailPlayer(Player player);

        [OperationContract]
        int ValidateUserNamePlayer(Player player);

        [OperationContract]
        int UpdatePlayer(Player newPlayer);

        [OperationContract]
        Player SearchPlayer(String userName);
    }

    
}
