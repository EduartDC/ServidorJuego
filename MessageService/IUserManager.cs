
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
        List<PlayerServer> MatchingFriends(string userName);

        [OperationContract]
        int AddFriend(FriendServer newFriend);

        [OperationContract]
        int AddPlayer(PlayerServer player);

        [OperationContract]
        PlayerServer UserConnect(PlayerServer player);

        [OperationContract]
        int ValidateEmailPlayer(PlayerServer player);

        [OperationContract]
        int ValidateUserNamePlayer(PlayerServer player);

        [OperationContract]
        int UpdatePlayer(PlayerServer newPlayer);

        [OperationContract]
        PlayerServer SearchPlayer(String userName);

        [OperationContract]
        int DeleteFriend(int idPlayer, string userName);

        [OperationContract]
        void UserDisconect(string userName);

        [OperationContract]
        int ValidateLobby(string code);

        [OperationContract]
        PlayerServer GuestUser();

        [OperationContract]
        int SendMail(PlayerServer player, string code);


    }




}
