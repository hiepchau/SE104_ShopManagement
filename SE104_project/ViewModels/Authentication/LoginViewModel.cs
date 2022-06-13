using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.Views.Windows;
using SE104_OnlineShopManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network;
using System.Windows.Controls;
using SE104_OnlineShopManagement.ViewModels.Home;

namespace SE104_OnlineShopManagement.ViewModels.Authentication
{
    public class LoginViewModel:ViewModelBase
    {
        #region Properties
        private IViewModelFactory _viewModelFactory;
        private INavigator _navigator;
        public string companyname { get; set; }
        public string username { get; set; }    
        public string password { get; set; }

        private MongoConnect Connection;
        private AppSession Session;

        #endregion

        #region Commands
        public ICommand LoginCommand { get;private set; }
        public ICommand RegisterCommand1 { get; private set; }
        public UpdateCurrentViewModelCommand<RegisterViewModel> RegisterCommand { get; set; }
        #endregion

        public LoginViewModel(IViewModelFactory factory, MainWindowNavigator<HomeWindow> navigator, MongoConnect connection, AppSession session)
        {
            _viewModelFactory = factory;
            _navigator = navigator;
            this.Connection = connection;
            this.Session = session;
            //RegisterCommand = new UpdateCurrentViewModelCommand<RegisterViewModel>(factory.CreateViewModel<MainViewModel>(), factory);
            RegisterCommand1 = new RelayCommand<object>(null, OpenRegister);
            LoginCommand = new RelayCommand<object>(null, Login);
        }

        public async void Login(object o)
        {
            var pass = o as PasswordBox;
            password= pass.Password;
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(companyname))
            {
                Console.WriteLine("Required information left blank");
                CustomMessageBox.Show("Đăng nhập thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AuthenticationInformation authen = new AuthenticationInformation(username, password, companyname);
            Authenticator LoginNet = new Authenticator(authen, Connection.client);
            UserInfomation logininfo = await LoginNet.Authenticate();
            if (logininfo != null)
            {
                Session.CurrnetUser = logininfo;
                HomeViewModel home =_viewModelFactory.CreateViewModel<HomeViewModel>();
                _viewModelFactory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _viewModelFactory.CreateViewModel<HomeViewModel>();
                _navigator.Navigate();
            }
            else
            {
                Console.WriteLine("Login failed.");
                CustomMessageBox.Show("Đăng nhập thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void OpenRegister(object o=null)
        {
            Console.WriteLine("Openregister Command executed");
            _viewModelFactory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _viewModelFactory.CreateViewModel<RegisterViewModel>();
            //_authenticationWindow.DataContext = _viewModelFactory.CreateViewModel<MainViewModel>();
        }
    }

    
}
