using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class ReportMenu : MenuViewModel
    {
        public int _selectedItem = -1;
        public int selectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
            }
        }
        public ReportMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<object>(null, change);
        }
        public override void change(object o)
        {
            base.change(o);
        }
    }
}
