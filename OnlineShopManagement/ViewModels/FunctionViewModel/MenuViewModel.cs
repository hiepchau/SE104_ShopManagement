using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel
{
    public class MenuViewModel:ViewModelBase
    {
        protected ManagingFunctionsViewModel _viewModel;
        protected AppSession _session;
        protected MongoConnect _mongoConnect;
        public ICommand ChangeViewModelCommand { get; set; }
        public MenuViewModel(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect)
        {
            _viewModel = viewmodel;
            _session = session;
            _mongoConnect = connect;
        }
        public virtual void change(object o) { }
        
    }
}
