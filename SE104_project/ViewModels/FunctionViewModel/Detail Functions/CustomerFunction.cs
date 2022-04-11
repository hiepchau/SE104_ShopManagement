using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class CustomerFunction : BaseFunction
    {

        #region ICommand
        //Customer
        public ICommand LoadCustomerCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand GoToNextPageCommandCus { get; set; }
        public ICommand GoToPreviousPageCommandCus { get; set; }
        public ICommand FindCustomerCommand { get; set; }
        public ICommand OpenAddCustomerControlCommand { get; set; }
        public ICommand SortCustomerCommand { get; set; }
        public ICommand CountCustomerCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand EditCommand { get; set; }

        //Membership
        public ICommand OpenMembershipWindowCommand { get; set; }


        //AddCustomer
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion

        public CustomerFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            OpenAddCustomerControlCommand = new RelayCommand<Object>(null, OpenAddCustomerControl);
        }

        public void OpenAddCustomerControl(Object o = null)
        {
            AddCustomerControl addCustomerControl = new AddCustomerControl();
            addCustomerControl.DataContext = this;
            DialogHost.Show(addCustomerControl);

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }

    }
}
