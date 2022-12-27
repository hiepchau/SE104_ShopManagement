using SE104_OnlineShopManagement.ViewModels.Authentication;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using SE104_OnlineShopManagement.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    public class ViewModelFactory : IViewModelFactory
    {
        private ViewModelCreator<LoginViewModel> _loginViewModel;
        private ViewModelCreator<RegisterViewModel> _registerViewModel;
        private ViewModelCreator<MainViewModel> _mainViewModel;
        private ViewModelCreator<HomeViewModel> _homeViewModel;
        private ViewModelCreator<SellingViewModel> _sellingViewModel;
        public ViewModelFactory(ViewModelCreator<LoginViewModel> login, ViewModelCreator<RegisterViewModel> regist, ViewModelCreator<MainViewModel> main, ViewModelCreator<HomeViewModel> home, ViewModelCreator<SellingViewModel> selling)
        {
            _loginViewModel = login;
            _registerViewModel = regist;
            _mainViewModel = main;
            _homeViewModel = home;
            _sellingViewModel = selling;
        }
        public TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            Type type = typeof(TViewModel);
            if(type == typeof(LoginViewModel))
            {
                return (TViewModel)Convert.ChangeType(_loginViewModel.Invoke(), type);
            }
            if (type == typeof(RegisterViewModel)) 
            { 
                return (TViewModel)Convert.ChangeType(_registerViewModel.Invoke(), type);
            }
            if(type == typeof(MainViewModel))
            {
                return (TViewModel)Convert.ChangeType(_mainViewModel.Invoke(), type);
            }
            if (type == typeof(HomeViewModel))
            {
                return (TViewModel)Convert.ChangeType(_homeViewModel.Invoke(), type);
            }
            if(type == typeof(SellingViewModel))
                return (TViewModel)Convert.ChangeType(_sellingViewModel.Invoke(), type);
            return null;
        }
    }
}
