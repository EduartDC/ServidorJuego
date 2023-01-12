using MessageService;
using MessageService.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using DataAcces;
using System.Linq;

namespace TestUnitApp
{
    [TestClass]
    public class UserManagerTest
    {
        private PlayerServer objectPlayer { get; set; }
        private PlayerServer objectPlayerFriend { get; set; }
        private FriendServer objectFriend { get; set; }
        private ManagerService objectManagerService { get; set; }
        private List<String> friends { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            objectManagerService = new ManagerService();
            friends = new List<String>();
            objectPlayer = new PlayerServer();
            objectPlayerFriend = new PlayerServer();
            objectFriend = new FriendServer();

            objectPlayer.idPlayer = 1;
            objectPlayer.firstName = "Cristopher";
            objectPlayer.lastName = "Rodriguez Salamanca";
            objectPlayer.email = "cris@gmail.com";
            objectPlayer.userName = "Cris";
            objectPlayer.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayer.status = true;

            objectPlayerFriend.idPlayer = 4;
            objectPlayerFriend.firstName = "Eduardo";
            objectPlayerFriend.lastName = "Lopez Chacon";
            objectPlayerFriend.email = "edu@gmail.com";
            objectPlayerFriend.userName = "Edu";
            objectPlayerFriend.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayerFriend.status = true;

            objectFriend.idFriend = 1;
            objectFriend.gameFriend = 4;
            objectFriend.creationDate = DateTime.Now;
            objectFriend.ownerPlayer = 1;
        }


        [TestMethod]
        public void TestMatchingFriendsSuccess()
        {
            objectManagerService.UserConnect(objectPlayerFriend);
            Assert.AreEqual(1, objectManagerService.MatchingFriends(objectPlayer.userName).Count());
        }

        [TestMethod]
        public void TestMatchingFriendsUserWithoutFriends()
        {
            Assert.AreEqual(0, objectManagerService.MatchingFriends("Rosa").Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestMatchingFriendsFailedWithSpecialCharacters()
        {
            Assert.AreEqual(0, objectManagerService.MatchingFriends("#$&").Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestMatchingFriendsFailedWithBlankSpace()
        {
            Assert.AreEqual(0, objectManagerService.MatchingFriends(" ").Count());
        }

        [TestMethod]
        public void TestAddFriendSuccess()
        {
            Assert.AreEqual(1, objectManagerService.AddFriend(objectFriend));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddFriendFailedWithNullFriend()
        {            
            Assert.AreEqual(0, objectManagerService.AddFriend(null));
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
            objectPlayerNew.firstName = "Jorge Octavio";
            objectPlayerNew.lastName = "Ocharan Hernández";
            objectPlayerNew.email = "johe@gmail.com";
            objectPlayerNew.userName = "Architect";
            objectPlayerNew.password = "cda8b4c76cbf032571dd2b6f9545dd5d100153f92d11d9720a30ed9d24b55fdf";
            objectPlayerNew.status = false;

            Assert.AreEqual(1, objectManagerService.AddPlayer(objectPlayerNew));
        }

        [TestMethod]
        public void TestAddPlayerFailedWithExistingUser()
        {
            Assert.AreEqual(0, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestAddPlayerWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddPlayerFailedWithNullPlayer()
        {
            Assert.AreEqual(0, objectManagerService.AddPlayer(null));
        }

        [TestMethod]
        public void TestDeleteFriendSuccess()
        {
            objectManagerService.UserConnect(objectPlayerFriend);
            Assert.AreEqual(1, objectManagerService.DeleteFriend(objectPlayer.idPlayer, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendFriendOffline()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectPlayer.idPlayer, objectPlayerFriend.userName));            
        }

        [TestMethod]
        public void TestDeleteFriendNonExistentFriend()
        {            
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "Bad_Bunny"));
        }

        [TestMethod]
        public void TestDeleteFriendWithoutConnection()
        {
            Assert.AreEqual(404, objectManagerService.DeleteFriend(objectFriend.ownerPlayer, objectPlayer.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithWrongArgument()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(666, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithWrongUsername()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "Popó"));
        }

        [TestMethod]
        public void TestDeleteFriendWithInvalidCharacters()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "#+-!"));
        }

        [TestMethod]
        public void TestDeleteFriendWithInvalidCharacterOnID()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(-1, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithBlankSpace()
        {
            Assert.AreEqual(0, objectManagerService.DeleteFriend(objectPlayer.idPlayer, " "));
        }

        [TestMethod]
        public void TestSearchPlayerSuccess()
        {
            objectManagerService.SearchPlayer(objectPlayer.userName);
            Assert.AreEqual(objectPlayer.userName, objectManagerService.SearchPlayer(objectPlayer.userName).userName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSearchPlayerFailedWithUnregisteredPlayer()
        {
            Assert.AreNotEqual("Gaviota", objectManagerService.SearchPlayer("Gaviota").userName);
        }

        [TestMethod]
        public void TestSearchPlayerWithoutConnection()
        {
            Assert.IsNull(objectManagerService.SearchPlayer("Memo_8a"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSearchPlayerFailedWithBlankSpace()
        {
            Assert.AreNotEqual(objectPlayer.userName, objectManagerService.SearchPlayer(" ").userName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSearchPlayerFailedWithInvalidCharacters()
        {
            Assert.AreNotEqual(objectPlayer.userName, objectManagerService.SearchPlayer("#!-¿").userName);
        }

        [TestMethod]
        public void TestUpdatePlayerSuccess()
        {
            objectPlayer.email = "crisoforo@gmail.com";
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
