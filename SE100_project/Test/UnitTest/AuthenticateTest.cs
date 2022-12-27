using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.Authentication;

namespace SE104_OnlineShopManagement.Test.UnitTest
{
    [TestFixture]
    public class AuthenticateTest
    {
        string companyName;
        string password;
        string userName;
        AuthenticationInformation authInfo;
        Authenticator authenticator;
        Authenticator authenticatorFail;
        private MongoConnect Connection;
        [SetUp]
        public void Setup()
        {
            Connection = new MongoConnect();
        }
        [Test]
        public async Task TestLogin1()
        {
            authInfo = new AuthenticationInformation("1", "1", "123");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsTrue(isLogin);
        }
        [Test]
        public async Task TestLogin2()
        {
            authInfo = new AuthenticationInformation("1", "2", "123");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
        [Test]
        public async Task TestLogin3()
        {
            authInfo = new AuthenticationInformation("admin", "1", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsTrue(isLogin);
        }
        [Test]
        public async Task TestLogin4()
        {
            authInfo = new AuthenticationInformation("admin", "2", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
        [Test]
        public async Task TestLogin5()
        {
            authInfo = new AuthenticationInformation("nkhanh", "1", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsTrue(isLogin);
        }
        [Test]
        public async Task TestLogin6()
        {
            authInfo = new AuthenticationInformation("nkhanh", "2", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
        [Test]
        public async Task TestLogin7()
        {
            authInfo = new AuthenticationInformation("hipchou", "TEST", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsTrue(isLogin);
        }
        [Test]
        public async Task TestLogin8()
        {
            authInfo = new AuthenticationInformation("hipchou", "2", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
        [Test]
        public async Task TestLogin9()
        {
            authInfo = new AuthenticationInformation("hipchou", "1", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
        public async Task TestLogin10()
        {
            authInfo = new AuthenticationInformation("nkhanh", "TEST", "TEST");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            bool isLogin;
            if (userInfo != null) { isLogin = true; }
            else isLogin = false;
            Assert.IsFalse(isLogin);
        }
    }
}
