using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    public delegate TViewModel ViewModelCreator<TViewModel>();
    public interface IViewModelFactory
    {
        public TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
    }
}
