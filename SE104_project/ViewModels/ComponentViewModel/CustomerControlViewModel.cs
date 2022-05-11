using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
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
        public bool isActivated { get; set; }
        public string address { get; set; }
        private IUpdateCustomerList _parent;
        #endregion

        #region ICommand
        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        #endregion
        public CustomerControlViewModel(CustomerInformation customer, IUpdateCustomerList parent)
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
            DeleteCustomerCommand = new RelayCommand<Object>(null, deteleCustomer);
            EditCustomerCommand = new RelayCommand<Object>(null, editCustomer);
        }

        #region Function
        private void deteleCustomer(Object o)
        {
            _parent.UpdateCustomerList(customer);
        }
        private void editCustomer(Object o)
        {
            _parent.EditCustomer(customer);
        }
        #endregion
    }
}
