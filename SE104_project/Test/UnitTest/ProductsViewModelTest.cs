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
    public class ProductsViewModelTest
    {
        private Mock<ManagingFunctionsViewModel> mockProvider;
        private Mock<ManagementMenu> managementProvider;
        private ProductsFunction productsViewModel;
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
            mockProvider = new Mock<ManagingFunctionsViewModel>(_session, Connection);           
            managementProvider = new Mock<ManagementMenu>(mockProvider.Object, _session, Connection);
            productsViewModel = new ProductsFunction(_session, Connection, mockProvider.Object, managementProvider.Object);
            productsViewModel.listActiveItemsProduct = new Mock<ObservableCollection<ProductsControlViewModel>>().Object;
            productsViewModel.listAllProduct = new Mock<ObservableCollection<ProductsControlViewModel>>().Object;
            await productsViewModel.GetProducerData();
            await productsViewModel.GetProductTypeData();
        }
        [Test]
        public async Task SetUnactiveTest()
        {
            ProductsControlViewModel testProduct = new ProductsControlViewModel(new ProductsInformation("6358d45e521a20a54e3e9773", "OMO", 0, 1500, 5000, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich"), productsViewModel);
            Assert.IsTrue(await productsViewModel.SetUnactive(testProduct));         
        }
        [Test]
        public async Task SetActiveTest()
        {
            ProductsControlViewModel testProduct = new ProductsControlViewModel(new ProductsInformation("6358d45e521a20a54e3e9773", "OMO", 0, 1500, 5000, "6358d1ca8152b59882a832ff", "6358d2b508104d5c4f2f3cad", "Bich"), productsViewModel);
            Assert.IsTrue(await productsViewModel.SetActive(testProduct));
        }
        [Test]
        public void AddProductTest1()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 5000;
            productsViewModel.productCost = 2000;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsTrue(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest2()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 2000;
            productsViewModel.productCost = 5000;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsFalse(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest3()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 2000;
            productsViewModel.productCost = 5000;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsFalse(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest4()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 2000;
            productsViewModel.productCost = 5000;
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsFalse(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest5()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 99999999;
            productsViewModel.productCost = 5000;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsTrue(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest6()
        {
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 99999999;
            productsViewModel.productCost = 5000;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            Assert.IsFalse(productsViewModel.SaveCommand.CanExecute(null));
        }
        [Test]
        public void AddProductTest7()
        {
            var uri = "..//..//Test//TestImage//omo.jpg";
            productsViewModel.productName = "OMO";
            productsViewModel.productPrice = 0;
            productsViewModel.productCost = -1;
            productsViewModel.productUnit = "Bich";
            productsViewModel.SelectedProducer = productsViewModel.ItemSourceProducer[0];
            productsViewModel.SelectedProductsType = productsViewModel.ItemSourceProductsType[0];
            productsViewModel.productImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Relative));
            Assert.IsFalse(productsViewModel.SaveCommand.CanExecute(null));
        }
    }
}
