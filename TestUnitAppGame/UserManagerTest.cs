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
        public void MatchingFriends()
        {
            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.MatchingFriends(username);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void AddFriend()
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
        public void AddPlayer()
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
        public void DeleteFriend()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.idPlayer = 1;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.DeleteFriend(playerServer, username);

            Assert.AreEqual(1, obtainedResult);
        }

        [TestMethod]
        public void SearchPlayer()
        {
            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.SearchPlayer(username);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void UpdatePlayer()
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
        public void ValidateEmailPlayer()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.email = "cris@gmail.com";

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateEmailPlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

        [TestMethod]
        public void UserConnect()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = username;
            playerServer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            ManagerService objectManagerService = new ManagerService();
            var obtainedResult = objectManagerService.UserConnect(playerServer);

            Assert.IsNotNull(obtainedResult);
        }

        [TestMethod]
        public void ValidateUserNamePlayer()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = username;

            ManagerService objectManagerService = new ManagerService();
            int obtainedResult = objectManagerService.ValidateUserNamePlayer(playerServer);

            Assert.Equals(1, obtainedResult);
        }

    }
}
