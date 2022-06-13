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
    public class FinanceMenu : MenuViewModel
    {
        public bool ismanager { get; set; }
        public FinanceMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<Object>(null, change);
            if(Utils.RoleSeperator.managerRole(_session))
                ismanager = true;
            else
                ismanager = false;
        }
        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Overall")
            {
                Console.WriteLine("Overall");
                _viewModel.Currentdisplaying = new FinanceOverViewFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Income")
            {
                Console.WriteLine("Income");
                _viewModel.Currentdisplaying = new IncomeFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Outcome")
            {
                Console.WriteLine("Outcome");
                _viewModel.Currentdisplaying = new SpendingFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
        }
    }
}
