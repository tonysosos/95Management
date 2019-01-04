using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _95Management.Service;

namespace _95Management.Tests.Service
{
    [TestClass]
    public class MySqlHelperTest
    {
        [TestMethod]
        public void InsertNewUser()
        {
            int result = MySQLHelper.Instance.InsertNewUser("TestName1", "TestNickName1", "TestOpenid1", "TestPhone1", DateTime.Now);

            Assert.AreEqual(1, result);
        }
    }
}
