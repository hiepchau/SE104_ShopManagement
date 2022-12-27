using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewState
    {
        public static MainViewModel Instance { get; private set; }
        public ViewModelBase CurrentMainViewModel
        {
            get => _viewState.CurrentMainViewModel;
            set
            {
                _viewState.CurrentMainViewModel = value;
                OnPropertyChanged(nameof(CurrentMainViewModel));
            }
        }

        

        public event Action StateChanged
        {
            add => _viewState.StateChanged += value;
            remove => _viewState.StateChanged -= value;
        }

        private readonly IViewState _viewState;

        public MainViewModel(IViewState viewState)
        {
            _viewState = viewState;
            //UpdateCurrentViewModelCommand<HomeViewModel> command = new UpdateCurrentViewModelCommand<HomeViewModel>(_viewState, factory);
            //command.Execute();
            /*UpdateCurrentViewModelCommand<AuthenticationViewModel> command = new UpdateCurrentViewModelCommand<AuthenticationViewModel>(_viewState, factory);
            command.Execute();

            AuthenticationViewModel authenticationViewModel = (AuthenticationViewModel) CurrentViewModel;
            UpdateCurrentViewModelCommand<LoginViewModel> loginCommand = new UpdateCurrentViewModelCommand<LoginViewModel>(authenticationViewModel, factory);
            loginCommand.Execute();*/
        }
    }
}
