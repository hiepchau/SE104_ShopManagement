using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;

namespace SE104_OnlineShopManagement.Test.UnitTest
{
    public class ProductTypeViewModelTest
    {
        private ProductsTypeFunction productTypeViewModel;
        private MongoConnect Connection;
        private AppSession _session;
        [SetUp]
        public async Task Setup()
        {
            Connection = new MongoConnect();
            _session = new AppSession();
            AuthenticationInformation authInfo = new AuthenticationInformation("1", "1", "123");
            Authenticator authenticator = new Authenticator(authInfo, Connection.client);
            UserInfomation userInfo = await authenticator.Authenticate();
            _session.CurrnetUser = userInfo;
            productTypeViewModel = new ProductsTypeFunction(_session, Connection);
            productTypeViewModel.listItemsProductType = new Mock<ObservableCollection<ProductsTypeControlViewModel>>().Object;
            productTypeViewModel.listItemsUnactiveProductType = new Mock<ObservableCollection<ProductsTypeControlViewModel>>().Object;
        }
        [Test]
        public void AddProductTypeTest1()
        {
            productTypeViewModel.productTypeName = "Bot giat";
            Assert.IsTrue(productTypeViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTypeTest2()
        {
            productTypeViewModel.productTypeName = "Sach";
            Assert.IsTrue(productTypeViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTypeTest3()
        {
            productTypeViewModel.productTypeName = "";
            Assert.IsFalse(productTypeViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTypeTest4()
        {
            productTypeViewModel.productTypeName = ".";
            Assert.IsFalse(productTypeViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTypeTest5()
        {
            productTypeViewModel.productTypeName = "1";
            Assert.IsFalse(productTypeViewModel.SaveCommand.CanExecute(null));
        }
    }
}
