using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class SettingMenu : MenuViewModel
    {
        public SettingMenu(BaseFunction viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<object>(null, change);

        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Employee")
            {
                Console.WriteLine(v.Name);
            }
            if (v != null && v.Name == "Store")
            {
                Console.WriteLine(v.Name);
            }
            
        }
    }
}
