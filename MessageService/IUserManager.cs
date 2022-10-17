using DataAcces;
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
        int AddUser(User user);

        [OperationContract]
        User GetUserById(String userId);

        [OperationContract]
        int AddManager(Manager manager);

        [OperationContract]
        int AddPlayer(Player newPlayer);

        [OperationContract]
        int ValidatePlayer(Player player);


    }

    [DataContract]
    public class User
    {
        private String userName;
        private String lastName;

        [DataMember]
        public String UserName { get { return userName; } set { userName = value; } }

        [DataMember]
        public String LastName { get { return lastName; } set { lastName = value; } }

    }
    
    [DataContract]
    public class Manager
    {
        private String userName;
        private String lastName;

        [DataMember]
        public String UserName { get { return userName; } set { userName = value; } }

        [DataMember]
        public String LastName { get { return lastName; } set { lastName = value; } }
    }
}
