using MessageService;
using MessageService.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using DataAcces;
using System.Linq;
using System.Reflection;

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
            int expectedResult = 1;
            objectManagerService.UserConnect(objectPlayerFriend);
            Assert.AreEqual(expectedResult, objectManagerService.MatchingFriends(objectPlayer.userName).Count());
        }

        [TestMethod]
        public void TestMatchingFriendsUserWithoutFriends()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.MatchingFriends("Rosa").Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestMatchingFriendsFailedWithSpecialCharacters()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.MatchingFriends("#$&").Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestMatchingFriendsFailedWithBlankSpace()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.MatchingFriends(" ").Count());
        }

        [TestMethod]
        public void TestAddFriendSuccess()
        {
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, objectManagerService.AddFriend(objectFriend));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddFriendFailedWithNullFriend()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.AddFriend(null));
        }

        [TestMethod]
        public void TestAddFriendWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.AddFriend(objectFriend));
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

            int expectedResult = 1;

            Assert.AreEqual(expectedResult, objectManagerService.AddPlayer(objectPlayerNew));
        }

        [TestMethod]
        public void TestAddPlayerFailedWithExistingUser()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestAddPlayerWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.AddPlayer(objectPlayer));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddPlayerFailedWithNullPlayer()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.AddPlayer(null));
        }

        [TestMethod]
        public void TestDeleteFriendSuccess()
        {
            objectManagerService.UserConnect(objectPlayerFriend);

            int expectedResult = 1;

            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendFriendOffline()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, objectPlayerFriend.userName));            
        }

        [TestMethod]
        public void TestDeleteFriendNonExistentFriend()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "Bad_Bunny"));
        }

        [TestMethod]
        public void TestDeleteFriendWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectFriend.ownerPlayer, objectPlayer.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithWrongArgument()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(666, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithWrongUsername()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "Popó"));
        }

        [TestMethod]
        public void TestDeleteFriendWithInvalidCharacters()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, "#+-!"));
        }

        [TestMethod]
        public void TestDeleteFriendWithInvalidCharacterOnID()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(-1, objectPlayerFriend.userName));
        }

        [TestMethod]
        public void TestDeleteFriendWithBlankSpace()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.DeleteFriend(objectPlayer.idPlayer, " "));
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
            PlayerServer objectPlayerNew = new PlayerServer();
            objectPlayerNew.idPlayer = 1;
            objectPlayerNew.firstName = "Cristopher";
            objectPlayerNew.lastName = "Hijo de Messi";
            objectPlayerNew.email = "cris@gmail.com";
            objectPlayerNew.userName = "Cris";
            objectPlayerNew.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayerNew.status = true;

            int expectedResult = 1;

            Assert.AreEqual(expectedResult, objectManagerService.UpdatePlayer(objectPlayerNew));
        }        

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUpdatePlayerUnregisteredFailed()
        {
            PlayerServer objectPlayerNew = new PlayerServer();
            objectPlayerNew.idPlayer = 666;
            objectPlayerNew.firstName = "Jaime";
            objectPlayerNew.lastName = "Del Rincón Dorado";
            objectPlayerNew.email = "jdrd@gmail.com";
            objectPlayerNew.userName = "JaimitoBebe";
            objectPlayerNew.password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            objectPlayerNew.status = false;

            int expectedResult = 0;

            Assert.Equals(expectedResult, objectManagerService.UpdatePlayer(objectPlayerNew));
        }

        [TestMethod]
        public void TestUpdatePlayerWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.UpdatePlayer(objectPlayer));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUpdatePlayerWithNullObjectFailed()
        {
            int expectedResult = 0;
            Assert.Equals(expectedResult, objectManagerService.UpdatePlayer(null));
        }

        [TestMethod]
        public void TestValidateEmailPlayerSuccess()
        {
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateEmailPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestValidateEmailPlayerUnregisteredEmail()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.email = "crisoforogamer@gmail.com";

            int expectedResult = 0;

            Assert.AreEqual(expectedResult, objectManagerService.ValidateEmailPlayer(playerServer));
        }

        [TestMethod]
        [ExpectedException(typeof(TargetException))]
        public void TestValidateEmailPlayerFailedWithNullObject()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateEmailPlayer(null));
        }

        [TestMethod]
        public void TestValidateEmailPlayerWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateEmailPlayer(objectPlayer));
        }

        [TestMethod]
        public void TestUserConnectSuccess()
        {
            Assert.IsNotNull(objectManagerService.UserConnect(objectPlayer));
        }

        [TestMethod]
        public void TestUserConnectWithUnregisteredUser()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "Pedrito";
            playerServer.password = "8d969eef64a7q8c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";

            Assert.IsNull(objectManagerService.UserConnect(playerServer).userName);
        }

        [TestMethod]
        public void TestUserConnectWithoutConnection()
        {
            Assert.IsNull(objectManagerService.UserConnect(objectPlayer));
        }

        [TestMethod]
        [ExpectedException(typeof(TargetException))]
        public void TestUserConnectWithNullObject()
        {
            Assert.IsNull(objectManagerService.UserConnect(null));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerSuccess()
        {
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateUserNamePlayer(objectPlayer));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerUnregisteredUsername()
        {
            PlayerServer playerServer = new PlayerServer();
            playerServer.userName = "PedritoGamer";

            int expectedResult = 0;

            Assert.AreEqual(expectedResult, objectManagerService.ValidateUserNamePlayer(playerServer));
        }

        [TestMethod]
        [ExpectedException(typeof(TargetException))]
        public void TestValidateUserNamePlayerWithNullObjectFailed()
        {
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateUserNamePlayer(null));
        }

        [TestMethod]
        public void TestValidateUserNamePlayerWithoutConnection()
        {
            int expectedResult = 404;
            Assert.AreEqual(expectedResult, objectManagerService.ValidateUserNamePlayer(objectPlayer));
        }

    }
}
