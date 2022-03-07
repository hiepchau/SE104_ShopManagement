using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.Authentication
{
    public class RegisterViewModel: ViewModelBase
    {
        public ICommand BackCommand { get; private set; }
        private AuthenticationWindow authenticationWindow;
        private IViewModelFactory _factory;
        public RegisterViewModel(IViewModelFactory factory, AuthenticationWindow authen)
        {
            _factory =  factory;
            authenticationWindow = authen;
            BackCommand = new RelayCommand<object>(null, Back2Login);
        }
        public void Back2Login(object o=null)
        {
            _factory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _factory.CreateViewModel<LoginViewModel>();
            authenticationWindow.DataContext = _factory.CreateViewModel<MainViewModel>();
        }
    }
}
