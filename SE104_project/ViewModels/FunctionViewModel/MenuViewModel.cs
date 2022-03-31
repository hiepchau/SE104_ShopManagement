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
        private BaseFunction _viewModel;
        private AppSession _session;
        private MongoConnect _mongoConnect;
        public ICommand ChangeViewModelCommand { get; set; }
        public MenuViewModel(BaseFunction viewmodel, AppSession session, MongoConnect connect)
        {
            _viewModel = viewmodel;
            _session = session;
            _mongoConnect = connect;
        }
        public virtual void change(object o) { }
        
    }
}
