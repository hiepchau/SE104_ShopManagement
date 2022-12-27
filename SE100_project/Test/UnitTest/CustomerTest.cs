using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit.Framework;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Services;

namespace SE104_OnlineShopManagement.Test.UnitTest
{
    public class CustomerTest
    {
        AuthenticationInformation authInfo;
        Authenticator authenticator;
        private MongoConnect Connection;
        private AppSession _session;
        [SetUp]
        public async Task Setup()
        {
            Connection = new MongoConnect();
            _session = new AppSession();
            authInfo = new AuthenticationInformation("1", "1", "123");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            _session.CurrnetUser = userInfo;
        }
        [Test]
        public async Task AddCustomerTest1()
        {
            CustomerInformation cus = new CustomerInformation("", "Khach", "0909090909", "62a86fb49d0d3a76ab2fa151", "078078078078", "Q7", true, await new AutoCustomerIDGenerator(_session, Connection.client).Generate());
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("true", await register.register());
        }
        [Test]
        public async Task AddCustomerTest2()
        {
            CustomerInformation cus = new CustomerInformation("", "Khach", "0909090909", "62a86fc39d0d3a76ab2fa152", "078078078078", "Q7", true, await new AutoCustomerIDGenerator(_session, Connection.client).Generate());
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("true", await register.register());
        }
        [Test]
        public async Task AddCustomerTest3()
        {
            CustomerInformation cus = new CustomerInformation("", "TEST", "", "62a86fb49d0d3a76ab2fa151", "079079079079", "Q8", true);
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("false", await register.register());
        }
        [Test]
        public async Task AddCustomerTest4()
        {
            CustomerInformation cus = new CustomerInformation("", "TEST", "0808080808", "62a86fc39d0d3a76ab2fa152", "", "Q8", true);
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("false", await register.register());
        }
        [Test]
        public async Task AddCustomerTest5()
        {
            CustomerInformation cus = new CustomerInformation("", "TEST", "0808080808", "62a86fb49d0d3a76ab2fa151", "079079079079", "Q8", true, await new AutoCustomerIDGenerator(_session, Connection.client).Generate());
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("true", await register.register());
        }
        [Test]
        public async Task AddCustomerTest6()
        {
            CustomerInformation cus = new CustomerInformation("", "Khach", "", "", "", "Q7", true);
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("false", await register.register());
        }
        [Test]
        public async Task AddCustomerTest7()
        {
            CustomerInformation cus = new CustomerInformation("", "Khach", "0909090909", "", "078078078078078", "Q7", true);
            RegisterCustomer register = new RegisterCustomer(cus, Connection.client, _session);
            Assert.AreSame("false", await register.register());
        }
    }
}
