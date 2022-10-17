using DataAcces;
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
        public int AddManager(Manager manager)
        {
            Console.WriteLine("Adding manager ..{0},{1}", manager.UserName, manager.LastName);
            return 1;
            //código que se conecta con la BD y guarda la información de manager
        }

        public int AddUser(User user)
        {
           Console.WriteLine("Adding user...{0},{1}",user.UserName,user.LastName);

            return 1;
        }

        public User GetUserById(string userId)
        {
            return new User
            {
                 LastName="Pérez Arriaga",
                 UserName="Juan Carlos"
            };
        }
        public int AddPlayer(Player newPlayer)
        {
            var result = 0;
            using(var connection = new ModelDB())
            {
               
                
                    connection.Players.Add(newPlayer);
                    result = connection.SaveChanges();
                
                
                
            }
            return result;
        }
        public int ValidatePlayer(Player player)
        {
            Console.Write("Validar");
            return 1;
        }
    }
}
