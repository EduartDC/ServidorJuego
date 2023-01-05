using MessageService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestUnitApp
{
    [TestClass]
    public class ChatServiceTest
    {
        readonly string code = "123456";
        readonly string username = "Cris";

        [TestMethod]
        public void SearchPlayers()
        {
            ManagerService objectManagerService = new ManagerService();

            var obtainedResult = objectManagerService.SearchPlayers(username);

            Assert.IsTrue(obtainedResult);
        }
    }
}
