using MessageService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestUnitApp
{
    [TestClass]
    public class ChatServiceTest
    {
        private ManagerService objectManagerService { get; set; }
        String username = "Cris";

        [TestInitialize]
        public void TestInitialize()
        {
            objectManagerService = new ManagerService();
        }            

        [TestMethod]
        public void TestSearchPlayersInChatWithoutUsers()
        {                        
            Assert.IsFalse(objectManagerService.SearchPlayersInChat(username));
        }

        [TestMethod]
        public void TestSearchPlayersWithBlankSpace()
        {            
            Assert.IsFalse(objectManagerService.SearchPlayersInChat(" "));
        }

        [TestMethod]
        public void TestSearchPlayersWithNullInput()
        {
            Assert.IsFalse(objectManagerService.SearchPlayersInChat(null));
        }

    }
}
