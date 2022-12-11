
using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MessageService
{

    [ServiceContract(CallbackContract = typeof(IUserManagerCallBack))]
    interface IUserManager
    {
        [OperationContract]
        List<PlayerServer> MatchingFriends(int ownerFriend);

        [OperationContract]
        int AddFriend(FriendServer newFriend);

        [OperationContract]
        int AddPlayer(PlayerServer player);

        [OperationContract]
        int UserConnect(PlayerServer player);

        [OperationContract]
        int ValidateEmailPlayer(PlayerServer player);

        [OperationContract]
        int ValidateUserNamePlayer(PlayerServer player);

        [OperationContract]
        int UpdatePlayer(PlayerServer newPlayer);

        [OperationContract]
        PlayerServer SearchPlayer(String userName);

        [OperationContract]
        PlayerServer GetFriend(int idFriend);
    }

    public interface IUserManagerCallBack
    {

    }




}
