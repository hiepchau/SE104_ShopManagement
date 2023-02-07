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
        public async Task AddProductTest1()
        {
            ProductsInformation product = new ProductsInformation("", "OMO", 0, 0, 0, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNotNull(await register.register());
        }
        [Test]
        public async Task AddProductTest2()
        {
            ProductsInformation product = new ProductsInformation("", "OMO", 0, 1500, 5000, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNotNull(await register.register());
        }
        [Test]
        public async Task AddProductTest3()
        {
            ProductsInformation product = new ProductsInformation("", "TEST", 0, 99999999, 99999999, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNotNull(await register.register());
        }
        [Test]
        public async Task AddProductTest4()
        {
            ProductsInformation product = new ProductsInformation("", "TEST", 0, 1500, 1500, "", "6358d2b508104d5c4f2f3cad", "Bich", true);
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNull(await register.register());
        }
        [Test]
        public async Task AddProductTest5()
        {
            ProductsInformation product = new ProductsInformation("", "TEST", 0, 1500, 1500, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "", true);
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNull(await register.register());
        }
        [Test]
        public async Task AddProductTest6()
        {
            ProductsInformation product = new ProductsInformation("", "OMO", 0, 5000, 5000, "6358d1ca8152b59882a832ff", "", "Bich", true);
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNull(await register.register());
        }
        [Test]
        public async Task AddProductTest7()
        {
            ProductsInformation product = new ProductsInformation("", "TEST", 0, 0, 99999999, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNotNull(await register.register());
        }
        [Test]
        public async Task AddProductTest8()
        {
            ProductsInformation product = new ProductsInformation("", "OMO", 0, 1500, 5000, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich", true, await new AutoProductsIDGenerator(_session, Connection.client).Generate());
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNotNull(await register.register());
        }
        [Test]
        public async Task AddProductTest9()
        {
            ProductsInformation product = new ProductsInformation("", "OMO", 0, 5000, 5000, "", "6358d2b508104d5c4f2f3cad", "Bich", true);
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNull(await register.register());
        }
        [Test]
        public async Task AddProductTest10()
        {
            ProductsInformation product = new ProductsInformation("", "TEST", 0, 1500, 5000, "6358d1ca8152b59882a832ff", "", "", true);
            RegisterProducts register = new RegisterProducts(product, Connection.client, _session);
            Assert.IsNull(await register.register());
        }
    }
}