using SE104_OnlineShopManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Services.Common
{
    public class ViewState : IViewState
    {
        private ViewModelBase _currentMainViewModel;
        public ViewModelBase CurrentMainViewModel { get => _currentMainViewModel;set { 
            _currentMainViewModel= value;
             StateChanged.Invoke();
            } }

        public event Action StateChanged;
    }
}
