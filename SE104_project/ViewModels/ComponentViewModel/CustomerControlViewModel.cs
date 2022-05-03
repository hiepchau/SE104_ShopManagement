using SE104_OnlineShopManagement.Models.ModelEntity;
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
        #endregion

        #region ICommand
        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        #endregion

        public CustomerControlViewModel(CustomerInformation customer)
        {
            ID = customer.ID;
            Name = customer.Name;
            PhoneNumber = customer.PhoneNumber;
            level = customer.CustomerLevel;
            cmnd = customer.CMND;
            displayId = customer.displayID;
            this.customer = customer;
        }
    }
}
