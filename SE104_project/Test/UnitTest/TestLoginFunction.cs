using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.Authentication;

namespace SE104_OnlineShopManagement.Test.UnitTest
{
    [TestFixture]
    public class TestLoginFunction
    {
        string companyName;
        string password;
        string userName;
        AuthenticationInformation authInfo;
        Authenticator authenticator;
        private MongoConnect Connection;
        [SetUp]
        public void Setup()
        {
            Connection = new MongoConnect();
            companyName = "123";
            password = "1";
            userName = "1";
            authInfo = new AuthenticationInformation(userName, password, companyName);
            authenticator = new Authenticator(authInfo, Connection.client);
        }
        [Test]
        public async Task TestLogin()
        {
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) isLogin = true;
            else isLogin = false;
            Assert.IsTrue(isLogin);
        }

    }
}
