using MongoDB.Driver;
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
        public string total { get; set; }
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
            customer = "Unknown";
            total = SeparateThousands(bill.total.ToString());
            displayID = bill.displayID;
            _parent = parent;
            GetEmployeeName();
            GetCustomerName();
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
            var filter = Builders<CustomerInformation>.Filter.Eq(x => x.PhoneNumber, billInformation.customer);
            GetCustomer getter = new GetCustomer((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var task = getter.Get();
            List<CustomerInformation> ls = new List<CustomerInformation>();
            ls = await task;
                if (ls.Count > 0)
                {
                    customer = ls.FirstOrDefault().Name;
                    OnPropertyChanged(nameof(customer));
                }
                else
                {
                    return;
                }
   
          
        }
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        #endregion
    }
}
