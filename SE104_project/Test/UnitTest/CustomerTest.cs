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
        private RegisterCustomer register;
        private CustomerInformation cus;
        private GetCustomer getter;
        [SetUp]
        public async Task Setup()
        {
            Connection = new MongoConnect();
            _session = new AppSession();
            authInfo = new AuthenticationInformation("1", "1", "123");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            _session.CurrnetUser = userInfo;
            cus = new CustomerInformation("", "Khach", "0123456789", "62a86fc39d0d3a76ab2fa152", "079202202202", "Q7", true, await new AutoCustomerIDGenerator(_session, Connection.client).Generate());
            register = new RegisterCustomer(cus, Connection.client, _session);
            var filter = Builders<CustomerInformation>.Filter.Empty;
            getter = new GetCustomer(Connection.client, _session, filter);
        }
        [Test]
        public async Task AddCustomerTest()
        {
            Assert.AreSame("true", await register.register());
        }
        [Test]
        public async Task GetCustomerTest()
        {
            bool flag = false;
            var list = await getter.Get();
            foreach (var customer in list)
            {
                if (customer.Name.Equals(cus.Name)) flag = true;
            }
            Assert.IsTrue(flag);
        }
    }
}
