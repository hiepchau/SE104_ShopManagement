﻿using SE104_OnlineShopManagement.ViewModels.Authentication;
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
        public ViewModelFactory(ViewModelCreator<LoginViewModel> login, ViewModelCreator<RegisterViewModel> regist, ViewModelCreator<MainViewModel> main)
        {
            _loginViewModel = login;
            _registerViewModel = regist;
            _mainViewModel = main;
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
            return null;
        }
    }
}
