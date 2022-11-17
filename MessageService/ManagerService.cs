

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
    ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
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
                var player = connection.Players.Find(newPlayer);

                player.firstName = newPlayer.firstName;
                player.lastName = newPlayer.lastName;
                player.userName = newPlayer.userName;
                player.password = newPlayer.password;

                try
                {
                    
                    return connection.SaveChanges();
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
        Dictionary<Player, IChatCallback> clients = new Dictionary<Player, IChatCallback>();

        List<Player> clientList = new List<Player>();

        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();

            }
        }

        object syncObj = new object();

        private bool SearchClientsByName(string name)
        {
            foreach (Player c in clients.Keys)
            {
                if (c.userName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Connect(Player client)
        {
            if (!clients.ContainsValue(CurrentCallback) && !SearchClientsByName(client.userName))
            {
                lock (syncObj)
                {
                    clients.Add(client, CurrentCallback);
                    clientList.Add(client);

                    foreach (Player key in clients.Keys)
                    {
                        IChatCallback callback = clients[key];
                        try
                        {
                            callback.RefreshClients(clientList);
                            callback.UserJoin(client);
                        }
                        catch
                        {
                            clients.Remove(key);
                            return false;
                        }

                    }

                }
                return true;
            }
            return false;
        }

        public void Disconnect(Player client)
        {
            foreach (Player c in clients.Keys)
            {
                if (client.userName == c.userName)
                {
                    lock (syncObj)
                    {
                        this.clients.Remove(c);
                        this.clientList.Remove(c);
                        foreach (IChatCallback callback in clients.Values)
                        {
                            callback.RefreshClients(this.clientList);
                            callback.UserLeave(client);
                        }
                    }
                    return;
                }
            }
        }

        public void Say(Message msg)
        {
            lock (syncObj)
            {
                foreach (IChatCallback callback in clients.Values)
                {
            
                    callback.Receive(msg);
                }
            }
        }

        public void Whisper(Message msg, Player receiver)
        {
            foreach (Player rec in clients.Keys)
            {
                if (rec.userName == receiver.userName)
                {
                    IChatCallback callback = clients[rec];
                    callback.ReceiveWhisper(msg, rec);

                    foreach (Player sender in clients.Keys)
                    {
                        if (sender.userName == msg.Sender)
                        {
                            IChatCallback senderCallback = clients[sender];
                            senderCallback.ReceiveWhisper(msg, rec);
                            return;
                        }
                    }
                }
            }
        }
    }



}
