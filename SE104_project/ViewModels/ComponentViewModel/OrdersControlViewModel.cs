﻿using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class OrdersControlViewModel : ViewModelBase
    {
        #region Properties
        public BillInformation billInformation { get; set; }
        public string ID { get; set; }
        public DateTime saleDay { get; set; }
        public string User { get; set; }
        public string customer { get; set; }
        public long total { get; set; }
        public string displayID { get; set; }
        private IOrdersParent _parent;

        #endregion

        #region ICommand
        public ICommand ViewOrdersCommand { get; set; }
        #endregion

        public OrdersControlViewModel(BillInformation bill, IOrdersParent parent)
        {
            this.billInformation = bill;
            ID = bill.ID;
            saleDay = bill.saleDay;
            customer = bill.customer;
            total = bill.total;
            displayID = bill.displayID;
            _parent = parent;
            GetEmployeeName();
        }

        #region Function
        public async void GetEmployeeName()
        {
            var filter = Builders<UserInfomation>.Filter.Eq(x => x.ID, billInformation.User);
            GetUsers getter = new GetUsers((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.get();
            if (ls != null && ls.Count > 0)
            {
                User = ls.First().LastName;
                OnPropertyChanged(nameof(User));
            }
            else
            {
                return;
            }
        }
        public async void GetCustomerName()
        {
            var filter = Builders<CustomerInformation>.Filter.Eq(x => x.ID, billInformation.customer);
            GetCustomer getter = new GetCustomer((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                customer = ls.First().Name;
                OnPropertyChanged(nameof(customer));
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}