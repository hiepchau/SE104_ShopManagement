using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.Authentication
{
    public class LoginViewModel:ViewModelBase
    {
        private IViewModelFactory _viewModelFactory;
        public UpdateCurrentViewModelCommand<RegisterViewModel> RegisterCommand { get; set; }
        public LoginViewModel(IViewModelFactory factory)
        {
            _viewModelFactory = factory;
            RegisterCommand = new UpdateCurrentViewModelCommand<RegisterViewModel>(factory.CreateViewModel<MainViewModel>(), factory);
        }

        
    }
}
