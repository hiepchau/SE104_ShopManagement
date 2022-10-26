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
    public class ProductTest
    {
        AuthenticationInformation authInfo;
        Authenticator authenticator;
        private MongoConnect Connection;
        private AppSession _session;
        private RegisterProducts register;
        private ProductsInformation pro;
        private GetProducts getter;
        [SetUp]
        public async Task Setup()
        {
            Connection = new MongoConnect();
            _session = new AppSession();
            authInfo = new AuthenticationInformation("1", "1", "123");
            authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            _session.CurrnetUser = userInfo;
            pro = new ProductsInformation("", "OMO", 1, 15000, 10000, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            register = new RegisterProducts(pro, Connection.client, _session);
            var filter = Builders<ProductsInformation>.Filter.Empty;
            getter = new GetProducts(Connection.client, _session, filter);
        }
        [Test]
        public async Task AddProductTest()
        {
            Assert.AreSame("true", await register.register());
        }
        [Test]
        public async Task GetProductTest()
        {
            bool flag = false;
            var list = await getter.Get();
            foreach(var product in list)
            {
                if(product.name.Equals(pro.name)) flag= true;
            }
            Assert.IsTrue(flag);
        }
    }
}
