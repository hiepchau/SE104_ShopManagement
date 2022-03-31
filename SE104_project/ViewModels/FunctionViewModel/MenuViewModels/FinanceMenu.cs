using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class FinanceMenu : MenuViewModel
    {
        public FinanceMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<Object>(null, change);
        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Overall")
            {
                Console.WriteLine("Overall");
            }
            if (v != null && v.Name == "Income")
            {
                Console.WriteLine("Income");
            }
            if (v != null && v.Name == "Outcome")
            {
                Console.WriteLine("Outcome");
            }
        }
    }
}
