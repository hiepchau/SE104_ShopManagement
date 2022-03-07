using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.Views.Windows;
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
        public ICommand RegisterCommand1 { get;private set; }   
        public UpdateCurrentViewModelCommand<RegisterViewModel> RegisterCommand { get; set; }
        public LoginViewModel(IViewModelFactory factory)
        {
            _viewModelFactory = factory;
            //RegisterCommand = new UpdateCurrentViewModelCommand<RegisterViewModel>(factory.CreateViewModel<MainViewModel>(), factory);
            RegisterCommand1 = new RelayCommand<object>(null, OpenRegister);
        }

        public void OpenRegister(object o=null)
        {
            Trace.WriteLine("Command executed");
            _viewModelFactory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _viewModelFactory.CreateViewModel<RegisterViewModel>();
            //_authenticationWindow.DataContext = _viewModelFactory.CreateViewModel<MainViewModel>();
        }
    }
}
