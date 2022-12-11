
using DataAcces;
using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MessageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple)]

    public partial class ManagerService : IUserManager
    {
        List<PlayerServer> usersOnline = new List<PlayerServer>();

        public List<PlayerServer> MatchingFriends(int ownerFriendID)
        {
            using (var connection = new DataContext())
            {
                var friendsFromDataBase = connection.Friends;

                List<PlayerServer> friendsForClient = new List<PlayerServer>();

                foreach (var objectFriendForeach in friendsFromDataBase)
                {
                    if (objectFriendForeach.ownerPlayer == ownerFriendID)
                    {
                        friendsForClient.Add(GetFriend(objectFriendForeach.gameFriend));
                    }
                }

                return friendsForClient;
            }
        }

        public int AddFriend(FriendServer newFriend)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                Friend friend = new Friend();
                friend.idFriend = newFriend.idFriend;
                friend.gameFriend = newFriend.gameFriend;
                friend.ownerPlayer = newFriend.ownerPlayer;
                friend.creationDate = newFriend.creationDate;

                connection.Friends.Add(friend);
                result = connection.SaveChanges();
            }
            return result;
        }

        public int AddPlayer(PlayerServer newPlayer)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                Player player = new Player();
                player.idPlayer = newPlayer.idPlayer;
                player.firstName = newPlayer.firstName;
                player.lastName = newPlayer.lastName;
                player.email = newPlayer.email;
                player.userName = newPlayer.userName;
                player.password = newPlayer.password;
                player.status = newPlayer.status;

                connection.Players.Add(player);
                result = connection.SaveChanges();

            }
            return result;
        }

        public PlayerServer GetFriend(int idFriend)
        {
            using (var connection = new DataContext())
            {
                var friend = (from user in connection.Players
                              where user.idPlayer.Equals(idFriend)
                              select user).First();

                PlayerServer player = new PlayerServer
                {
                    idPlayer = friend.idPlayer,
                    firstName = friend.firstName,
                    lastName = friend.lastName,
                    email = friend.email,
                    userName = friend.userName,
                };
                return player;
            }
        }

        public PlayerServer SearchPlayer(String userName)
        {

            using (var connection = new DataContext())
            {
                try
                {
                    var players = (from user in connection.Players
                                   where user.userName.Equals(userName)
                                   select user).First();
                    PlayerServer player = new PlayerServer
                    {
                        idPlayer = players.idPlayer,
                        firstName = players.firstName,
                        lastName = players.lastName,
                        email = players.email,
                        userName = players.userName,
                        password = players.password,
                        status = players.status,
                    };
                    return player;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }


        }

        public int UpdatePlayer(PlayerServer newPlayer)
        {

            using (var connection = new DataContext())
            {
                var firstName = newPlayer.firstName;
                var lastName = newPlayer.lastName;
                var userName = newPlayer.userName;
                var password = newPlayer.password;
                try
                {
                    var player = connection.Players.Find(newPlayer.idPlayer);

                    player.firstName = firstName;
                    player.lastName = lastName;
                    player.userName = userName;
                    player.password = password;

                    var result = connection.SaveChanges();

                    return result;
                }
                catch (DbUpdateException)
                {
                    return 0;
                }
            }
        }

        public int ValidateEmailPlayer(PlayerServer player)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                var playerList = (from Player in connection.Players
                                  where Player.email.Equals(player.email)
                                  select Player).FirstOrDefault();

                if (playerList != null)
                {
                    result = 1;

                }
            }
            return result;
        }



        public int UserConnect(PlayerServer dataPlayer)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                var player = (from Player in connection.Players
                              where Player.userName.Equals(dataPlayer.userName) && Player.password.Equals(dataPlayer.password)
                              select Player).FirstOrDefault();



                if (player != null)
                {
                    result = 1;
                    PlayerServer playerServer = new PlayerServer();
                    playerServer.idPlayer = player.idPlayer;
                    playerServer.firstName = player.firstName;
                    playerServer.lastName = player.lastName;
                    playerServer.email = player.email;
                    playerServer.userName = player.userName;
                    playerServer.password = player.password;
                    playerServer.userCallBack = OperationContext.Current.GetCallbackChannel<IUserManagerCallBack>();
                    usersOnline.Add(playerServer);
                }
            }
            return result;
        }

        public int ValidateUserNamePlayer(PlayerServer player)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                var playerList = (from Player in connection.Players
                                  where Player.userName.Equals(player.userName)
                                  select Player).FirstOrDefault();

                if (playerList != null)
                {
                    result = 1;

                }
            }
            return result;
        }
    }
    public partial class ManagerService : IGameService
    {
        public int addPoints(PlayerServer player, int score)
        {
            throw new NotImplementedException();
        }

        public List<AnswerServer> GetAnswers(QuestionServer question)
        {
            throw new NotImplementedException();
        }

        public List<QuestionServer> GetQuestions()
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard()
        {
            throw new NotImplementedException();
        }

        public void UpdateStrikes()
        {
            throw new NotImplementedException();
        }
    }

    public partial class ManagerService : IChatService
    {
        Dictionary<string, List<PlayerServer>> lobbys = new Dictionary<string, List<PlayerServer>>();
        List<PlayerServer> playersInLobby = new List<PlayerServer>();

        private bool SearchPlayers(string name)
        {
            foreach (PlayerServer player in playersInLobby)
            {
                if (player.userName == name)
                {

                    return true;

                }
            }

            return false;

        }

        public void Connect(PlayerServer player, string code)
        {
            if (!SearchPlayers(player.userName))
            {
                player.chatCallback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
                playersInLobby.Add(player);

                foreach (var players in playersInLobby)
                {
                    players.chatCallback.UserJoin(player);
                    players.chatCallback.RefreshClients(playersInLobby);
                }
            }
        }

        public void Disconnect(PlayerServer player)
        {
            var result = false;
            foreach (var players in playersInLobby)
            {
                if (players.userName.Equals(player.userName))
                {
                    playersInLobby.Remove(players);
                    result = true;
                    break;
                }
            }
            if (result)
            {
                foreach (var players in playersInLobby)
                {
                    players.chatCallback.UserLeave(player);
                    players.chatCallback.RefreshClients(playersInLobby);
                }
            }

        }


        public void Say(int idMatch, MessageServer msg)
        {
            foreach (var players in playersInLobby)
            {
                players.chatCallback.Receive(msg);
            }

        }

        public void Whisper(MessageServer msg, string player)
        {
            PlayerServer newPlayer = null;
            foreach (var players in playersInLobby)
            {
                if (players.userName.Equals(player))
                {
                    newPlayer = players;
                }
            }
            if (newPlayer != null)
            {
                newPlayer.chatCallback.ReceiveWhisper(msg);
            }
            else
            {
                foreach (var playersIn in playersInLobby)
                {
                    if (playersIn.userName.Equals(msg.Sender))
                    {
                        MessageServer messageServer = new MessageServer();
                        messageServer.Sender = "Sytem";
                        messageServer.Content = "User not foud";

                        playersIn.chatCallback.ReceiveWhisper(messageServer);
                    }
                }
            }
        }
    }

    public partial class ManagerService : IMatchService
    {
        public void ConnectToMatch(MatchServer match)
        {
            throw new NotImplementedException();
        }

        public void CreatetMatch(MatchServer newMatch)
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromMatch(MatchServer match)
        {
            throw new NotImplementedException();
        }

        public AnswerServer GetAnswer(int idAnswer)
        {
            using (var connection = new DataContext())
            {
                var answers = connection.Answers.Find(idAnswer);
                AnswerServer answer = new AnswerServer();
                answer.idAnswer = answers.idAnswer;
                answer.answer = answers.answer1;
                answer.place = answers.place;
                answer.score = answers.score;
                Console.WriteLine(answers.idAnswer);
                return answer;
            }
        }

        public MatchServer GetMatch(int idMatch)
        {
            throw new NotImplementedException();
        }

        public QuestionServer GetQuestion(int idQuestion)
        {
            throw new NotImplementedException();
        }

        public void StartLobby(PlayerServer player, string code)
        {
            List<PlayerServer> players = new List<PlayerServer>();
            players.Add(player);
            lobbys.Add(code, players);
        }

        public void SendInvitation(PlayerServer friend, string code)
        {
            PlayerServer playerServer = null;
            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(friend.userName))
                {
                    playerServer = players;
                }
            }

            if (playerServer != null)
            {
                playerServer.matchCallBack.ShowInvitation(friend, code);

            }
        }

        public void AddToLobby(PlayerServer friend, string code)
        {
            throw new NotImplementedException();
        }
    }
}

