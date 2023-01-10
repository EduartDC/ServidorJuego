using MessageService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestUnitApp
{
    [TestClass]
    public class ChatServiceTest
    {
        String username = "Cris";

        [TestMethod]
        public void TestSearchPlayersInChatWithoutUsers()
        {

            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.SearchPlayersInChat(username);

            Assert.IsTrue(obtainedResult);
        }

        [TestMethod]
        public void TestSearchPlayersInChatFailed()
        {
            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.SearchPlayersInChat("Pedrito");

            Assert.IsFalse(obtainedResult);
        }

    }
}
