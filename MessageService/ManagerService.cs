﻿
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

       

        public int ValidatePlayer(PlayerServer player)
        {
            var result = 0;
            using (var connection = new DataContext())
            {
                var playerList = (from Player in connection.Players
                                  where Player.userName.Equals(player.userName) && Player.password.Equals(player.password)
                                  select Player).FirstOrDefault();

                if (playerList != null)
                {
                    result = 1;

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

        public void StartLobby(List<PlayerServer> players, MatchServer newMatch)
        {
            throw new NotImplementedException();
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
        Dictionary<PlayerServer, IChatServiceCallback> playersCallback = new Dictionary<PlayerServer, IChatServiceCallback>();
        Dictionary<int, List<MessageServer>> messages = new Dictionary<int, List<MessageServer>>();
        List<PlayerServer> playersInchat = new List<PlayerServer>();

        public IChatServiceCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();

            }
        }

        object syncObj = new object();

        private bool SearchClientsByName(string name)
        {
            foreach (PlayerServer c in playersCallback.Keys)
            {
                if (c.userName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void Connect(PlayerServer player, int idMatch)
        {

            if (!playersCallback.ContainsValue(CurrentCallback) && !SearchClientsByName(player.userName))
            {   
                lock (syncObj)
                { 
                    playersCallback.Add(player, CurrentCallback);
                    playersInchat.Add(player);
                    SetMessages(idMatch);
                }

            }
           
        }

        public void Disconnect(PlayerServer player)
        {
            foreach (PlayerServer c in playersCallback.Keys)
            {
                if (player.userName == c.userName)
                {
                    lock (syncObj)
                    {
                        this.playersCallback.Remove(c);
                        this.playersInchat.Remove(c);
                        foreach (IChatServiceCallback callback in playersCallback.Values)
                        {
                            callback.RefreshClients(this.playersInchat);
                            callback.UserLeave(player);
                        }

                        return;
                    }
                }
            }
        }

        public void Say(int idMatch, MessageServer msg)
        {
            try
            {
                        messages[idMatch].Add(msg);
                        SetMessages(idMatch);
                
            }
            catch (Exception commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message + commProblem.StackTrace);
                Console.Read();
            }
            
        }

        public void SetMessages(int idMatch)
        {
            foreach (var expectPalyer in playersInchat)
            {
                
                if (playersCallback.ContainsKey(expectPalyer))
                {
                    try
                    {
                        OperationContext.Current.GetCallbackChannel<IChatServiceCallback>().Receive(GetMessages(idMatch));
                    }
                    catch (Exception commProblem)
                    {
                        Console.WriteLine("There was a communication problem. " + commProblem.Message + commProblem.StackTrace);
                        Console.Read();
                    }
                    
                }
            }
        }

        public List<MessageServer> GetMessages(int idMatch)
        {

                if(!messages.ContainsKey(idMatch))
                {
                List<MessageServer> messageList = new List<MessageServer>();
                    messages.Add(idMatch, messageList);
                }
 
            return messages[idMatch];
        }

        public void Whisper(MessageServer msg, PlayerServer receiver)
        {
            foreach (PlayerServer rec in playersCallback.Keys)
            {
                if (rec.userName == receiver.userName)
                {
                    IChatServiceCallback callback = playersCallback[rec];
                    callback.ReceiveWhisper(msg, rec);

                    foreach (PlayerServer sender in playersCallback.Keys)
                    {
                        if (sender.userName == msg.Sender)
                        {
                            IChatServiceCallback senderCallback = playersCallback[sender];
                            senderCallback.ReceiveWhisper(msg, rec);
                            return;
                        }
                    }
                }
            }
        }
    }
}
