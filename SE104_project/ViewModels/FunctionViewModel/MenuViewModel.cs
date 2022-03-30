using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel
{
    public class MenuViewModel:ViewModelBase
    {
        private ViewModelBase _viewModel;
        public ICommand ChangeViewModelCommand { get; set; }
        public MenuViewModel(ViewModelBase viewmodel)
        {
            _viewModel = viewmodel;
            ChangeViewModelCommand = new RelayCommand<object>(null, change);
        }
        public void change(object o) { }
        
    }
}
