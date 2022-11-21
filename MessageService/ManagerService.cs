

using DataBase;
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

        public int AddPlayer(Player newPlayer)
        {
            var result = 0;
            using (var connection = new DataConnect())
            {

                    connection.Players.Add(newPlayer);
                    result = connection.SaveChanges();
                
            }
            return result;
        }

        public Player SearchPlayer(String userName)
        {
            
            using (var connection = new DataConnect())
            {

                var players = (from gamer in connection.Players
                               where gamer.userName.Equals(userName)
                               select gamer).First();
                Player player = new Player
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

            
        }

        public int UpdatePlayer(Player newPlayer)
        {
            
            using (var connection = new DataConnect())
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

        public int ValidateEmailPlayer(Player player)
        {
            var result = 0;
            using (var connection = new DataConnect())
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

       

        public int ValidatePlayer(Player player)
        {
            var result = 0;
            using (var connection = new DataConnect())
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

        public int ValidateUserNamePlayer(Player player)
        {
            var result = 0;
            using (var connection = new DataConnect())
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
        public void ConnectToMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public void CreatetMatch(Match newMatch)
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public void StartLobby(List<Player> players, Match newMatch)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ManagerService : IGameService
    {
        public int addPoints(Player player, int score)
        {
            throw new NotImplementedException();
        }

        public List<Answer> GetAnswers(Question question)
        {
            throw new NotImplementedException();
        }

        public List<Question> GetQuestions()
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
        Dictionary<Player, IChatServiceCallback> playersCallback = new Dictionary<Player, IChatServiceCallback>();
        Dictionary<int, List<Message>> messages = new Dictionary<int, List<Message>>();
        List<Player> playersInchat = new List<Player>();

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
            foreach (Player c in playersCallback.Keys)
            {
                if (c.userName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void Connect(Player player, int idMatch)
        {

            if (!playersCallback.ContainsValue(CurrentCallback) && !SearchClientsByName(player.userName))
            {   
                lock (syncObj)
                { 
                    playersCallback.Add(player, CurrentCallback);
                    playersInchat.Add(player);
                    SetMessages(idMatch);

                    foreach (Player key in playersCallback.Keys)
                    {
                        //IChatServiceCallback callback = playersCallback[key];

                        OperationContext.Current.GetCallbackChannel<IChatServiceCallback>().RefreshClients(playersInchat);
                        OperationContext.Current.GetCallbackChannel<IChatServiceCallback>().UserJoin(player);

                    }
                }

            }
           
        }

        public void Disconnect(Player player)
        {
            foreach (Player c in playersCallback.Keys)
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

        public void Say(int idMatch, Message msg)
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

        public List<Message> GetMessages(int idMatch)
        {

                if(!messages.ContainsKey(idMatch))
                {
                    List<Message> messageList = new List<Message>();
                    messages.Add(idMatch, messageList);
                }
 
            return messages[idMatch];
        }

        public void Whisper(Message msg, Player receiver)
        {
            foreach (Player rec in playersCallback.Keys)
            {
                if (rec.userName == receiver.userName)
                {
                    IChatServiceCallback callback = playersCallback[rec];
                    callback.ReceiveWhisper(msg, rec);

                    foreach (Player sender in playersCallback.Keys)
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
