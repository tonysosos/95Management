using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _95Management.Models;

namespace _95Management.Tests.Service
{
    [TestClass]
    public class MySqlHelperTest
    {
        [TestMethod]
        public void InsertNewUser()
        {
            int result = UserDAO.Instance.InsertNewUser("TestName1", "TestNickName1", "TestOpenid1", "TestPhone1", DateTime.Now);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void InsertNewMatch()
        {
            int result = MatchDAO.Instance.InsertNewMatch(DateTime.Now, "北海稻", "一盘散沙", JERSEYS.RED);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetMatch()
        {
            MatchModel res = MatchDAO.Instance.GetMatch(1);

            Assert.IsNotNull(res);
        }
    }
}
