using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
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
    class CustomerControlViewModel : ViewModelBase
    {
        #region Properties
        public CustomerInformation customer { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string level { get; set; }
        public string cmnd { get; set; }
        public string displayId { get; set; }
        public string membership { get; set; }
        public bool isActivated { get; set; }
        public string address { get; set; }
        public long Sum { get;set; }
        private IUpdateCustomerList _parent;
        #endregion

        #region ICommand
        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        #endregion
        public CustomerControlViewModel(CustomerInformation customer,long sum, IUpdateCustomerList parent)
        {
            ID = customer.ID;
            Name = customer.Name;
            PhoneNumber = customer.PhoneNumber;
            //level = customer.CustomerLevel;
            cmnd = customer.CMND;
            displayId = customer.displayID;
            address = customer.Address;
            
            this.customer = customer;
            _parent = parent;
            this.Sum = sum;
            EditCustomerCommand = new RelayCommand<Object>(null, editCustomer);
            GetMembershipName();
        }

        #region Function
        private void editCustomer(Object o)
        {
            _parent.EditCustomer(customer);
        }

        public async void GetMembershipName()
        {
            var filter = Builders<MembershipInformation>.Filter.Eq(x => x.ID, customer.CustomerLevel);
            GetMembership getter = new GetMembership((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                membership = ls.First().name;
                OnPropertyChanged(nameof(membership));
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
