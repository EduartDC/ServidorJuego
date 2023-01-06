using MessageService;
using MessageService.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitApp
{
    internal class UserManagerTest
    {
        readonly string code = "123456";
        readonly string username = "Cris";


        [TestMethod]
        public void MatchingFriendsSuccess()
        {
            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.MatchingFriends(username);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void MatchingFriendsFailed()
        {
            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.MatchingFriends("Pedrito");

            Assert.IsNull(obtainedResult);
        }

        [TestMethod]
        public void AddFriendSuccess()
        {
            FriendServer friend = new FriendServer();
            friend.gameFriend = 1;
            friend.ownerPlayer = 2;
            friend.creationDate = System.DateTime.Now;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.AddFriend(friend);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void AddFriendFailed()
        {
            FriendServer friend = new FriendServer();
            friend.gameFriend = 80;
            friend.ownerPlayer = 80;
            friend.creationDate = System.DateTime.Now;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.AddFriend(friend);

            Assert.Equals(0, obtainedResult);
        }

        [TestMethod]
        public void AddPlayerSuccess()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.idPlayer = 1;
            playerServer.firstName = "Cristopher";
            playerServer.lastName = "Rodríguez Salamanca";
            playerServer.email = "cris@gmail.com";
            playerServer.userName = username;
            playerServer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            playerServer.status = true;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.AddPlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void AddPlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.idPlayer = 1;
            playerServer.firstName = "Pedrito";
            playerServer.lastName = "Sola";
            playerServer.email = "peso@gmail.com";
            playerServer.userName = "PedritoGamer";
            playerServer.password = "8d4544d4sdcad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            playerServer.status = true;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.AddPlayer(playerServer);

            Assert.Equals(0, obtainedResult);
        }



        [TestMethod]
        public void SearchPlayerSuccess()
        {
            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.SearchPlayer(username);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void SearchPlayerFailed()
        {
            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.SearchPlayer("Pedrito");

            Assert.IsNull(obtainedResult);
        }

        [TestMethod]
        public void UpdatePlayerSuccess()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.firstName = "Cristopher";
            playerServer.lastName = "Rodríguez Salamanca";
            playerServer.userName = username;
            playerServer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.UpdatePlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void UpdatePlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.firstName = "Cristopher";
            playerServer.lastName = "Rodríguez Salamanca";
            playerServer.userName = "PedritoGamer";
            playerServer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.UpdatePlayer(playerServer);

            Assert.Equals(0, obtainedResult);
        }

        [TestMethod]
        public void ValidateEmailPlayerSuccess()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.email = "cris@gmail.com";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateEmailPlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void ValidateEmailPlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.email = "crisoforogamer@gmail.com";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateEmailPlayer(playerServer);

            Assert.Equals(0, obtainedResult);
        }

        [TestMethod]
        public void UserConnectSuccess()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = username;
            playerServer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.UserConnect(playerServer);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void UserConnectFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "Pedrito";
            playerServer.password = "8d969eef64a7q8c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.UserConnect(playerServer);

            Assert.IsNull(obtainedResult);
        }

        [TestMethod]
        public void ValidateUserNamePlayerSuccess()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = username;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateUserNamePlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void ValidateUserNamePlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "PedritoGamer";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateUserNamePlayer(playerServer);

            Assert.Equals(0, obtainedResult);
        }

    }
}
