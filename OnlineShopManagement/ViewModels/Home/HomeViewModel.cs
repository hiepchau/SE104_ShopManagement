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
using System.Windows.Controls;
using SE104_OnlineShopManagement.Services;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SE104_OnlineShopManagement.Models;
using System.Linq;
using SE104_OnlineShopManagement.Views.Windows;
using System.Windows;

namespace SE104_OnlineShopManagement.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        #region Properties
        private BaseFunction _currentState;
        private ManagingFunctionsViewModel _managingFunctionsViewModel;
        private SellingViewModel _sellingViewModel;
        public TitleBarViewModel _titleBarViewModel { get; set; }
        public BitmapImage testimg { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public BaseFunction CurrentState { get => _currentState;set => _currentState = value; }
        public BitmapImage ImageSrc { get; set; }
        private INavigator _navigator;
        #endregion

        #region Commands
        private IViewModelFactory _factory;
        public ICommand testCommand { get; set; }
        public ICommand SelectFunctionListCommand { get; set; }
        #endregion
        public HomeViewModel(IViewModelFactory factory, AppSession session,MongoConnect connect, MainWindowNavigator<AuthenticationWindow> navigator)
        {
            this._factory= factory;
            this._connection = connect;
            this._session= session;
            testCommand = new RelayCommand<object>(null, testing);
            _managingFunctionsViewModel = new ManagingFunctionsViewModel(session,connect);
            _sellingViewModel = new SellingViewModel(session, connect);
            _titleBarViewModel = new TitleBarViewModel();
            SelectFunctionListCommand = new RelayCommand<object>(null, selectFuncList);
            _navigator = navigator;
        }

        private async void testing(object o = null)
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
            ProductsInformation product = new ProductsInformation(await new AutoProductsIDGenerator(_session,_connection.client).Generate(),name, quantity, price, cost, category, producer,"Cai");
            RegisterProducts regist = new RegisterProducts(product, _connection.client, _session);
            string s = await regist.register();
            Console.WriteLine(s);
        }

        private void selectFuncList(object o)
        {
            
            if ((o as string) == "POS")
            {
                CurrentState = _sellingViewModel;
                OnPropertyChanged(nameof(CurrentState));
            }
            if ((o as string) == "Management")
            {
                CurrentState = _managingFunctionsViewModel;
                (CurrentState as ManagingFunctionsViewModel).changeMenu("Management");
                OnPropertyChanged(nameof(CurrentState));
            }
            if ((o as string) == "Customer")
            {
                CurrentState = _managingFunctionsViewModel;
                (CurrentState as ManagingFunctionsViewModel).changeMenu("Customer");
                OnPropertyChanged(nameof(CurrentState));
            }
            if ((o as string) == "Finance")
            {
                CurrentState = _managingFunctionsViewModel;
                (CurrentState as ManagingFunctionsViewModel).changeMenu("Finance");
                OnPropertyChanged(nameof(CurrentState));
            }
            if ((o as string) == "Report")
            {
                CurrentState = _managingFunctionsViewModel;
                (CurrentState as ManagingFunctionsViewModel).changeMenu("Report");
                OnPropertyChanged(nameof(CurrentState));
            }
            if ((o as string) == "Settings")
            {
                CurrentState = _managingFunctionsViewModel;
                (CurrentState as ManagingFunctionsViewModel).changeMenu("Settings");
                OnPropertyChanged(nameof(CurrentState));
            }

            if ((o as string) == "Logout")
            {
                _session = null;
                _factory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _factory.CreateViewModel<LoginViewModel>();
                _navigator.Navigate();
            }

        }
    }
}
