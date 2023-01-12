
using DataAcces;
using MessageService.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Net;
using System.ServiceModel.Channels;

namespace MessageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple)]

    public partial class ManagerService : IUserManager, INotificationService
    {
        List<PlayerServer> usersOnline = new List<PlayerServer>();
        int errorConnection = 404;
        int invalid = 0;
        int valid = 1;

        public bool VerifyConnection()
        {
            var result = false;
            var connection = new DataContext();
            try
            {
                connection.Database.Connection.Open();
                connection.Database.Connection.Close();

                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public List<PlayerServer> MatchingFriends(string username)
        {
            using (var connection = new DataContext())
            {
                var friends = new List<PlayerServer>();
                if (VerifyConnection())
                {
                    var player = (from user in connection.Players
                                  where user.userName.Equals(username)
                                  select user).First();
                    var listFriends = (from user in connection.Friends
                                       where user.ownerPlayer.Equals(player.idPlayer)
                                       select user).ToList();
                    foreach (var friendServer in listFriends)
                    {
                        foreach (var playerOnline in usersOnline)
                        {
                            if (friendServer.gameFriend.Equals(playerOnline.idPlayer))
                            {
                                friends.Add(playerOnline);
                            }
                        }
                    }
                }
                else
                {
                    friends = null;
                }
                return friends;
            }
        }

        public int AddFriend(FriendServer newFriend)
        {
            var result = invalid;
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    Friend friendGame = new Friend();

                    friendGame.gameFriend = newFriend.gameFriend;
                    friendGame.ownerPlayer = newFriend.ownerPlayer;
                    friendGame.creationDate = newFriend.creationDate;
                    connection.Friends.Add(friendGame);
                    result = connection.SaveChanges();

                    Friend friendsOwner = new Friend();
                    friendsOwner.gameFriend = newFriend.ownerPlayer;
                    friendsOwner.ownerPlayer = newFriend.gameFriend;
                    friendsOwner.creationDate = newFriend.creationDate;

                    connection.Friends.Add(friendsOwner);
                    result = connection.SaveChanges();

                }
                else
                {
                    result = errorConnection;
                }
                return result;
            }

        }

        public int AddPlayer(PlayerServer player)
        {
            var result = invalid;
            var validate = false;
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var validatePlayers = new List<Player>();

                    validatePlayers = connection.Players.ToList();

                    foreach (var players in validatePlayers)
                    {
                        if (players.userName.Equals(player.userName))
                        {
                            validate = true;
                        }
                    }

                    if (!validate)
                    {
                        Player newplayer = new Player();
                        newplayer.idPlayer = player.idPlayer;
                        newplayer.firstName = player.firstName;
                        newplayer.lastName = player.lastName;
                        newplayer.email = player.email;
                        newplayer.userName = player.userName;
                        newplayer.password = player.password;
                        newplayer.status = player.status;

                        connection.Players.Add(newplayer);
                        result = connection.SaveChanges();

                    }

                }
                else
                {
                    result = errorConnection;
                }

                return result;
            }

        }

        public int DeleteFriend(int idPlayer, string username)
        {
            using (var connection = new DataContext())
            {
                var result = invalid;
                if (VerifyConnection())
                {

                    PlayerServer player = new PlayerServer();
                    foreach (var players in usersOnline)
                    {
                        if (players.userName.Equals(username))
                        {
                            player = players;
                            break;
                        }
                    }

                    var friendsDB = (from user in connection.Friends
                                     where user.ownerPlayer.Equals(player.idPlayer)
                                     select user).ToList();

                    foreach (var friends in friendsDB)
                    {
                        if (friends.gameFriend.Equals(idPlayer))
                        {
                            connection.Friends.Remove(friends);
                            result = connection.SaveChanges();

                            break;
                        }

                    }

                }
                else
                {
                    result = errorConnection;
                }
                return result;
            }
        }

        public PlayerServer SearchPlayer(String userName)
        {

            using (var connection = new DataContext())
            {
                PlayerServer player = new PlayerServer();
                if (VerifyConnection())
                {
                    var players = (from user in connection.Players
                                   where user.userName.Equals(userName)
                                   select user).FirstOrDefault();

                    if (players.userName != null)
                    {
                        player.idPlayer = players.idPlayer;
                        player.firstName = players.firstName;
                        player.lastName = players.lastName;
                        player.email = players.email;
                        player.userName = players.userName;
                        player.password = players.password;
                        player.status = players.status;
                    }
                }
                else
                {
                    player = null;
                }
                return player;
            }

        }
        public int UpdatePlayer(PlayerServer newPlayer)
        {
            var result = invalid;
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var firstName = newPlayer.firstName;
                    var lastName = newPlayer.lastName;
                    var userName = newPlayer.userName;
                    var password = newPlayer.password;
                    var status = newPlayer.status;

                    var player = connection.Players.Find(newPlayer.idPlayer);

                    player.firstName = firstName;
                    player.lastName = lastName;
                    player.userName = userName;
                    player.password = password;
                    player.status = status;

                    result = connection.SaveChanges();

                }
                else
                {
                    result = errorConnection;
                }
                return result;
            }
        }

        public int ValidateEmailPlayer(PlayerServer player)
        {
            var result = invalid;
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var playerList = (from Player in connection.Players
                                      where Player.email.Equals(player.email)
                                      select Player).FirstOrDefault();

                    if (playerList != null)
                    {
                        result = valid;

                    }

                }
                else
                {
                    result = errorConnection;
                }
                return result;
            }

        }

        public PlayerServer UserConnect(PlayerServer player)
        {

            PlayerServer playerServer = new PlayerServer();
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {

                    var newPlayer = new Player();

                    newPlayer = (from user in connection.Players
                                 where user.userName.Equals(player.userName) && user.password.Equals(player.password)
                                 select user).FirstOrDefault();

                    if (newPlayer != null)
                    {

                        playerServer.idPlayer = newPlayer.idPlayer;
                        playerServer.firstName = newPlayer.firstName;
                        playerServer.lastName = newPlayer.lastName;
                        playerServer.email = newPlayer.email;
                        playerServer.userName = newPlayer.userName;
                        playerServer.password = newPlayer.password;
                        playerServer.status = newPlayer.status;
                        var list = new List<FriendServer>();
                        foreach (var friend in newPlayer.Friends)
                        {
                            var friendServer = new FriendServer();
                            friendServer.idFriend = friend.idFriend;
                            friendServer.gameFriend = friend.gameFriend;
                            friendServer.ownerPlayer = friend.ownerPlayer;
                            friendServer.creationDate = friend.creationDate;
                            list.Add(friendServer);
                        }
                        playerServer.friends = list;
                        usersOnline.Add(playerServer);
                    }


                }
                else
                {

                    playerServer = null;
                }
            }
            return playerServer;
        }

        public int ValidateUserNamePlayer(PlayerServer player)
        {
            var result = invalid;
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var playerList = (from Player in connection.Players
                                      where Player.userName.Equals(player.userName)
                                      select Player).FirstOrDefault();

                    if (playerList != null)
                    {
                        result = valid;

                    }

                }
                else
                {
                    result = errorConnection;
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

            newPlayer.userCallBack.Notification(name, code);

        }

        public void UserDisconect(string username)
        {
            foreach (var player in usersOnline)
            {
                if (player.userName.Equals(username))
                {
                    usersOnline.Remove(player);
                    break;
                }


            }
        }

        public int ValidateLobby(string code)
        {
            var result = invalid;
            foreach (var lobby in lobbys.Keys)
            {
                if (lobby.Equals(code))
                {
                    result = valid;
                }

            }

            return result;
        }

        public PlayerServer GuestUser()
        {
            using (var connection = new DataContext())
            {
                PlayerServer player = new PlayerServer();
                if (VerifyConnection())
                {
                    var players = (from user in connection.Players
                                   where user.userName.Equals("Guest")
                                   select user).FirstOrDefault();

                    if (players != null)
                    {
                        player.idPlayer = players.idPlayer;
                        player.firstName = players.firstName;
                        player.lastName = players.lastName;
                        player.email = players.email;
                        player.userName = players.userName;
                        player.password = players.password;
                        player.status = players.status;

                        usersOnline.Add(player);
                    }


                }
                else
                {
                    player = null;
                }
                return player;
            }

        }

        public int SendMail(PlayerServer player, string code)
        {
            string smtpServer = ConfigurationManager.AppSettings["SMTP_SERVER"];
            int port = int.Parse(ConfigurationManager.AppSettings["PORT"]);
            string emailAddress = ConfigurationManager.AppSettings["EMAIL_ADDRESS"];
            string password = ConfigurationManager.AppSettings["PASSWORD"];
            string playerEmail = player.email;
            var result = invalid;

            try
            {
                var mailMessage = new MailMessage(emailAddress, playerEmail, "Verificacion de correo en 100 Mexicanos Dijeron", ("Codigo de verificacion" + " " + code + "."))
                {
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailAddress, password),
                    EnableSsl = true,
                };
                smtpClient.Send(mailMessage);
                result = valid;
            }
            catch (SmtpException)
            {
                result = errorConnection;
            }

            return result;
        }
    }

    public partial class ManagerService : IChatService
    {
        List<PlayerServer> playersInChat = new List<PlayerServer>();

        public bool SearchPlayersInChat(string name)
        {
            var result = false;
            if (playersInChat.Count != 0)
            {
                foreach (PlayerServer player in playersInChat)
                {
                    if (player.userName == name)
                    {
                        result = true;
                    }
                }
                result = false;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void Connect(PlayerServer player, string code)
        {
            if (player != null && !SearchPlayersInChat(player.userName))
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

        public void Say(MessageServer message)
        {
            foreach (var players in playersInChat)
            {
                players.chatCallback.Receive(message);

            }

        }

        public void Whisper(MessageServer message, string player)
        {
            PlayerServer newPlayer = null;
            foreach (var players in playersInChat)
            {
                if (players.userName.Equals(player))
                {
                    newPlayer = players;
                }

                if (newPlayer != null)
                {
                    newPlayer.chatCallback.ReceiveWhisper(message);
                }
                else
                {
                    foreach (var playersIn in playersInChat)
                    {
                        if (playersIn.userName.Equals(message.Sender))
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
                        break;
                    }
                }
            }
            else
            {
                foreach (var players in list)
                {
                    if (players.userName.Equals(username))
                    {
                        lobbys[code].Remove(players);
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
            if (username.Equals("Guest"))
            {
                foreach (var player in usersOnline)
                {
                    if (player.userName.Equals(username))
                    {
                        lobbys[code].Add(player);
                    }
                }
                foreach (var players in lobbys[code].ToList())
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
                if (VerifyConnection())
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

                    foreach (var players in list)
                    {
                        players.matchCallBack.LoadMatch(match);

                    }
                }
                else
                {
                    throw new CommunicationObjectFaultedException();
                }
            }
        }

        public void StartRound(MatchServer match)
        {
            QuestionServer questionServer = new QuestionServer();
            List<AnswerServer> answers = new List<AnswerServer>();
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var question = connection.Questions.ToList();
                    var random = new Random();
                    var index = random.Next(0, 24);

                    var questionRandom = question[index];

                    questionServer.idQuestion = questionRandom.idQuestion;
                    questionServer.question = questionRandom.question1;
                    questionServer.questionClass = questionRandom.questionClass;

                    foreach (var answer in questionRandom.Answers)
                    {
                        AnswerServer newAnswer = new AnswerServer();
                        newAnswer.idAnswer = answer.idAnswer;
                        newAnswer.answer = answer.answer1;
                        newAnswer.place = answer.place;
                        newAnswer.score = answer.score;

                        answers.Add(newAnswer);
                    }
                    questionServer.answers = answers;

                    var list = lobbys[match.inviteCode].ToList();
                    foreach (var players in list)
                    {
                        players.gameCallback.SetRound(questionServer, answers, match);

                    }
                }
                else
                {
                    throw new CommunicationObjectFaultedException();
                }
            }

        }

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

        public void SetBoard(MatchServer matchServer, AnswerServer answerServer)
        {

            var list = lobbys[matchServer.inviteCode].ToList();
            foreach (var player in list)
            {
                player.gameCallback.UpdateMatch(matchServer, answerServer);
            }

        }

        public void EndMatch(MatchServer match)
        {
            using (var connection = new DataContext())
            {
                if (VerifyConnection())
                {
                    var newMatch = (from quest in connection.Matches
                                    where quest.inviteCode.Equals(match.inviteCode)
                                    select quest).First();

                    newMatch.scorePlayerOne = match.scorePlayerOne;
                    newMatch.scorePlayerTwo = match.scorePlayerTwo;
                    newMatch.playerWinner = match.playerWinner;

                    connection.SaveChanges();
                }
                else
                {
                    throw new EndpointNotFoundException();
                }

            }

            var list = lobbys[match.inviteCode].ToList();

            foreach (var player in list)
            {
                player.gameCallback.ExitMatch(match);
            }

            lobbys.Remove(match.inviteCode);
        }

        public void AddStrikes(int strikesOne, int strikesTwo, string code)
        {
            var list = lobbys[code].ToList();
            foreach (var player in list)
            {
                player.gameCallback.SetStrikes(strikesOne, strikesTwo);
            }
        }

        public void KickFromLobby(string username, string code)
        {
            var list = lobbys[code].ToList();
            if (list.Count >= 2)
            {
                foreach (var player in list)
                {
                    if (!player.userName.Equals(username))
                    {
                        lobbys[code].Remove(player);
                        player.matchCallBack.Kicked();
                    }

                }

            }

        }
    }
}

