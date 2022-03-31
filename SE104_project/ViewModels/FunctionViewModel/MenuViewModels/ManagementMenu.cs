using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class ManagementMenu : MenuViewModel
    {
        public ManagementMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<Object>(null, change);
        }

        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Overall")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new OverviewFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();

            }
            if (v != null && v.Name == "Orders")
            {
                Console.WriteLine(v.Name);
            }
            if (v != null && v.Name == "Products")
            {
                Console.WriteLine(v.Name);
            }
            if (v != null && v.Name == "Stock")
            {
                Console.WriteLine(v.Name);
            }
            if (v != null && v.Name == "Storage")
            {
                Console.WriteLine(v.Name);
            }
        }
    }
}
