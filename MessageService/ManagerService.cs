
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

    public partial class ManagerService : IUserManager, INotificationService
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
                    playerServer.status = player.status;
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

        public void SetCallBack(string username)
        {

            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(username))
                {
                    players.userCallBack = OperationContext.Current.GetCallbackChannel<INotificationServiceCallback>();

                }
            }

        }

        public void NotificationUsers(string name, string code)
        {

            PlayerServer newPlayer = new PlayerServer();
            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(name))
                {
                    newPlayer = players;
                }
            }

            newPlayer.userCallBack.notification(name, code);

        }
    }

    public partial class ManagerService : IChatService
    {
        List<PlayerServer> playersInChat = new List<PlayerServer>();

        private bool SearchPlayers(string name)
        {
            foreach (PlayerServer player in playersInChat)
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

                playersInChat.Add(player);

                foreach (var players in playersInChat)
                {
                    players.chatCallback.UserJoin(player);
                    players.chatCallback.RefreshClients(playersInChat);
                }
            }
        }

        public void Disconnect(PlayerServer player)
        {
            var result = false;
            foreach (var players in playersInChat)
            {
                if (players.userName.Equals(player.userName))
                {
                    playersInChat.Remove(players);
                    result = true;
                    break;
                }
            }
            if (result)
            {
                foreach (var players in playersInChat)
                {
                    players.chatCallback.UserLeave(player);
                    players.chatCallback.RefreshClients(playersInChat);
                }
            }

        }


        public void Say(int idMatch, MessageServer msg)
        {
            foreach (var players in playersInChat)
            {
                players.chatCallback.Receive(msg);

            }

        }

        public void Whisper(MessageServer msg, string player)
        {
            PlayerServer newPlayer = null;
            foreach (var players in playersInChat)
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
                foreach (var playersIn in playersInChat)
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

    public partial class ManagerService : IMatchService, IGameService
    {
        Dictionary<string, List<PlayerServer>> lobbys = new Dictionary<string, List<PlayerServer>>();

        public void DisconnectFromLobby(string username, string code)
        {
            var list = lobbys[code].ToList();
            if (list.Count == 1)
            {
                foreach (var lobby in lobbys.Keys)
                {
                    if (lobby.Equals(code))
                    {
                        lobbys.Remove(code);
                    }
                }
            }
            else
            {
                foreach (var players in list)
                {
                    if (players.userName.Equals(username))
                    {
                        list.Remove(players);
                        break;
                    }

                }

                foreach (var players in list)
                {
                    players.matchCallBack.UpdateLobby(list);
                }
            }

        }

        public void StartLobby(string username, string code)
        {
            var result = false;
            foreach (var codeInvitation in lobbys.Keys)
            {
                if (codeInvitation.Equals(code))
                {
                    result = true;
                }
            }

            if (!result)
            {
                PlayerServer playerServer = new PlayerServer();
                foreach (var player in usersOnline)
                {
                    if (player.userName.Equals(username))
                    {

                        playerServer = player;
                    }
                }

                List<PlayerServer> players = new List<PlayerServer>();
                players.Add(playerServer);
                lobbys.Add(code, players);
                playerServer.matchCallBack.UpdateLobby(players);
            }
            else
            {
                List<PlayerServer> playerList = new List<PlayerServer>();
                foreach (var codeInvitation in lobbys.Keys)
                {
                    if (codeInvitation.Equals(code))
                    {

                        playerList = lobbys[codeInvitation].ToList();
                    }
                }

                foreach (var players in playerList)
                {
                    players.matchCallBack.UpdateLobby(lobbys[code].ToList());
                }

            }
        }

        public void AddToLobby(string username, string code)
        {
            PlayerServer playerServer = new PlayerServer();
            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(username))
                {

                    playerServer = players;
                }
            }

            lobbys[code].Add(playerServer);

            var list = lobbys[code].ToList();

            playerServer.userCallBack.LoadLobby(list, code);


        }

        public void SetCallbackMatch(string username)
        {
            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(username))
                {
                    players.matchCallBack = OperationContext.Current.GetCallbackChannel<IMatchServiceCallBack>();

                }
            }
        }

        public void StartMatch(string code)
        {

            MatchServer match = new MatchServer();
            var list = lobbys[code].ToList();
            using (var connection = new DataContext())
            {
                List<Player> listPlayers = new List<Player>();
                foreach (var players in list)
                {
                    var player = (from user in connection.Players
                                  where user.userName.Equals(players.userName)
                                  select user).First();

                    listPlayers.Add(player);
                }

                Match newMatch = new Match();
                newMatch.scorePlayerOne = 0;
                newMatch.scorePlayerTwo = 0;
                newMatch.playerWinner = 0;
                newMatch.inviteCode = code;
                newMatch.Players = (ICollection<Player>)listPlayers;

                match.scorePlayerOne = newMatch.scorePlayerOne;
                match.scorePlayerTwo = newMatch.scorePlayerTwo;
                match.playerWinner = newMatch.playerWinner;
                match.inviteCode = code;
                match.players = list;

                connection.Matches.Add(newMatch);
                connection.SaveChanges();
            }

            foreach (var players in list)
            {
                players.matchCallBack.LoadMatch(match);

            }

        }

        public void DisconnectFromMatch(PlayerServer player)
        {
            throw new NotImplementedException();
        }

        public void StartRaund(string username, string code)
        {
            QuestionServer questionServer = new QuestionServer();
            List<AnswerServer> answers = new List<AnswerServer>();
            using (var connection = new DataContext())
            {
                var question = connection.Questions;
                var random = new Random();

                var idQuest = random.Next(0, 25);
                foreach (var quest in question)
                {
                    if (quest.idQuestion == idQuest)
                    {
                        questionServer.idQuestion = quest.idQuestion;
                        questionServer.question = quest.question1;
                        questionServer.questionClass = quest.questionClass;
                        foreach (var answer in quest.Answers)
                        {
                            AnswerServer newAnswer = new AnswerServer();
                            newAnswer.idAnswer = answer.idAnswer;
                            newAnswer.answer = answer.answer1;
                            newAnswer.place = answer.place;
                            newAnswer.score = answer.score;

                            answers.Add(newAnswer);
                        }
                        questionServer.answers = answers;
                        break;
                    }
                }

            }
            var list = lobbys[code].ToList();
            foreach (var players in list)
            {
                players.gameCallback.SetRound(questionServer, answers);

            }
        }

        /*public void UpdateGamek(MatchServer match, AnswerServer correctAnswer)
        {

            try
            {
                var list = match.players;
                foreach (var player in list)
                {
                    player.gameCallback.UpdateMatch(match, correctAnswer);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }*/

        public void SetCallbackGame(string username)
        {
            foreach (var players in usersOnline)
            {
                if (players.userName.Equals(username))
                {
                    players.gameCallback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();

                }
            }
        }

        public void YouTurn(string username, string code)
        {

            var list = lobbys[code].ToList();
            var turnOn = new PlayerServer();
            var turnOff = new PlayerServer();

            foreach (var players in list)
            {
                if (players.userName.Equals(username))
                {
                    turnOn = players;
                }
                else
                {
                    turnOff = players;
                }
            }

            turnOn.gameCallback.SetTurn(turnOn.userName);
            turnOff.gameCallback.EndTurn(turnOff.userName);


        }

        public void NextRound(MatchServer match, string username)
        {
            var code = match.inviteCode;
            var list = lobbys[code].ToList();

            foreach (var player in list)
            {
                player.gameCallback.NewRound(match, username);
            }
        }

        public void SetBoard(MatchServer matchServer, AnswerServer answerServer)
        {
            try
            {
                var list = lobbys[matchServer.inviteCode].ToList();
                foreach (var player in list)
                {
                    player.gameCallback.UpdateMatch(matchServer);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}

