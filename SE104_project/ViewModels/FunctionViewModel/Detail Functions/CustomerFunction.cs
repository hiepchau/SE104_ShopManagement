﻿using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
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
        public int selectedItem { get; set; }

        private ManagingFunctionsViewModel managingFunction;
        private CustomerSelectMenu customerSelectMenu;

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
        public ICommand OpenMemberShipControlCommand { get; set; }


        //AddCustomer
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion

        public CustomerFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, CustomerSelectMenu _customerSelectMenu) : base(session, connect)
        {
            managingFunction = managingFunctionsViewModel;
            customerSelectMenu = _customerSelectMenu;
            OpenAddCustomerControlCommand = new RelayCommand<Object>(null, OpenAddCustomerControl);
            OpenMemberShipControlCommand = new RelayCommand<Object>(null, OpenMemberShipControl);

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
        public void OpenMemberShipControl(Object o = null)
        {
            managingFunction.Currentdisplaying = new MembershipFunction(Session, Connect);
            customerSelectMenu.changeSelectedItem(1);
            managingFunction.CurrentDisplayPropertyChanged();
        }

    }
}