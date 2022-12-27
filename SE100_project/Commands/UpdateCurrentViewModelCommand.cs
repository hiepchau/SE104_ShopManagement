using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.Commands
{
    public class UpdateCurrentViewModelCommand<TViewModel> : ICommand where TViewModel : ViewModelBase
    {
        public event EventHandler CanExecuteChanged;

        private readonly MainViewModel _mainviewModel;
        private readonly IViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(MainViewModel mainViewModel, IViewModelFactory viewModelFactory)
        {
            _mainviewModel = mainViewModel;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = null)
        {
            _mainviewModel.CurrentMainViewModel = _viewModelFactory.CreateViewModel<TViewModel>();
        }
    }
}
