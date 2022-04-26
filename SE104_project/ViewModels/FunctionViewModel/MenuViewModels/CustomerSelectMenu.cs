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
    public class CustomerSelectMenu : MenuViewModel
    {
        public int selectedItem { get; set; }

        public CustomerSelectMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            selectedItem = -1;
            ChangeViewModelCommand = new RelayCommand<object>(null, change);
        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if(v != null && v.Name== "Customer")
            {
                Console.WriteLine("Customer");
                _viewModel.Currentdisplaying = new CustomerFunction(_session, _mongoConnect, _viewModel, this);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Member")
            {
                Console.WriteLine("Member");
                _viewModel.Currentdisplaying = new MembershipFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Provider")
            {
                Console.WriteLine("Provider");
                _viewModel.Currentdisplaying = new SupplierFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }

        }
        public void changeSelectedItem(int i)
        {
            selectedItem = i;
            OnPropertyChanged(nameof(selectedItem));
        }
    }
}
