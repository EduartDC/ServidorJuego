using MessageService;
using MessageService.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using DataAcces;

namespace TestUnitApp
{
    [TestClass]
    public class UserManagerTest
    {
        private PlayerServer objectPlayer { get; set; }
        private FriendServer objectFriend { get; set; }
        private ManagerService objectManagerService { get; set; }
        private List<String> friends { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            objectManagerService = new ManagerService();
            friends = new List<String>();
            objectPlayer = new PlayerServer();
            objectFriend = new FriendServer();

            objectPlayer.idPlayer = 1;
            objectPlayer.firstName = "Cristopher";
            objectPlayer.lastName = "Rodriguez Salamanca";
            objectPlayer.email = "cris@gmail.com";
            objectPlayer.userName = "Cris";
            objectPlayer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayer.status = true;

            objectFriend.idFriend = 1;
            objectFriend.gameFriend = 4;
            objectFriend.creationDate = DateTime.Now;
            objectFriend.ownerPlayer = 1;
        }


        [TestMethod]
        public void TestMatchingFriendsSuccess()
        {
            Assert.IsNotNull(objectManagerService.MatchingFriends(objectPlayer.userName));
        }

        [TestMethod]
        public void TestMatchingFriendsUserWithoutFriends()
        {
            Assert.IsNull(objectManagerService.MatchingFriends("Rosa"));
        }

        [TestMethod]
        public void TestMatchingFriendsWithSpecialCharacters()
        {
            Assert.IsNull(objectManagerService.MatchingFriends("4%#"));
        }

        [TestMethod]
        public void TestAddFriendSuccess()
        {
            Assert.AreEqual(1, objectManagerService.AddFriend(objectFriend));
        }

        [TestMethod]
        public void TestAddFriendFailed()
        {
            FriendServer objectFriendNew = new FriendServer();
            objectFriendNew.idFriend = 30;
            objectFriendNew.gameFriend = 40;
            objectFriendNew.creationDate = DateTime.Now;
            objectFriendNew.ownerPlayer = 12;
            Assert.AreEqual(0, objectManagerService.AddFriend(objectFriendNew));
        }

        [TestMethod]
        public void TestAddFriendWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.AddFriend(objectFriend));
        }

        [TestMethod]
        public void TestAddPlayerSuccess()
        {
            PlayerServer objectPlayerNew = new PlayerServer();
            objectPlayer.idPlayer = 9;
            objectPlayer.firstName = "Heisenberg";
            objectPlayer.lastName = "Medio Metro";
            objectPlayer.email = "hme@gmail.com";
            objectPlayer.userName = "Destrozador";
            objectPlayer.password = "8d969ee9etcad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayer.status = true;

            Assert.AreEqual(1, objectManagerService.AddPlayer(objectPlayerNew));
        }

        [TestMethod]
        public void TestAddPlayerFailed()
        {
            Assert.AreEqual(0, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestAddPlayerWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestDeleteFriendSuccess()
        {
            Assert.AreEqual(1, objectManagerService.DeleteFriend(objectFriend.ownerPlayer, objectPlayer.userName));
        }

        [TestMethod]
        public void TestDeleteFriendFailed()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectFriend.ownerPlayer, "Bad_Bunny"));
        }

        [TestMethod]
        public void TestDeleteFriendWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.DeleteFriend(objectFriend.ownerPlayer, objectPlayer.userName));
        }

        [TestMethod]
        public void TestSearchPlayerSuccess()
        {
            objectManagerService.SearchPlayer(objectPlayer.userName);
            Assert.AreEqual(objectPlayer.userName, objectManagerService.SearchPlayer(objectPlayer.userName).userName);
        }

        [TestMethod]
        public void TestSearchPlayerFailed()
        {
            Assert.AreNotEqual(objectPlayer, objectManagerService.SearchPlayer("Dircio"));
        }

        [TestMethod]
        public void TestSearchPlayerWithoutConnection()
        {
            Assert.IsNull(objectManagerService.SearchPlayer("Memo_8a"));
        }

        [TestMethod]
        public void TestUpdatePlayerSuccess()
        {
            objectPlayer.lastName = "Rodriguez Pereira";

            Assert.AreEqual(1, objectManagerService.UpdatePlayer(objectPlayer));
        }

        [TestMethod]
        public void TestUpdatePlayerUnregistered()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.firstName = "Chicharito";
            playerServer.lastName = "Perez Mendez";
            playerServer.userName = "ChichaGod";
            playerServer.password = "8d969eef8q7t8ec29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            Assert.Equals(0, objectManagerService.UpdatePlayer(playerServer));
        }

        [TestMethod]
        public void TestUpdatePlayerWithoutConnection()
        {
            Assert.Equals(404, objectManagerService.UpdatePlayer(objectPlayer));
        }

        [TestMethod]
        public void TestValidateEmailPlayerSuccess()
        {
            Assert.Equals(1, objectManagerService.ValidateEmailPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestValidateEmailPlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.email = "crisoforogamer@gmail.com";

            Assert.Equals(0, objectManagerService.ValidateEmailPlayer(playerServer));
        }

        [TestMethod]
        public void TestValidateEmailPlayerWithoutConnection()
        {
            Assert.Equals(404, objectManagerService.ValidateEmailPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestUserConnectSuccess()
        {
            Assert.AreEqual(objectPlayer, objectManagerService.UserConnect(objectPlayer));
        }

        [TestMethod]
        public void TestUserConnectFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "Pedrito";
            playerServer.password = "8d969eef64a7q8c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            Assert.AreNotEqual(objectPlayer, objectManagerService.UserConnect(playerServer));
        }

        [TestMethod]
        public void TestUserConnectWithoutConnection()
        {
            Assert.IsNull(objectManagerService.UserConnect(objectPlayer));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerSuccess()
        {
            Assert.AreEqual(1, objectManagerService.ValidateUserNamePlayer(objectPlayer));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerFailed()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "PedritoGamer";

            Assert.AreEqual(0, objectManagerService.ValidateUserNamePlayer(playerServer));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.ValidateUserNamePlayer(objectPlayer));
        }

    }
}
