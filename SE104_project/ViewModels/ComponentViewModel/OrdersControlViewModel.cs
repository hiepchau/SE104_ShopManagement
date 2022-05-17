using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
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
        #endregion

        #region ICommand
        public ICommand ViewOrdersCommand { get; set; }
        #endregion

        public OrdersControlViewModel(BillInformation bill)
        {
            this.billInformation = bill;
            ID = bill.ID;
            saleDay = bill.saleDay;
            User = bill.User;
            customer = bill.customer;
            total = bill.total;
            displayID = bill.displayID;
        }

        #region Function
        #endregion
    }
}
