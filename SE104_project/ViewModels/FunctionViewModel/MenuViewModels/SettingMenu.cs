using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class SettingMenu : MenuViewModel
    {
        public SettingMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<object>(null, change);

        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Employee")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new EmployeeFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Store")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new InfoStoreFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            
        }
    }
}
