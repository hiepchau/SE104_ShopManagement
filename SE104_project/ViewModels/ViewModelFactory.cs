using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    public class ViewModelFactory : IViewModelFactory
    {
        //private ViewModelCreator<> _SomeViewmodel;
        public ViewModelFactory()
        {

        }
        public TViewModel CreateViewModel<TViewModel>()
        {
            throw new NotImplementedException();
        }
    }
}
