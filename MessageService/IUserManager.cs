
using MessageService.Domain;
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
        int AddPlayer(PlayerServer player);

        [OperationContract]
        int ValidatePlayer(PlayerServer player);

        [OperationContract]
        int ValidateEmailPlayer(PlayerServer player);

        [OperationContract]
        int ValidateUserNamePlayer(PlayerServer player);

        [OperationContract]
        int UpdatePlayer(PlayerServer newPlayer);

        [OperationContract]
        PlayerServer SearchPlayer(String userName);

        [OperationContract]
        FriendServer GetFriend(int idFriend);
    }

   




}
