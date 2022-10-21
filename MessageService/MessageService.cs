
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MessageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class MessageService : IUserManager
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
        public int ValidatePlayer(Player player)
        {
                return 1;
        }
    }
}
