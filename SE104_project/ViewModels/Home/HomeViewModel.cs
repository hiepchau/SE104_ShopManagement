using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Windows.Input;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.ViewModels.Authentication;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;

namespace SE104_OnlineShopManagement.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        #region Properties
        private BaseFunction _currentState;
        private ManagingFunctionsViewModel _managingFunctionsViewModel;
        private SellingViewModel _sellingViewModel;
        
        private MongoConnect _connection;
        private AppSession _session;
        public BaseFunction CurrentState { get => _currentState;set => _currentState = value; }
        #endregion

        #region Commands
        private IViewModelFactory _factory;
        public ICommand testCommand { get; set; }
        public ICommand testCommand1 { get; set; }
        #endregion
        public HomeViewModel(IViewModelFactory factory, AppSession session,MongoConnect connect)
        {
            this._factory= factory;
            this._connection = connect;
            this._session= session;
            testCommand = new RelayCommand<object>(null, testing);
            testCommand1 = new RelayCommand<object>(null, testing1);
            _managingFunctionsViewModel = new ManagingFunctionsViewModel(session,connect);
            _sellingViewModel = new SellingViewModel(session, connect);
        }

        private void testing1(object o = null)
        {
            SellingViewModel sell = _factory.CreateViewModel<SellingViewModel>();
            var filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, "diblo1");
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = getter.Get();
            foreach(ProductsInformation pro in ls)
            {
                Console.WriteLine(pro.quantity);
            }
        }
        private void testing(object o = null)
        {
            string name;
            string producer;
            long price;
            long cost;
            int quantity;
            string category;
            Console.Write("Name:");
            name = Console.ReadLine();
            Console.Write("Producer:");
            producer = Console.ReadLine();
            Console.Write("Price:");
            price = long.Parse(Console.ReadLine());
            Console.Write("Cost:");
            cost = long.Parse(Console.ReadLine());
            Console.Write("Quantity:");
            quantity = int.Parse(Console.ReadLine());
            Console.Write("Category:");
            category = Console.ReadLine();
            ProductsInformation product = new ProductsInformation("",name, quantity, price, cost, category, producer);
            RegisterProducts regist = new RegisterProducts(product, _connection.client, _session);
            regist.register();
        }
    }
}
