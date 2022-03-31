using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class CustomerSelectMenu : MenuViewModel
    {
        public CustomerSelectMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<object>(null, change);
        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if(v != null && v.Name== "Customer")
            {
                Console.WriteLine("Customer");
            }
            if (v != null && v.Name == "Member")
            {
                Console.WriteLine("Member");
            }
            if (v != null && v.Name == "Provider")
            {
                Console.WriteLine("Provider");
            }

        }
    }
}
