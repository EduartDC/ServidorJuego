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
        int AddPlayer(Player newPlayer);

        [OperationContract]
        int ValidatePlayer(Player player);

    }

   
}
