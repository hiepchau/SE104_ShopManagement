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

        private readonly ViewState _viewState;
        private readonly IViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(ViewState viewState, IViewModelFactory viewModelFactory)
        {
            _viewState = viewState;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = null)
        {
            _viewState.CurrentMainViewModel = _viewModelFactory.CreateViewModel<TViewModel>();
        }
    }
}
