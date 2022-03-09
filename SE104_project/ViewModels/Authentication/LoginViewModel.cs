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

namespace SE104_OnlineShopManagement.ViewModels.Authentication
{
    public class LoginViewModel:ViewModelBase
    {
        private IViewModelFactory _viewModelFactory;
        private INavigator _navigator;
        public ICommand RegisterCommand1 { get;private set; }   
        public ICommand LoginCommand { get;private set; }   
        public UpdateCurrentViewModelCommand<RegisterViewModel> RegisterCommand { get; set; }
       
        public LoginViewModel(IViewModelFactory factory, MainWindowNavigator<HomeWindow> navigator)
        {
            _viewModelFactory = factory;
            _navigator = navigator;
            //RegisterCommand = new UpdateCurrentViewModelCommand<RegisterViewModel>(factory.CreateViewModel<MainViewModel>(), factory);
            RegisterCommand1 = new RelayCommand<object>(null, OpenRegister);
            LoginCommand = new RelayCommand<object>(null, Login);
        }

        public void Login(object o = null)
        {
            _navigator.Navigate();
            Console.WriteLine("Login Command executed");
        }

        public void OpenRegister(object o=null)
        {
            Console.WriteLine("Openregister Command executed");
            _viewModelFactory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _viewModelFactory.CreateViewModel<RegisterViewModel>();
            //_authenticationWindow.DataContext = _viewModelFactory.CreateViewModel<MainViewModel>();
        }
    }

    
}
